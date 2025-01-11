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
            intervalSplitParamsPanel = new Panel();
            secondLabel = new Label();
            minuteLabel = new Label();
            secondTextBox = new TextBox();
            minuteTextBox = new TextBox();
            splitTypePanel = new Panel();
            specificRadioButton = new RadioButton();
            intervalRadioButton = new RadioButton();
            splitTypeLabel = new Label();
            specificSplitParamsPanel = new Panel();
            addButton = new Button();
            endTextBox = new TextBox();
            endLabel = new Label();
            removeButton = new Button();
            startTextBox = new TextBox();
            startLabel = new Label();
            fileDialogPanel.SuspendLayout();
            actionTypePanel.SuspendLayout();
            progressPanel.SuspendLayout();
            intervalSplitParamsPanel.SuspendLayout();
            splitTypePanel.SuspendLayout();
            specificSplitParamsPanel.SuspendLayout();
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
            howLongLabel.Location = new Point(12, 210);
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
            progressPanel.Location = new Point(12, 279);
            progressPanel.Name = "progressPanel";
            progressPanel.Size = new Size(760, 131);
            progressPanel.TabIndex = 10;
            // 
            // intervalSplitParamsPanel
            // 
            intervalSplitParamsPanel.Controls.Add(secondLabel);
            intervalSplitParamsPanel.Controls.Add(minuteLabel);
            intervalSplitParamsPanel.Controls.Add(secondTextBox);
            intervalSplitParamsPanel.Controls.Add(minuteTextBox);
            intervalSplitParamsPanel.Controls.Add(hourLabel);
            intervalSplitParamsPanel.Controls.Add(hourTextBox);
            intervalSplitParamsPanel.Location = new Point(12, 174);
            intervalSplitParamsPanel.Name = "intervalSplitParamsPanel";
            intervalSplitParamsPanel.Size = new Size(760, 32);
            intervalSplitParamsPanel.TabIndex = 11;
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
            minuteTextBox.Text = "10";
            minuteTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // splitTypePanel
            // 
            splitTypePanel.Controls.Add(specificRadioButton);
            splitTypePanel.Controls.Add(intervalRadioButton);
            splitTypePanel.Location = new Point(318, 113);
            splitTypePanel.Name = "splitTypePanel";
            splitTypePanel.Size = new Size(147, 27);
            splitTypePanel.TabIndex = 8;
            // 
            // specificRadioButton
            // 
            specificRadioButton.AutoSize = true;
            specificRadioButton.Location = new Point(77, 3);
            specificRadioButton.Name = "specificRadioButton";
            specificRadioButton.Size = new Size(66, 19);
            specificRadioButton.TabIndex = 3;
            specificRadioButton.TabStop = true;
            specificRadioButton.Text = "Specific";
            specificRadioButton.UseVisualStyleBackColor = true;
            // 
            // intervalRadioButton
            // 
            intervalRadioButton.AutoSize = true;
            intervalRadioButton.Location = new Point(4, 3);
            intervalRadioButton.Name = "intervalRadioButton";
            intervalRadioButton.Size = new Size(64, 19);
            intervalRadioButton.TabIndex = 2;
            intervalRadioButton.TabStop = true;
            intervalRadioButton.Text = "Interval";
            intervalRadioButton.UseVisualStyleBackColor = true;
            intervalRadioButton.CheckedChanged += IntervalRadioButton_CheckedChanged;
            // 
            // splitTypeLabel
            // 
            splitTypeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            splitTypeLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            splitTypeLabel.ForeColor = SystemColors.ControlDarkDark;
            splitTypeLabel.Location = new Point(12, 143);
            splitTypeLabel.Name = "splitTypeLabel";
            splitTypeLabel.Size = new Size(760, 15);
            splitTypeLabel.TabIndex = 7;
            splitTypeLabel.Text = "Are you splitting ar regular intervals or are you extracting at arbitrary time frames?";
            splitTypeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // specificSplitParamsPanel
            // 
            specificSplitParamsPanel.Controls.Add(addButton);
            specificSplitParamsPanel.Controls.Add(endTextBox);
            specificSplitParamsPanel.Controls.Add(endLabel);
            specificSplitParamsPanel.Controls.Add(removeButton);
            specificSplitParamsPanel.Controls.Add(startTextBox);
            specificSplitParamsPanel.Controls.Add(startLabel);
            specificSplitParamsPanel.Location = new Point(12, 212);
            specificSplitParamsPanel.Name = "specificSplitParamsPanel";
            specificSplitParamsPanel.Size = new Size(760, 61);
            specificSplitParamsPanel.TabIndex = 12;
            // 
            // addButton
            // 
            addButton.Location = new Point(334, 31);
            addButton.Name = "addButton";
            addButton.Size = new Size(75, 21);
            addButton.TabIndex = 12;
            addButton.Text = "Add";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += AddButton_Click;
            // 
            // endTextBox
            // 
            endTextBox.Location = new Point(370, 2);
            endTextBox.Name = "endTextBox";
            endTextBox.Size = new Size(59, 23);
            endTextBox.TabIndex = 11;
            endTextBox.Text = "00:00:00";
            endTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // endLabel
            // 
            endLabel.AutoSize = true;
            endLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            endLabel.Location = new Point(334, 7);
            endLabel.Name = "endLabel";
            endLabel.Size = new Size(22, 12);
            endLabel.TabIndex = 10;
            endLabel.Text = "End";
            endLabel.TextAlign = ContentAlignment.BottomLeft;
            // 
            // removeButton
            // 
            removeButton.Enabled = false;
            removeButton.Location = new Point(452, 3);
            removeButton.Name = "removeButton";
            removeButton.Size = new Size(75, 21);
            removeButton.TabIndex = 9;
            removeButton.Text = "Remove";
            removeButton.UseVisualStyleBackColor = true;
            // 
            // startTextBox
            // 
            startTextBox.Location = new Point(251, 2);
            startTextBox.Name = "startTextBox";
            startTextBox.Size = new Size(59, 23);
            startTextBox.TabIndex = 9;
            startTextBox.Text = "00:00:00";
            startTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // startLabel
            // 
            startLabel.AutoSize = true;
            startLabel.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
            startLabel.Location = new Point(215, 7);
            startLabel.Name = "startLabel";
            startLabel.Size = new Size(24, 12);
            startLabel.TabIndex = 4;
            startLabel.Text = "Start";
            startLabel.TextAlign = ContentAlignment.BottomLeft;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 410);
            Controls.Add(specificSplitParamsPanel);
            Controls.Add(splitTypePanel);
            Controls.Add(splitTypeLabel);
            Controls.Add(progressPanel);
            Controls.Add(actionTypePanel);
            Controls.Add(label2);
            Controls.Add(selectLabel);
            Controls.Add(fileNameLabel);
            Controls.Add(fileDialogPanel);
            Controls.Add(intervalSplitParamsPanel);
            Controls.Add(howLongLabel);
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
            intervalSplitParamsPanel.ResumeLayout(false);
            intervalSplitParamsPanel.PerformLayout();
            splitTypePanel.ResumeLayout(false);
            splitTypePanel.PerformLayout();
            specificSplitParamsPanel.ResumeLayout(false);
            specificSplitParamsPanel.PerformLayout();
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
        private Panel intervalSplitParamsPanel;
        private Label secondLabel;
        private Label minuteLabel;
        private TextBox secondTextBox;
        private TextBox minuteTextBox;
        private Button selectSingleFileButton;
        private Panel splitTypePanel;
        private RadioButton specificRadioButton;
        private RadioButton intervalRadioButton;
        private Label splitTypeLabel;
        private Panel specificSplitParamsPanel;
        private Label startLabel;
        private TextBox startTextBox;
        private TextBox endTextBox;
        private Label endLabel;
        private Button removeButton;
        private Button addButton;
    }
}