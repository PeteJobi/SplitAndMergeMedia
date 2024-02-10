using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace SplitAndMerge
{
    public partial class MainForm : Form
    {
        static readonly string[] allowedExts = new[] { ".mkv", ".mp4", ".mp3" };
        const int PROGRESS_MAX = 1_000_000;
        const string ffmpegPath = "ffmpeg.exe";
        Process? currentProcess;
        bool hasBeenKilled;
        bool isPaused;
        bool isSplitting;
        string pathForView;
        string[] filesCreated = Array.Empty<string>();
        public MainForm()
        {
            InitializeComponent();
            splitRadioButton.Checked = true;
            overallProgressBar.Maximum = currentActionProgressBar.Maximum = PROGRESS_MAX;
            Reset(null, EventArgs.Empty);
        }

        private void Reset(object? sender, EventArgs e)
        {
            totalSegmentCountLabel.Text = string.Empty;
            currentFileLabel.Text = string.Empty;
            currentActionLabel.Text = string.Empty;
            progressLabel.Text = string.Empty;
            cancelButton.Text = "Cancel";
            pauseButton.Text = "Pause";
            cancelButton.Click += CancelButton_Click;
            cancelButton.Click -= Reset;
            pauseButton.Click += pauseButton_Click;
            pauseButton.Click -= ViewFiles;
            fileDialogPanel.Show();
            selectLabel.Show();
            fileNameLabel.Hide();
            overallProgressBar.Value = 0;
            currentActionProgressBar.Value = 0;
            actionTypePanel.Enabled = true;
            splitParamsPanel.Enabled = true;
            ResetProgressUI(true);
            Height = splitRadioButton.Checked ? 224 : 160;
        }

        void ResetProgressUI(bool show)
        {
            currentActionProgressBar.Value = 0;
            splitMergeProgressLabel.Visible = show;
            splitMergeProgressLabel.Text = string.Empty;
        }

        async Task PrepareFiles(string[] fileNames)
        {
            fileDialogPanel.Hide();
            selectLabel.Hide();
            fileNameLabel.Show();
            actionTypePanel.Enabled = false;
            splitParamsPanel.Enabled = false;
            Height = isSplitting ? 373 : 295;
            howLongLabel.Visible = isSplitting;
            progressPanel.Top = isSplitting ? 204 : 127;
            if (isSplitting)
            {
                fileNameLabel.Text = Path.GetFileName(fileNames[0]);
                TimeSpan segmentDuration = TimeSpan.Parse($"{hourTextBox.Text}:{minuteTextBox.Text}:{secondTextBox.Text}");
                await Split(fileNames[0], segmentDuration);
            }
            else
            {
                Array.Sort(fileNames);
                string fileNameNoExt = Path.GetFileNameWithoutExtension(fileNames[0]);
                string outputFileName = fileNameNoExt.EndsWith("000") ? fileNameNoExt[0..^3] : fileNameNoExt;
                string folder = Path.GetDirectoryName(fileNames[0]) ?? throw new NullReferenceException("The specified path is null");
                fileNameLabel.Text = outputFileName;
                outputFileName = pathForView = Path.Combine(folder, outputFileName + Path.GetExtension(fileNames[0]));
                await Merge(fileNames, outputFileName, folder);
            }
        }

        async Task Split(string fileName, TimeSpan segmentDuration)
        {
            TimeSpan duration = TimeSpan.MinValue;
            int totalSegments = 0;
            int currentSegment = -1;
            await StartProcess(ffmpegPath, $"-i \"{fileName}\" -c copy -map 0 -segment_time {segmentDuration} -f segment -reset_timestamps 1 \"{GetOutputFolder(fileName)}/{ExtendedName(fileName, "%03d")}\"", null, (sender, args) =>
            {
                if (string.IsNullOrWhiteSpace(args.Data) || hasBeenKilled) return;
                if (duration == TimeSpan.MinValue)
                {
                    MatchCollection matchCollection = Regex.Matches(args.Data, @"\s*Duration:\s(\d{2}:\d{2}:\d{2}\.\d{2}).+");
                    if (matchCollection.Count == 0) return;
                    duration = TimeSpan.Parse(matchCollection[0].Groups[1].Value);
                    totalSegments = SetTotalProgress(duration, segmentDuration);
                }
                else if (args.Data.StartsWith("[segment @"))
                {
                    currentSegment++;
                    Invoke(() => currentFileLabel.Text = ExtendedName(fileName, currentSegment.ToString("D3")));
                }
                else if (args.Data.StartsWith("frame"))
                {
                    if (CheckNoSpaceDuringBreakMerge(args.Data)) return;
                    MatchCollection matchCollection = Regex.Matches(args.Data, @"^frame=\s*\d+\s.+?time=(\d{2}:\d{2}:\d{2}\.\d{2}).+");
                    if (matchCollection.Count == 0) return;
                    IncrementSplitProgress(segmentDuration, TimeSpan.Parse(matchCollection[0].Groups[1].Value), duration, currentSegment, totalSegments);
                }
            });
            if (HasBeenKilled()) return;
            AllDone(totalSegments);
        }

        async Task Merge(string[] fileNames, string outputFileName, string folder)
        {
            List<TimeSpan> segmentDurations = new();
            await StartProcess(ffmpegPath, string.Join(" ", fileNames.Select(name => $"-i \"{name}\"")), null, (sender, args) =>
            {
                if (string.IsNullOrWhiteSpace(args.Data) || hasBeenKilled) return;
                MatchCollection matchCollection = Regex.Matches(args.Data, @"\s*Duration:\s(\d{2}:\d{2}:\d{2}\.\d{2}),.+");
                if (matchCollection.Count == 0) return;
                segmentDurations.Add(TimeSpan.Parse(matchCollection[0].Groups[1].Value));
            });
            if (HasBeenKilled()) return;

            string concatFileName = Path.Combine(folder, Path.GetFileNameWithoutExtension(outputFileName) + "_Concat.txt");
            using (StreamWriter writer = new(File.Create(concatFileName)))
            {
                foreach (string fileName in fileNames) await writer.WriteLineAsync($"file '{fileName}'");
            }
            filesCreated = new[] { outputFileName, concatFileName };

            int currentSegment = 0;
            TimeSpan elapsedSegmentDurationSum = segmentDurations[currentSegment];
            TimeSpan totalDuration = segmentDurations.Aggregate((curr, prev) => curr + prev);
            await StartProcess(ffmpegPath, $"-f concat -safe 0 -i \"{concatFileName}\" -c copy \"{outputFileName}\"", null, (sender, args) =>
            {
                if (string.IsNullOrWhiteSpace(args.Data) || hasBeenKilled) return;
                if (args.Data.StartsWith("frame"))
                {
                    Debug.WriteLine(args.Data);
                    if (CheckNoSpaceDuringBreakMerge(args.Data)) return;
                    MatchCollection matchCollection = Regex.Matches(args.Data, @"^frame=\s*\d+\s.+?time=(\d{2}:\d{2}:\d{2}\.\d{2}).+");
                    if (matchCollection.Count == 0) return;
                    TimeSpan currentTime = TimeSpan.Parse(matchCollection[0].Groups[1].Value);
                    if (currentTime > elapsedSegmentDurationSum)
                    {
                        currentSegment++;
                        elapsedSegmentDurationSum += segmentDurations[currentSegment];
                    }
                    TimeSpan segmentDuration = segmentDurations[currentSegment];
                    int totalSegments = segmentDurations.Count;
                    IncrementMergeProgress(currentTime, segmentDurations, totalDuration, currentSegment);
                }
            });
            if (HasBeenKilled()) return;
            AllDone(segmentDurations.Count);
            File.Delete(concatFileName);
        }

        string ExtendedName(string fileName, string extra) => $"{Path.GetFileNameWithoutExtension(fileName)}{extra}{Path.GetExtension(fileName)}";

        int SetTotalProgress(TimeSpan fullDuration, TimeSpan segmentDuration)
        {
            double fraction = fullDuration / segmentDuration;
            int total = (int)Math.Ceiling(fraction);
            Invoke(() => totalSegmentCountLabel.Text = $"0/{total}");
            return total;
        }

        void IncrementSplitProgress(TimeSpan segmentDuration, TimeSpan currentTime, TimeSpan totalDuration, int currentSegment, int totalSegments)
        {
            Invoke(() =>
            {
                totalSegmentCountLabel.Text = $"{currentSegment}/{totalSegments}";
                overallProgressBar.Value = (int)(currentTime / totalDuration * PROGRESS_MAX);
                TimeSpan currentSegmentDuration = currentSegment < totalSegments - 1 ? segmentDuration : totalDuration - (currentSegment * segmentDuration);
                if (currentSegment == totalSegments - 1) Debug.WriteLine(currentSegmentDuration);
                double fraction = (currentTime - (currentSegment * segmentDuration)) / currentSegmentDuration;
                currentActionProgressBar.Value = Math.Max(0, Math.Min((int)(fraction * PROGRESS_MAX), PROGRESS_MAX));
                splitMergeProgressLabel.Text = $"{Math.Round(fraction * 100, 2)} %";
            });
        }

        void IncrementMergeProgress(TimeSpan currentTime, List<TimeSpan> segmentDurations, TimeSpan totalDuration, int currentSegment)
        {
            TimeSpan segmentDuration = segmentDurations[currentSegment];
            int totalSegments = segmentDurations.Count;
            Invoke(() =>
            {
                totalSegmentCountLabel.Text = $"{currentSegment}/{totalSegments}";
                overallProgressBar.Value = (int)(currentTime / totalDuration * PROGRESS_MAX);
                TimeSpan currentSegmentDuration = currentSegment < totalSegments - 1 ? segmentDuration : totalDuration - (currentSegment * segmentDuration);
                if (currentSegment == totalSegments - 1) Debug.WriteLine(currentSegmentDuration);
                double fraction = (currentTime - (currentSegment * segmentDuration)) / currentSegmentDuration;
                currentActionProgressBar.Value = Math.Max(0, Math.Min((int)(fraction * PROGRESS_MAX), PROGRESS_MAX));
                splitMergeProgressLabel.Text = $"{Math.Round(fraction * 100, 2)} %";
            });
        }

        bool CheckNoSpaceDuringBreakMerge(string line)
        {
            if (!line.EndsWith("No space left on device") && !line.EndsWith("I/O error")) return false;
            SuspendProcess(currentProcess);
            MessageBox.Show($"Process failed.\nError message: {line}");
            Invoke(() => Cancel(true));
            return true;
        }

        string GetOutputFolder(string path)
        {
            string inputName = Path.GetFileNameWithoutExtension(path);
            string parentFolder = Path.GetDirectoryName(path) ?? throw new NullReferenceException("The specified path is null");
            string outputFolder = Path.Combine(parentFolder, $"{inputName}_SplitVideos");
            pathForView = outputFolder;
            if (Directory.Exists(outputFolder))
            {
                Directory.Delete(outputFolder, true);
            }
            Directory.CreateDirectory(outputFolder);
            filesCreated = new[] { outputFolder };
            return outputFolder;
        }

        void AllDone(int totalSegments)
        {
            currentProcess = null;
            overallProgressBar.Value = overallProgressBar.Maximum;
            currentActionProgressBar.Value = currentActionProgressBar.Maximum;
            totalSegmentCountLabel.Text = $"{totalSegments}/{totalSegments}";
            splitMergeProgressLabel.Text = "100 %";
            currentActionLabel.Text = "Done";
            cancelButton.Text = "Retry";
            cancelButton.Click -= CancelButton_Click;
            cancelButton.Click += Reset;
            pauseButton.Text = "View";
            pauseButton.Click -= pauseButton_Click;
            pauseButton.Click += ViewFiles;
        }

        private void ViewFiles(object? sender, EventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "explorer";
            info.Arguments = $"/e, /select, \"{pathForView}\"";
            Process.Start(info);
        }

        bool HasBeenKilled()
        {
            if (!hasBeenKilled) return false;
            hasBeenKilled = false;
            return true;
        }

        void Cancel(bool dontWaitForExit = false)
        {
            currentProcess.Kill();
            if (!dontWaitForExit) currentProcess.WaitForExit();
            hasBeenKilled = true;
            cancelButton.Click -= CancelButton_Click;
            pauseButton.Click -= pauseButton_Click;
            Reset(null, EventArgs.Empty);
            currentProcess = null;
            foreach (string path in filesCreated)
            {
                if (Directory.Exists(path)) Directory.Delete(path, true);
                else if(File.Exists(path)) File.Delete(path);
            }
        }

        bool ConfirmCancel()
        {
            const string message = "Are you sure that you would like to cancel the process?";
            string caption = $"Cancel {(isSplitting ? "splitting" : "merging")} task";
            if (!isPaused) SuspendProcess(currentProcess);
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            bool confirm = result == DialogResult.Yes;
            if (!confirm && !isPaused) ResumeProcess(currentProcess);
            return confirm;
        }

        private void SplitRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            isSplitting = splitRadioButton.Checked;
            selectSingleFileButton.Visible = splitRadioButton.Checked;
            selectFilesButton.Visible = !splitRadioButton.Checked;
            selectFolderButton.Visible = !splitRadioButton.Checked;
            selectLabel.Text = splitRadioButton.Checked ? "Select a video to split" : "Select multiple videos or a folder with multiple videos to merge";
            splitParamsPanel.Visible = splitRadioButton.Checked;
            progressPanel.Top = splitRadioButton.Checked ? 204 : 127;
            Height = splitRadioButton.Checked ? 224 : 160;
        }

        private async void SelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Select a video";
            openFileDialog.Filter = "Video File|" + string.Join(";", allowedExts.Select(e => $"*{e}"));
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            await PrepareFiles(openFileDialog.FileNames);
        }

        private async void SelectFiles_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Select multiple videos";
            openFileDialog.Filter = "Video Files|" + string.Join(";", allowedExts.Select(e => $"*{e}"));
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            if (openFileDialog.FileNames.Length <= 1)
            {
                MessageBox.Show("Please select more than one video");
                return;
            }
            await PrepareFiles(openFileDialog.FileNames);
        }

        private async void SelectFolderButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = "Select a folder that contains images and/or videos";
            folderBrowserDialog.UseDescriptionForTitle = true;
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
            string[] filePaths = Directory.GetFiles(folderBrowserDialog.SelectedPath).Where(p => allowedExts.Contains(Path.GetExtension(p).ToLower())).ToArray();
            if (filePaths.Length < 2)
            {
                MessageBox.Show("The selected folder does not contain enough supported files");
                return;
            }
            await PrepareFiles(filePaths);
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            if (ConfirmCancel()) Cancel();
        }

        private void pauseButton_Click(object? sender, EventArgs e)
        {
            if (isPaused)
            {
                ResumeProcess(currentProcess);
                pauseButton.Text = "Pause";
                isPaused = false;
            }
            else
            {
                SuspendProcess(currentProcess);
                pauseButton.Text = "Resume";
                isPaused = true;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (currentProcess == null) return;

            if (ConfirmCancel()) Cancel();
            else e.Cancel = true;
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (!fileDialogPanel.Visible) return;
            e.Effect = DragDropEffects.All;
        }

        private async void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (!fileDialogPanel.Visible) return;
            string[]? files = ((string[]?)e.Data?.GetData(DataFormats.FileDrop, false));
            if (files?.Length < 1) return;
            if (Path.GetExtension(files[0]) == string.Empty)
            {
                files = Directory.GetFiles(files[0]);
            }
            files = files.Where(p => allowedExts.Contains(Path.GetExtension(p).ToLower())).ToArray();
            if (!isSplitting && files?.Length < 2)
            {
                MessageBox.Show("The selected folder does not contain enough supported files");
                return;
            }
            else await PrepareFiles(files);
        }

        async Task StartProcess(string processFileName, string arguments, DataReceivedEventHandler? outputEventHandler, DataReceivedEventHandler? errorEventHandler)
        {
            Process ffmpeg = new()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = processFileName,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                },
                EnableRaisingEvents = true
            };
            ffmpeg.OutputDataReceived += outputEventHandler;
            ffmpeg.ErrorDataReceived += errorEventHandler;
            ffmpeg.Start();
            ffmpeg.BeginErrorReadLine();
            ffmpeg.BeginOutputReadLine();
            currentProcess = ffmpeg;
            await ffmpeg.WaitForExitAsync();
            ffmpeg.Dispose();
        }

        [Flags]
        public enum ThreadAccess : int
        {
            SUSPEND_RESUME = (0x0002)
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        private static void SuspendProcess(Process process)
        {
            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                SuspendThread(pOpenThread);

                CloseHandle(pOpenThread);
            }
        }

        public static void ResumeProcess(Process process)
        {
            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                int suspendCount;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);
            }
        }
    }
}