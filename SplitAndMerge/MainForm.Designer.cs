namespace SplitAndMerge
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            fileDialogPanel = new Panel();
            selectFolderButton = new Button();
            selectSingleFileButton = new Button();
            selectFilesButton = new Button();
            openFileDialog = new OpenFileDialog();
            fileNameLabel = new Label();
            overallProgressBar = new ProgressBar();
            cancelButton = new Button();
            currentActionProgressBar = new ProgressBar();
            pauseButton = new Button();
            totalSegmentCountLabel = new Label();
            currentFileLabel = new Label();
            progressLabel = new Label();
            currentActionLabel = new Label();
            selectLabel = new Label();
            splitRadioButton = new RadioButton();
            actionTypePanel = new Panel();
            mergeRadioButton = new RadioButton();
            label2 = new Label();
            folderBrowserDialog = new FolderBrowserDialog();
            splitMergeProgressLabel = new Label();
            hourTextBox = new TextBox();
            howLongLabel = new Label();
            hourLabel = new Label();
            progressPanel = new Panel();
            splitParamsPanel = new Panel();
            secondLabel = new Label();
            minuteLabel = new Label();
            secondTextBox = new TextBox();
            minuteTextBox = new TextBox();
            fileDialogPanel.SuspendLayout();
            actionTypePanel.SuspendLayout();
            progressPanel.SuspendLayout();
            splitParamsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // fileDialogPanel
            // 
            fileDialogPanel.Anchor = AnchorStyles.Top;
            fileDialogPanel.Controls.Add(selectFolderButton);
            fileDialogPanel.Controls.Add(selectSingleFileButton);
            fileDialogPanel.Controls.Add(selectFilesButton);
            fileDialogPanel.Location = new Point(290, 12);
            fileDialogPanel.Name = "fileDialogPanel";
            fileDialogPanel.Size = new Size(213, 30);
            fileDialogPanel.TabIndex = 0;
            // 
            // selectFolderButton
            // 
            selectFolderButton.Location = new Point(109, 3);
            selectFolderButton.Name = "selectFolderButton";
            selectFolderButton.Size = new Size(100, 23);
            selectFolderButton.TabIndex = 1;
            selectFolderButton.Text = "Select folder";
            selectFolderButton.UseVisualStyleBackColor = true;
            selectFolderButton.Click += SelectFolderButton_Click;
            // 
            // selectSingleFileButton
            // 
            selectSingleFileButton.Location = new Point(51, 3);
            selectSingleFileButton.Name = "selectSingleFileButton";
            selectSingleFileButton.Size = new Size(100, 23);
            selectSingleFileButton.TabIndex = 0;
            selectSingleFileButton.Text = "Select file";
            selectSingleFileButton.UseVisualStyleBackColor = true;
            selectSingleFileButton.Click += SelectFile_Click;
            // 
            // selectFilesButton
            // 
            selectFilesButton.Location = new Point(3, 3);
            selectFilesButton.Name = "selectFilesButton";
            selectFilesButton.Size = new Size(100, 23);
            selectFilesButton.TabIndex = 0;
            selectFilesButton.Text = "Select files";
            selectFilesButton.UseVisualStyleBackColor = true;
            selectFilesButton.Click += SelectFiles_Click;
            // 
            // fileNameLabel
            // 
            fileNameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            fileNameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            fileNameLabel.Location = new Point(12, 16);
            fileNameLabel.Name = "fileNameLabel";
            fileNameLabel.Size = new Size(760, 22);
            fileNameLabel.TabIndex = 1;
            fileNameLabel.Text = "File Name";
            fileNameLabel.TextAlign = ContentAlignment.TopCenter;
            fileNameLabel.Visible = false;
            // 
            // overallProgressBar
            // 
            overallProgressBar.Location = new Point(0, 25);
            overallProgressBar.Name = "overallProgressBar";
            overallProgressBar.Size = new Size(668, 21);
            overallProgressBar.Style = ProgressBarStyle.Continuous;
            overallProgressBar.TabIndex = 2;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(685, 25);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 21);
            cancelButton.TabIndex = 7;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // currentActionProgressBar
            // 
            currentActionProgressBar.Location = new Point(0, 97);
            currentActionProgressBar.Name = "currentActionProgressBar";
            currentActionProgressBar.Size = new Size(668, 21);
            currentActionProgressBar.Style = ProgressBarStyle.Continuous;
            currentActionProgressBar.TabIndex = 2;
            // 
            // pauseButton
            // 
            pauseButton.Location = new Point(685, 97);
            pauseButton.Name = "pauseButton";
            pauseButton.Size = new Size(75, 21);
            pauseButton.TabIndex = 8;
            pauseButton.Text = "Pause";
            pauseButton.UseVisualStyleBackColor = true;
            // 
            // totalSegmentCountLabel
            // 
            totalSegmentCountLabel.AutoSize = true;
            totalSegmentCountLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            totalSegmentCountLabel.Location = new Point(0, 7);
            totalSegmentCountLabel.Name = "totalSegmentCountLabel";
            totalSegmentCountLabel.Size = new Size(34, 12);
            totalSegmentCountLabel.TabIndex = 4;
            totalSegmentCountLabel.Text = "21/181";
            totalSegmentCountLabel.TextAlign = ContentAlignment.BottomLeft;
            // 
            // currentFileLabel
            // 
            currentFileLabel.Anchor = AnchorStyles.Top;
            currentFileLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            currentFileLabel.Location = new Point(348, 0);
            currentFileLabel.Name = "currentFileLabel";
            currentFileLabel.Size = new Size(320, 20);
            currentFileLabel.TabIndex = 4;
            currentFileLabel.Text = "spiderman.png";
            currentFileLabel.TextAlign = ContentAlignment.BottomRight;
            // 
            // progressLabel
            // 
            progressLabel.AutoSize = true;
            progressLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            progressLabel.Location = new Point(0, 82);
            progressLabel.Name = "progressLabel";
            progressLabel.Size = new Size(34, 12);
            progressLabel.TabIndex = 4;
            progressLabel.Text = "21/181";
            progressLabel.TextAlign = ContentAlignment.BottomLeft;
            // 
            // currentActionLabel
            // 
            currentActionLabel.Anchor = AnchorStyles.Top;
            currentActionLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            currentActionLabel.Location = new Point(428, 74);
            currentActionLabel.Name = "currentActionLabel";
            currentActionLabel.Size = new Size(240, 20);
            currentActionLabel.TabIndex = 4;
            currentActionLabel.Text = "Breaking video into frames";
            currentActionLabel.TextAlign = ContentAlignment.BottomRight;
            // 
            // selectLabel
            // 
            selectLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            selectLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            selectLabel.ForeColor = SystemColors.ControlDarkDark;
            selectLabel.Location = new Point(12, 40);
            selectLabel.Name = "selectLabel";
            selectLabel.Size = new Size(760, 15);
            selectLabel.TabIndex = 1;
            selectLabel.Text = "Select a video to split";
            selectLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // splitRadioButton
            // 
            splitRadioButton.AutoSize = true;
            splitRadioButton.Location = new Point(3, 3);
            splitRadioButton.Name = "splitRadioButton";
            splitRadioButton.Size = new Size(48, 19);
            splitRadioButton.TabIndex = 2;
            splitRadioButton.TabStop = true;
            splitRadioButton.Text = "Split";
            splitRadioButton.UseVisualStyleBackColor = true;
            splitRadioButton.CheckedChanged += SplitRadioButton_CheckedChanged;
            // 
            // actionTypePanel
            // 
            actionTypePanel.Controls.Add(mergeRadioButton);
            actionTypePanel.Controls.Add(splitRadioButton);
            actionTypePanel.Location = new Point(329, 65);
            actionTypePanel.Name = "actionTypePanel";
            actionTypePanel.Size = new Size(126, 27);
            actionTypePanel.TabIndex = 6;
            // 
            // mergeRadioButton
            // 
            mergeRadioButton.AutoSize = true;
            mergeRadioButton.Location = new Point(64, 3);
            mergeRadioButton.Name = "mergeRadioButton";
            mergeRadioButton.Size = new Size(59, 19);
            mergeRadioButton.TabIndex = 3;
            mergeRadioButton.TabStop = true;
            mergeRadioButton.Text = "Merge";
            mergeRadioButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label2.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = SystemColors.ControlDarkDark;
            label2.Location = new Point(12, 95);
            label2.Name = "label2";
            label2.Size = new Size(760, 15);
            label2.TabIndex = 1;
            label2.Text = "Are you splitting a media into multiple segments or are you merging multiple segments into a media?";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // splitMergeProgressLabel
            // 
            splitMergeProgressLabel.Anchor = AnchorStyles.Top;
            splitMergeProgressLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            splitMergeProgressLabel.Location = new Point(192, 74);
            splitMergeProgressLabel.Name = "splitMergeProgressLabel";
            splitMergeProgressLabel.Size = new Size(295, 20);
            splitMergeProgressLabel.TabIndex = 4;
            splitMergeProgressLabel.Text = "33.33%";
            splitMergeProgressLabel.TextAlign = ContentAlignment.BottomCenter;
            // 
            // hourTextBox
            // 
            hourTextBox.Location = new Point(244, 2);
            hourTextBox.Name = "hourTextBox";
            hourTextBox.Size = new Size(35, 23);
            hourTextBox.TabIndex = 9;
            hourTextBox.Text = "0";
            hourTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // howLongLabel
            // 
            howLongLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            howLongLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            howLongLabel.ForeColor = SystemColors.ControlDarkDark;
            howLongLabel.Location = new Point(12, 163);
            howLongLabel.Name = "howLongLabel";
            howLongLabel.Size = new Size(760, 15);
            howLongLabel.TabIndex = 1;
            howLongLabel.Text = "How long do you want the segments to be?";
            howLongLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // hourLabel
            // 
            hourLabel.AutoSize = true;
            hourLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            hourLabel.Location = new Point(285, 7);
            hourLabel.Name = "hourLabel";
            hourLabel.Size = new Size(30, 12);
            hourLabel.TabIndex = 4;
            hourLabel.Text = "hours";
            hourLabel.TextAlign = ContentAlignment.BottomLeft;
            // 
            // progressPanel
            // 
            progressPanel.Controls.Add(overallProgressBar);
            progressPanel.Controls.Add(currentActionProgressBar);
            progressPanel.Controls.Add(cancelButton);
            progressPanel.Controls.Add(splitMergeProgressLabel);
            progressPanel.Controls.Add(pauseButton);
            progressPanel.Controls.Add(currentActionLabel);
            progressPanel.Controls.Add(totalSegmentCountLabel);
            progressPanel.Controls.Add(currentFileLabel);
            progressPanel.Controls.Add(progressLabel);
            progressPanel.Location = new Point(12, 204);
            progressPanel.Name = "progressPanel";
            progressPanel.Size = new Size(760, 131);
            progressPanel.TabIndex = 10;
            // 
            // splitParamsPanel
            // 
            splitParamsPanel.Controls.Add(secondLabel);
            splitParamsPanel.Controls.Add(minuteLabel);
            splitParamsPanel.Controls.Add(secondTextBox);
            splitParamsPanel.Controls.Add(minuteTextBox);
            splitParamsPanel.Controls.Add(hourLabel);
            splitParamsPanel.Controls.Add(hourTextBox);
            splitParamsPanel.Location = new Point(12, 127);
            splitParamsPanel.Name = "splitParamsPanel";
            splitParamsPanel.Size = new Size(760, 32);
            splitParamsPanel.TabIndex = 11;
            // 
            // secondLabel
            // 
            secondLabel.AutoSize = true;
            secondLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            secondLabel.Location = new Point(493, 7);
            secondLabel.Name = "secondLabel";
            secondLabel.Size = new Size(41, 12);
            secondLabel.TabIndex = 4;
            secondLabel.Text = "seconds";
            secondLabel.TextAlign = ContentAlignment.BottomLeft;
            // 
            // minuteLabel
            // 
            minuteLabel.AutoSize = true;
            minuteLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            minuteLabel.Location = new Point(384, 7);
            minuteLabel.Name = "minuteLabel";
            minuteLabel.Size = new Size(40, 12);
            minuteLabel.TabIndex = 4;
            minuteLabel.Text = "minutes";
            minuteLabel.TextAlign = ContentAlignment.BottomLeft;
            // 
            // secondTextBox
            // 
            secondTextBox.Location = new Point(452, 2);
            secondTextBox.Name = "secondTextBox";
            secondTextBox.Size = new Size(35, 23);
            secondTextBox.TabIndex = 9;
            secondTextBox.Text = "0";
            secondTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // minuteTextBox
            // 
            minuteTextBox.Location = new Point(343, 2);
            minuteTextBox.Name = "minuteTextBox";
            minuteTextBox.Size = new Size(35, 23);
            minuteTextBox.TabIndex = 9;
            minuteTextBox.Text = "20";
            minuteTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 334);
            Controls.Add(howLongLabel);
            Controls.Add(splitParamsPanel);
            Controls.Add(progressPanel);
            Controls.Add(actionTypePanel);
            Controls.Add(label2);
            Controls.Add(selectLabel);
            Controls.Add(fileNameLabel);
            Controls.Add(fileDialogPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Split And Merge";
            FormClosing += MainForm_FormClosing;
            DragDrop += MainForm_DragDrop;
            DragEnter += MainForm_DragEnter;
            fileDialogPanel.ResumeLayout(false);
            actionTypePanel.ResumeLayout(false);
            actionTypePanel.PerformLayout();
            progressPanel.ResumeLayout(false);
            progressPanel.PerformLayout();
            splitParamsPanel.ResumeLayout(false);
            splitParamsPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel fileDialogPanel;
        private Button selectFolderButton;
        private Button selectFilesButton;
        private OpenFileDialog openFileDialog;
        private Label fileNameLabel;
        private ProgressBar overallProgressBar;
        private Button cancelButton;
        private ProgressBar currentActionProgressBar;
        private Button pauseButton;
        private Label totalSegmentCountLabel;
        private Label currentFileLabel;
        private Label progressLabel;
        private Label currentActionLabel;
        private Label selectLabel;
        private RadioButton splitRadioButton;
        private Panel actionTypePanel;
        private RadioButton mergeRadioButton;
        private Label label2;
        private FolderBrowserDialog folderBrowserDialog;
        private Label splitMergeProgressLabel;
        private TextBox hourTextBox;
        private Label howLongLabel;
        private Label hourLabel;
        private Panel progressPanel;
        private Panel splitParamsPanel;
        private Label secondLabel;
        private Label minuteLabel;
        private TextBox secondTextBox;
        private TextBox minuteTextBox;
        private Button selectSingleFileButton;
    }
}