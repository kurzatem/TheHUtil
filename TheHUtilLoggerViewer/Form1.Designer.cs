namespace TheHUtilLoggerViewer
{
    partial class WinformViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param nameAndPath="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblLogNumber = new System.Windows.Forms.Label();
            this.gboxLogData = new System.Windows.Forms.GroupBox();
            this.lblExceptionOutput = new System.Windows.Forms.Label();
            this.lblExceptionMethod = new System.Windows.Forms.Label();
            this.lblLogComment = new System.Windows.Forms.Label();
            this.lblExceptionMessage = new System.Windows.Forms.Label();
            this.lblLogStack = new System.Windows.Forms.Label();
            this.lblLogExceptionType = new System.Windows.Forms.Label();
            this.lblLogClassName = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.comboxLogSelector = new System.Windows.Forms.ComboBox();
            this.gboxLogManager = new System.Windows.Forms.GroupBox();
            this.lblFileSelection = new System.Windows.Forms.Label();
            this.lblLogSelection = new System.Windows.Forms.Label();
            this.lblProgramVersion = new System.Windows.Forms.Label();
            this.lblProgramLogged = new System.Windows.Forms.Label();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.gboxClassFileViewer = new System.Windows.Forms.GroupBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.rtbClassViewer = new System.Windows.Forms.RichTextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tStripLblFileProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.tStripPbarFileLoading = new System.Windows.Forms.ToolStripProgressBar();
            this.tStripLblFileStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.workerThread = new System.ComponentModel.BackgroundWorker();
            this.gboxLogData.SuspendLayout();
            this.gboxLogManager.SuspendLayout();
            this.gboxClassFileViewer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblLogNumber
            // 
            this.lblLogNumber.BackColor = System.Drawing.Color.White;
            this.lblLogNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLogNumber.Location = new System.Drawing.Point(6, 46);
            this.lblLogNumber.Margin = new System.Windows.Forms.Padding(3);
            this.lblLogNumber.Name = "lblLogNumber";
            this.lblLogNumber.Size = new System.Drawing.Size(966, 19);
            this.lblLogNumber.TabIndex = 0;
            this.lblLogNumber.Text = "Number:";
            // 
            // gboxLogData
            // 
            this.gboxLogData.Controls.Add(this.lblExceptionOutput);
            this.gboxLogData.Controls.Add(this.lblExceptionMethod);
            this.gboxLogData.Controls.Add(this.lblLogComment);
            this.gboxLogData.Controls.Add(this.lblExceptionMessage);
            this.gboxLogData.Controls.Add(this.lblLogStack);
            this.gboxLogData.Controls.Add(this.lblLogExceptionType);
            this.gboxLogData.Controls.Add(this.lblLogClassName);
            this.gboxLogData.Controls.Add(this.lblLogNumber);
            this.gboxLogData.Location = new System.Drawing.Point(12, 95);
            this.gboxLogData.Name = "gboxLogData";
            this.gboxLogData.Size = new System.Drawing.Size(978, 271);
            this.gboxLogData.TabIndex = 1;
            this.gboxLogData.TabStop = false;
            this.gboxLogData.Text = "Log data";
            // 
            // lblExceptionOutput
            // 
            this.lblExceptionOutput.BackColor = System.Drawing.Color.White;
            this.lblExceptionOutput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblExceptionOutput.Location = new System.Drawing.Point(6, 246);
            this.lblExceptionOutput.Margin = new System.Windows.Forms.Padding(3);
            this.lblExceptionOutput.Name = "lblExceptionOutput";
            this.lblExceptionOutput.Size = new System.Drawing.Size(966, 19);
            this.lblExceptionOutput.TabIndex = 8;
            this.lblExceptionOutput.Text = "File and line number:";
            // 
            // lblExceptionMethod
            // 
            this.lblExceptionMethod.BackColor = System.Drawing.Color.White;
            this.lblExceptionMethod.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblExceptionMethod.Location = new System.Drawing.Point(6, 96);
            this.lblExceptionMethod.Margin = new System.Windows.Forms.Padding(3);
            this.lblExceptionMethod.Name = "lblExceptionMethod";
            this.lblExceptionMethod.Size = new System.Drawing.Size(966, 19);
            this.lblExceptionMethod.TabIndex = 7;
            this.lblExceptionMethod.Text = "Method that threw the exception:";
            // 
            // lblLogComment
            // 
            this.lblLogComment.BackColor = System.Drawing.Color.White;
            this.lblLogComment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLogComment.Location = new System.Drawing.Point(6, 196);
            this.lblLogComment.Margin = new System.Windows.Forms.Padding(3);
            this.lblLogComment.Name = "lblLogComment";
            this.lblLogComment.Size = new System.Drawing.Size(966, 44);
            this.lblLogComment.TabIndex = 5;
            this.lblLogComment.Text = "Comment:";
            // 
            // lblExceptionMessage
            // 
            this.lblExceptionMessage.BackColor = System.Drawing.Color.White;
            this.lblExceptionMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblExceptionMessage.Location = new System.Drawing.Point(6, 121);
            this.lblExceptionMessage.Margin = new System.Windows.Forms.Padding(3);
            this.lblExceptionMessage.Name = "lblExceptionMessage";
            this.lblExceptionMessage.Size = new System.Drawing.Size(966, 19);
            this.lblExceptionMessage.TabIndex = 4;
            this.lblExceptionMessage.Text = "Exception message:";
            // 
            // lblLogStack
            // 
            this.lblLogStack.BackColor = System.Drawing.Color.White;
            this.lblLogStack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLogStack.Location = new System.Drawing.Point(6, 146);
            this.lblLogStack.Margin = new System.Windows.Forms.Padding(3);
            this.lblLogStack.Name = "lblLogStack";
            this.lblLogStack.Size = new System.Drawing.Size(966, 44);
            this.lblLogStack.TabIndex = 3;
            this.lblLogStack.Text = "Stack tracing of log:";
            // 
            // lblLogExceptionType
            // 
            this.lblLogExceptionType.BackColor = System.Drawing.Color.White;
            this.lblLogExceptionType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLogExceptionType.Location = new System.Drawing.Point(6, 71);
            this.lblLogExceptionType.Margin = new System.Windows.Forms.Padding(3);
            this.lblLogExceptionType.Name = "lblLogExceptionType";
            this.lblLogExceptionType.Size = new System.Drawing.Size(966, 19);
            this.lblLogExceptionType.TabIndex = 2;
            this.lblLogExceptionType.Text = "Type of exception logged:";
            // 
            // lblLogClassName
            // 
            this.lblLogClassName.BackColor = System.Drawing.Color.White;
            this.lblLogClassName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLogClassName.Location = new System.Drawing.Point(6, 21);
            this.lblLogClassName.Margin = new System.Windows.Forms.Padding(3);
            this.lblLogClassName.Name = "lblLogClassName";
            this.lblLogClassName.Size = new System.Drawing.Size(966, 19);
            this.lblLogClassName.TabIndex = 1;
            this.lblLogClassName.Text = "Name of class calling for log:";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "xml";
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Title = "Open log file";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // comboxLogSelector
            // 
            this.comboxLogSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxLogSelector.FormattingEnabled = true;
            this.comboxLogSelector.Location = new System.Drawing.Point(191, 47);
            this.comboxLogSelector.Name = "comboxLogSelector";
            this.comboxLogSelector.Size = new System.Drawing.Size(179, 24);
            this.comboxLogSelector.TabIndex = 8;
            this.comboxLogSelector.SelectedValueChanged += new System.EventHandler(this.comboxLogSelector_SelectedValueChanged);
            // 
            // gboxLogManager
            // 
            this.gboxLogManager.Controls.Add(this.lblFileSelection);
            this.gboxLogManager.Controls.Add(this.lblLogSelection);
            this.gboxLogManager.Controls.Add(this.lblProgramVersion);
            this.gboxLogManager.Controls.Add(this.lblProgramLogged);
            this.gboxLogManager.Controls.Add(this.btnOpenFile);
            this.gboxLogManager.Controls.Add(this.comboxLogSelector);
            this.gboxLogManager.Location = new System.Drawing.Point(12, 12);
            this.gboxLogManager.Name = "gboxLogManager";
            this.gboxLogManager.Size = new System.Drawing.Size(978, 77);
            this.gboxLogManager.TabIndex = 2;
            this.gboxLogManager.TabStop = false;
            this.gboxLogManager.Text = "Log and file selection";
            // 
            // lblFileSelection
            // 
            this.lblFileSelection.AutoSize = true;
            this.lblFileSelection.BackColor = System.Drawing.Color.White;
            this.lblFileSelection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFileSelection.Location = new System.Drawing.Point(7, 22);
            this.lblFileSelection.Margin = new System.Windows.Forms.Padding(3);
            this.lblFileSelection.Name = "lblFileSelection";
            this.lblFileSelection.Size = new System.Drawing.Size(222, 19);
            this.lblFileSelection.TabIndex = 13;
            this.lblFileSelection.Text = "Log file selection and information:";
            // 
            // lblLogSelection
            // 
            this.lblLogSelection.AutoSize = true;
            this.lblLogSelection.BackColor = System.Drawing.Color.White;
            this.lblLogSelection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLogSelection.Location = new System.Drawing.Point(87, 50);
            this.lblLogSelection.Margin = new System.Windows.Forms.Padding(3);
            this.lblLogSelection.Name = "lblLogSelection";
            this.lblLogSelection.Size = new System.Drawing.Size(98, 19);
            this.lblLogSelection.TabIndex = 12;
            this.lblLogSelection.Text = "Log selection:";
            // 
            // lblProgramVersion
            // 
            this.lblProgramVersion.AutoSize = true;
            this.lblProgramVersion.BackColor = System.Drawing.Color.White;
            this.lblProgramVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProgramVersion.Location = new System.Drawing.Point(376, 46);
            this.lblProgramVersion.Margin = new System.Windows.Forms.Padding(3);
            this.lblProgramVersion.Name = "lblProgramVersion";
            this.lblProgramVersion.Size = new System.Drawing.Size(62, 19);
            this.lblProgramVersion.TabIndex = 11;
            this.lblProgramVersion.Text = "Version:";
            // 
            // lblProgramLogged
            // 
            this.lblProgramLogged.AutoSize = true;
            this.lblProgramLogged.BackColor = System.Drawing.Color.White;
            this.lblProgramLogged.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProgramLogged.Location = new System.Drawing.Point(376, 21);
            this.lblProgramLogged.Margin = new System.Windows.Forms.Padding(3);
            this.lblProgramLogged.Name = "lblProgramLogged";
            this.lblProgramLogged.Size = new System.Drawing.Size(147, 19);
            this.lblProgramLogged.TabIndex = 10;
            this.lblProgramLogged.Text = "Logs for the program:";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(6, 47);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 24);
            this.btnOpenFile.TabIndex = 9;
            this.btnOpenFile.Text = "Open";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // gboxClassFileViewer
            // 
            this.gboxClassFileViewer.Controls.Add(this.rtbClassViewer);
            this.gboxClassFileViewer.Location = new System.Drawing.Point(12, 372);
            this.gboxClassFileViewer.Name = "gboxClassFileViewer";
            this.gboxClassFileViewer.Size = new System.Drawing.Size(978, 322);
            this.gboxClassFileViewer.TabIndex = 3;
            this.gboxClassFileViewer.TabStop = false;
            this.gboxClassFileViewer.Text = "Class file viewer";
            // 
            // toolTip
            // 
            this.toolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip_Popup);
            // 
            // rtbClassViewer
            // 
            this.rtbClassViewer.BackColor = System.Drawing.Color.White;
            this.rtbClassViewer.HideSelection = false;
            this.rtbClassViewer.Location = new System.Drawing.Point(6, 21);
            this.rtbClassViewer.Name = "rtbClassViewer";
            this.rtbClassViewer.ReadOnly = true;
            this.rtbClassViewer.Size = new System.Drawing.Size(966, 295);
            this.rtbClassViewer.TabIndex = 0;
            this.rtbClassViewer.Text = "";
            this.rtbClassViewer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rtbClassViewer_MouseClick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tStripLblFileProgress,
            this.tStripPbarFileLoading,
            this.tStripLblFileStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 694);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1002, 25);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip";
            // 
            // tStripLblFileProgress
            // 
            this.tStripLblFileProgress.Name = "tStripLblFileProgress";
            this.tStripLblFileProgress.Size = new System.Drawing.Size(95, 20);
            this.tStripLblFileProgress.Text = "File Progress:";
            // 
            // tStripPbarFileLoading
            // 
            this.tStripPbarFileLoading.Name = "tStripPbarFileLoading";
            this.tStripPbarFileLoading.Size = new System.Drawing.Size(100, 19);
            // 
            // tStripLblFileStatus
            // 
            this.tStripLblFileStatus.Name = "tStripLblFileStatus";
            this.tStripLblFileStatus.Size = new System.Drawing.Size(115, 20);
            this.tStripLblFileStatus.Text = "Select a log file.";
            // 
            // workerThread
            // 
            this.workerThread.WorkerReportsProgress = true;
            this.workerThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerThread_DoWork);
            this.workerThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.workerThread_ProgressChanged);
            this.workerThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerThread_RunWorkerCompleted);
            // 
            // WinformViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 719);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.gboxClassFileViewer);
            this.Controls.Add(this.gboxLogManager);
            this.Controls.Add(this.gboxLogData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "WinformViewer";
            this.Text = "The-H Log Viewer";
            this.gboxLogData.ResumeLayout(false);
            this.gboxLogManager.ResumeLayout(false);
            this.gboxLogManager.PerformLayout();
            this.gboxClassFileViewer.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLogNumber;
        private System.Windows.Forms.GroupBox gboxLogData;
        private System.Windows.Forms.Label lblExceptionMethod;
        private System.Windows.Forms.Label lblLogComment;
        private System.Windows.Forms.Label lblExceptionMessage;
        private System.Windows.Forms.Label lblLogStack;
        private System.Windows.Forms.Label lblLogExceptionType;
        private System.Windows.Forms.Label lblLogClassName;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ComboBox comboxLogSelector;
        private System.Windows.Forms.GroupBox gboxLogManager;
        private System.Windows.Forms.Label lblFileSelection;
        private System.Windows.Forms.Label lblLogSelection;
        private System.Windows.Forms.Label lblProgramVersion;
        private System.Windows.Forms.Label lblProgramLogged;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.GroupBox gboxClassFileViewer;
        private System.Windows.Forms.Label lblExceptionOutput;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.RichTextBox rtbClassViewer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tStripLblFileProgress;
        private System.Windows.Forms.ToolStripProgressBar tStripPbarFileLoading;
        private System.Windows.Forms.ToolStripStatusLabel tStripLblFileStatus;
        private System.ComponentModel.BackgroundWorker workerThread;

    }
}

