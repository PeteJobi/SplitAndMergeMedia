using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SplitAndMerge
{
    public partial class MainForm : Form
    {
        static readonly string[] allowedExts = { ".mkv", ".mp4", ".mp3" };
        const int PROGRESS_MAX = 1_000_000;
        const string ffmpegPath = "ffmpeg.exe";
        Process? currentProcess;
        bool hasBeenKilled;
        bool isPaused;
        bool isSplitting;
        string pathForView;
        string[] filesCreated = Array.Empty<string>();
        List<SpecificSplitControls> specificSplitControls = new();
        private int incrementalSplitControlsDisplacement;
        private int fullSplitControlsDisplacement;

        public MainForm()
        {
            InitializeComponent();
            splitRadioButton.Checked = true;
            intervalRadioButton.Checked = true;
            overallProgressBar.Maximum = currentActionProgressBar.Maximum = PROGRESS_MAX;
            specificSplitParamsPanel.Top = intervalSplitParamsPanel.Top;
            specificSplitParamsPanel.Visible = false;
            incrementalSplitControlsDisplacement = startTextBox.Height + 3;
            specificSplitControls.Add(new SpecificSplitControls{ StartTextBox = startTextBox, EndTextBox = endTextBox });
            IntervalRadioButton_CheckedChanged(null, EventArgs.Empty);
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
            splitTypePanel.Enabled = true;
            splitTypePanel.Visible = true;
            splitTypeLabel.Visible = true;
            intervalSplitParamsPanel.Enabled = true;
            specificSplitParamsPanel.Enabled = true;
            ResetProgressUI(true);
            Height = splitRadioButton.Checked ? (intervalRadioButton.Checked ? 250 : 274 + fullSplitControlsDisplacement) : 158;
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
            splitTypePanel.Enabled = false;
            intervalSplitParamsPanel.Enabled = false;
            specificSplitParamsPanel.Enabled = false;
            Height = isSplitting ? 399 + (intervalRadioButton.Checked ? 0 : fullSplitControlsDisplacement) : 295;
            howLongLabel.Visible = isSplitting;
            progressPanel.Top = isSplitting ? 230 + (intervalRadioButton.Checked ? 0 : fullSplitControlsDisplacement) : 127;
            if (isSplitting)
            {
                fileNameLabel.Text = Path.GetFileName(fileNames[0]);
                await (intervalRadioButton.Checked ? IntervalSplit(fileNames[0]) : SpecificSplit(fileNames[0]));
            }
            else
            {
                splitTypePanel.Visible = false;
                splitTypeLabel.Visible = false;
                Array.Sort(fileNames);
                string fileNameNoExt = Path.GetFileNameWithoutExtension(fileNames[0]);
                string? num = fileNameNoExt.Contains("000") ? "000" : fileNameNoExt.Contains("001") ? "001" : null;
                string outputFileName = num != null ? fileNameNoExt.Remove(fileNameNoExt.LastIndexOf(num), num.Length) : fileNameNoExt + "_MERGED";
                string folder = Path.GetDirectoryName(fileNames[0]) ?? throw new NullReferenceException("The specified path is null");
                fileNameLabel.Text = outputFileName;
                outputFileName = pathForView = Path.Combine(folder, outputFileName + Path.GetExtension(fileNames[0]));
                await Merge(fileNames, outputFileName, folder);
            }
        }

        async Task IntervalSplit(string fileName)
        {
            var segmentDuration = TimeSpan.Parse($"{hourTextBox.Text}:{minuteTextBox.Text}:{secondTextBox.Text}");
            TimeSpan duration = TimeSpan.MinValue;
            int totalSegments = 0;
            int currentSegment = -1;
            await StartProcess(ffmpegPath, $"-i \"{fileName}\" -c copy -map 0 -segment_time {segmentDuration} -copyts -avoid_negative_ts make_non_negative -f segment -reset_timestamps 1 \"{GetOutputFolder(fileName)}/{ExtendedName(fileName, "%03d")}\"", null, (sender, args) =>
            {
                if (string.IsNullOrWhiteSpace(args.Data) || hasBeenKilled) return;
                if (duration == TimeSpan.MinValue)
                {
                    MatchCollection matchCollection = Regex.Matches(args.Data, @"\s*Duration:\s(\d{2}:\d{2}:\d{2}\.\d{2}).+");
                    if (matchCollection.Count == 0) return;
                    duration = TimeSpan.Parse(matchCollection[0].Groups[1].Value);
                    totalSegments = SetTotalProgressIntervalSplit(duration, segmentDuration);
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
                    IncrementIntervalSplitProgress(segmentDuration, TimeSpan.Parse(matchCollection[0].Groups[1].Value), duration, currentSegment, totalSegments);
                }
            });
            if (HasBeenKilled()) return;
            AllDone(totalSegments);
        }

        async Task SpecificSplit(string fileName)
        {
            var mediaDuration = TimeSpan.MinValue;
            var total = specificSplitControls.Count;
            await StartProcess(ffmpegPath, $"-i \"{fileName}\"", null, (sender, args) =>
            {
                if (string.IsNullOrWhiteSpace(args.Data) || hasBeenKilled) return;
                if (mediaDuration != TimeSpan.MinValue) return;
                var matchCollection = Regex.Matches(args.Data, @"\s*Duration:\s(\d{2}:\d{2}:\d{2}\.\d{2}).+");
                if (matchCollection.Count == 0) return;
                mediaDuration = TimeSpan.Parse(matchCollection[0].Groups[1].Value);
            });
            if (HasBeenKilled()) return;

            if (!ValidateSplitParams(mediaDuration))
            {
                Reset(null, EventArgs.Empty);
                return;
            }
            Invoke(() => totalSegmentCountLabel.Text = $"0/{total}");
            Invoke(() => currentFileLabel.Text = ExtendedName(fileName, 0.ToString("D3")));

            var totalDuration = specificSplitControls.Select(c => c.Duration).Aggregate((a, b) => a + b);
            var durationElapsed = TimeSpan.Zero;
            var folder = GetOutputFolder(fileName);
            for (var i = 0; i < specificSplitControls.Count; i++)
            {
                var controls = specificSplitControls[i];
                var current = i;
                Invoke(() => currentFileLabel.Text = ExtendedName(fileName, current.ToString("D3")));
                await StartProcess(ffmpegPath, $"-i \"{fileName}\" -c copy -map 0 -ss {controls.StartTextBox.Text} -to {controls.EndTextBox.Text} -avoid_negative_ts make_zero \"{folder}/{ExtendedName(fileName, current.ToString("D3"))}\"", null, (sender, args) =>
                {
                    if (string.IsNullOrWhiteSpace(args.Data) || hasBeenKilled) return;
                    if (!args.Data.StartsWith("frame")) return;
                    if (CheckNoSpaceDuringBreakMerge(args.Data)) return;
                    var matchCollection = Regex.Matches(args.Data, @"^frame=\s*\d+\s.+?time=(\d{2}:\d{2}:\d{2}\.\d{2}).+");
                    if (matchCollection.Count == 0) return;
                    IncrementSpecificSplitProgress(controls.Duration, TimeSpan.Parse(matchCollection[0].Groups[1].Value), durationElapsed, totalDuration, current, total);
                });
                if (HasBeenKilled()) return;
                durationElapsed += controls.Duration;
            }
            AllDone(total);
        }

        bool ValidateSplitParams(TimeSpan duration)
        {
            var errors = new List<string>();
            for (var i = 0; i < specificSplitControls.Count; i++)
            {
                var controls = specificSplitControls[i];

                if (!TimeSpan.TryParse(controls.StartTextBox.Text, out var startSpan) ||
                    !TimeSpan.TryParse(controls.EndTextBox.Text, out var endSpan) || startSpan > endSpan)
                {
                    errors.Add($"Range [{controls.StartTextBox.Text}, {controls.EndTextBox.Text}] contains invalid value(s)");
                    controls.Invalid = true;
                } else if (startSpan == endSpan)
                {
                    errors.Add($"Range [{controls.StartTextBox.Text}, {controls.EndTextBox.Text}] has no difference");
                    controls.Invalid = true;
                } else if (startSpan > duration || endSpan > duration)
                {
                    errors.Add($"Range [{controls.StartTextBox.Text}, {controls.EndTextBox.Text}] exceeds the duration of the video");
                    controls.Invalid = true;
                }
                else controls.Duration = endSpan - startSpan;
            }

            if (!errors.Any()) return true;
            var text = string.Join("/n", errors);
            MessageBox.Show(text, "Invalid split duration(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
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
            currentFileLabel.Text = Path.GetFileName(fileNames[currentSegment]);
            await StartProcess(ffmpegPath, $"-f concat -safe 0 -i \"{concatFileName}\" -c copy -map 0 \"{outputFileName}\"", null, (sender, args) =>
            {
                Debug.WriteLine(args.Data);
                if (string.IsNullOrWhiteSpace(args.Data) || hasBeenKilled) return;
                if (args.Data.StartsWith("frame"))
                {
                    if (CheckNoSpaceDuringBreakMerge(args.Data)) return;
                    MatchCollection matchCollection = Regex.Matches(args.Data, @"^frame=\s*\d+\s.+?time=(\d{2}:\d{2}:\d{2}\.\d{2}).+");
                    if (matchCollection.Count == 0) return;
                    TimeSpan currentTime = TimeSpan.Parse(matchCollection[0].Groups[1].Value);
                    if (currentTime > elapsedSegmentDurationSum)
                    {
                        currentSegment++;
                        elapsedSegmentDurationSum += segmentDurations[currentSegment];
                        Invoke(() => currentFileLabel.Text = Path.GetFileName(fileNames[currentSegment]));
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

        int SetTotalProgressIntervalSplit(TimeSpan fullDuration, TimeSpan segmentDuration)
        {
            double fraction = fullDuration / segmentDuration;
            int total = (int)Math.Ceiling(fraction);
            Invoke(() => totalSegmentCountLabel.Text = $"0/{total}");
            return total;
        }

        void IncrementIntervalSplitProgress(TimeSpan segmentDuration, TimeSpan currentTime, TimeSpan totalDuration, int currentSegment, int totalSegments)
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

        void IncrementSpecificSplitProgress(TimeSpan segmentDuration, TimeSpan currentTime, TimeSpan elapsedDuration, TimeSpan totalDuration, int currentSegment, int totalSegments)
        {
            Invoke(() =>
            {
                totalSegmentCountLabel.Text = $"{currentSegment}/{totalSegments}";
                overallProgressBar.Value = Math.Max(0, Math.Min((int)((currentTime + elapsedDuration) / totalDuration * PROGRESS_MAX), PROGRESS_MAX));
                double fraction = currentTime / segmentDuration;
                currentActionProgressBar.Value = (int)(fraction * PROGRESS_MAX);
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
                else if (File.Exists(path)) File.Delete(path);
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

        private void SplitRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            isSplitting = splitRadioButton.Checked;
            selectSingleFileButton.Visible = splitRadioButton.Checked;
            selectFilesButton.Visible = !splitRadioButton.Checked;
            selectFolderButton.Visible = !splitRadioButton.Checked;
            selectLabel.Text = splitRadioButton.Checked ? "Select a video to split" : "Select multiple videos or a folder with multiple videos to merge";
            intervalSplitParamsPanel.Visible = splitRadioButton.Checked && intervalRadioButton.Checked;
            specificSplitParamsPanel.Visible = splitRadioButton.Checked && !intervalRadioButton.Checked;
            progressPanel.Top = splitRadioButton.Checked ? 204 : 127;
            Height = splitRadioButton.Checked ? (intervalRadioButton.Checked ? 250 : 274 + fullSplitControlsDisplacement) : 158;
        }

        private void IntervalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            intervalSplitParamsPanel.Visible = intervalRadioButton.Checked;
            specificSplitParamsPanel.Visible = !intervalRadioButton.Checked;
            Height = intervalRadioButton.Checked ? 250 : 274 + fullSplitControlsDisplacement;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            fullSplitControlsDisplacement += incrementalSplitControlsDisplacement;
            var newStartLabel = new Label
            {
                Left = startLabel.Left,
                Top = startLabel.Top + fullSplitControlsDisplacement,
                Size = startLabel.Size,
                Text = startLabel.Text,
                Font = startLabel.Font,
                Parent = specificSplitParamsPanel,
            };
            var newStartTextBox = new TextBox
            {
                Top = startTextBox.Top + fullSplitControlsDisplacement,
                Left = startTextBox.Left,
                Size = startTextBox.Size,
                Text = "00:00:00",
                TextAlign = startTextBox.TextAlign,
                Parent = specificSplitParamsPanel,
            };
            var newEndLabel = new Label
            {
                Left = endLabel.Left,
                Top = endLabel.Top + fullSplitControlsDisplacement,
                Size = endLabel.Size,
                Text = endLabel.Text,
                Font = endLabel.Font,
                Parent = specificSplitParamsPanel,
            };
            var newEndTextBox = new TextBox
            {
                Top = endTextBox.Top + fullSplitControlsDisplacement,
                Left = endTextBox.Left,
                Size = endTextBox.Size,
                Text = "00:00:00",
                TextAlign = endTextBox.TextAlign,
                Parent = specificSplitParamsPanel,
            };
            var newRemove = new Button
            {
                Left = removeButton.Left,
                Top = removeButton.Top + fullSplitControlsDisplacement,
                Size = removeButton.Size,
                Text = removeButton.Text,
                Parent = specificSplitParamsPanel
            };
            addButton.Top += incrementalSplitControlsDisplacement;
            specificSplitParamsPanel.Height += incrementalSplitControlsDisplacement;
            Height += incrementalSplitControlsDisplacement;
            var newControls = new SpecificSplitControls
            {
                StartLabel = newStartLabel,
                StartTextBox = newStartTextBox,
                EndLabel = newEndLabel,
                EndTextBox = newEndTextBox,
                RemoveButton = newRemove,
                Index = specificSplitControls.Count
            };
            newControls.RemoveButton.Click += (_, _) => RemoveSpecificSplitControls(newControls);
            specificSplitControls.Add(newControls);
        }

        private void RemoveSpecificSplitControls(SpecificSplitControls controls)
        {
            specificSplitParamsPanel.Controls.Remove(controls.StartLabel);
            specificSplitParamsPanel.Controls.Remove(controls.StartTextBox);
            specificSplitParamsPanel.Controls.Remove(controls.EndLabel);
            specificSplitParamsPanel.Controls.Remove(controls.EndTextBox);
            specificSplitParamsPanel.Controls.Remove(controls.RemoveButton);

            for (var i = controls.Index + 1; i < specificSplitControls.Count; i++)
            {
                specificSplitControls[i].StartLabel.Top -= incrementalSplitControlsDisplacement;
                specificSplitControls[i].StartTextBox.Top -= incrementalSplitControlsDisplacement;
                specificSplitControls[i].EndLabel.Top -= incrementalSplitControlsDisplacement;
                specificSplitControls[i].EndTextBox.Top -= incrementalSplitControlsDisplacement;
                specificSplitControls[i].RemoveButton.Top -= incrementalSplitControlsDisplacement;
                specificSplitControls[i].Index--;
            }
            specificSplitControls.Remove(controls);

            fullSplitControlsDisplacement -= incrementalSplitControlsDisplacement;
            addButton.Top -= incrementalSplitControlsDisplacement;
            specificSplitParamsPanel.Height -= incrementalSplitControlsDisplacement;
            Height -= incrementalSplitControlsDisplacement;
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
            if (Directory.Exists(files[0]))
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

        class SpecificSplitControls
        {
            public Label StartLabel { get; set; }
            public TextBox StartTextBox { get; set; }
            public Label EndLabel { get; set; }
            public TextBox EndTextBox { get; set; }
            public Button RemoveButton { get; set; }
            public int Index { get; set; }
            public TimeSpan Duration { get; set; }
            public bool Invalid { get; set; }
        }
    }
}