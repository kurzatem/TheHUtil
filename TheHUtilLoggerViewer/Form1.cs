namespace TheHUtilLoggerViewer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using TheHUtil.Extensions;

    using TheHUtilLoggerViewer.Interfaces;

    public partial class WinformViewer : Form, IView
    {
        private const string LABEL_PROGRAM_NAME_CAPTION = "Logs for the program:";

        private const string LABEL_PROGRAM_VERSION_CAPTION = "Version: ";

        private const string LABEL_CLASS_NAME_CAPTION = "Name of class calling for log: ";

        private const string LABEL_LOG_NUMBER_CAPTION = "Number: ";

        private const string LABEL_EXCEPTION_TYPE_CAPTION = "Type of exception logged: ";

        private const string LABEL_EXCEPTION_METHOD_CAPTION = "Method that threw the exception: ";

        private const string LABEL_EXCEPTION_MESSAGE_CAPTION = "Exception message: ";

        private const string LABEL_EXCEPTION_STACK_CAPTION = "Stack tracing of log: ";

        private const string LABEL_LOG_COMMENT_CAPTION = "Comment: ";

        private const string LABEL_EXCEPTION_OUTPUT_CAPTION = "File and line number: ";

        private ILogLoader logLoader;

        private int selectedLine;

        string IView.ClassFileView
        {
            set
            {
                this.rtbClassViewer.Text = value;
            }
        }

        int IView.LineToHighlight
        {
            set
            {
                this.selectedLine = value;
                this.SelectLineInrtbClassViewer();
            }
        }

        string IView.SelectedLogListing
        {
            get
            {
                return this.comboxLogSelector.SelectedItem.IsNull() ? null : this.comboxLogSelector.SelectedItem.ToString();
            }
        }

        string ILogData.LogForClassName
        {
            set
            {
                this.toolTip.SetToolTip(this.lblLogClassName, value);
                this.lblLogClassName.Text = LABEL_CLASS_NAME_CAPTION + value;
            }
        }

        string ILogData.ExceptionType
        {
            set
            {
                this.toolTip.SetToolTip(this.lblLogExceptionType, value);
                this.lblLogExceptionType.Text = LABEL_EXCEPTION_TYPE_CAPTION + value;
            }
        }

        int ILogData.LogNumber
        {
            set
            {
                this.toolTip.SetToolTip(this.lblLogNumber, value.ToString());
                this.lblLogNumber.Text = LABEL_LOG_NUMBER_CAPTION + value.ToString();
            }
        }

        string ILogData.ExceptionMethodName
        {
            set
            {
                this.toolTip.SetToolTip(this.lblExceptionMethod, value);
                this.lblExceptionMethod.Text = LABEL_EXCEPTION_METHOD_CAPTION + value;
            }
        }

        string ILogData.ExceptionMessage
        {
            set
            {
                this.toolTip.SetToolTip(this.lblExceptionMessage, value);
                this.lblExceptionMessage.Text = LABEL_EXCEPTION_MESSAGE_CAPTION + value;
            }
        }

        string ILogData.ExceptionStack
        {
            set
            {
                this.toolTip.SetToolTip(this.lblLogStack, value);
                this.lblLogStack.Text = LABEL_EXCEPTION_STACK_CAPTION + value;
            }
        }

        string ILogData.ExceptionOutput
        {
            set
            {
                this.toolTip.SetToolTip(this.lblExceptionOutput, value);
                this.lblExceptionOutput.Text = LABEL_EXCEPTION_OUTPUT_CAPTION + value;
            }
        }

        string ILogData.LogComment
        {
            set
            {
                this.toolTip.SetToolTip(this.lblLogComment, value);
                this.lblLogComment.Text = LABEL_LOG_COMMENT_CAPTION + value;
            }
        }

        public WinformViewer()
        {
            InitializeComponent();

            this.rtbClassViewer.SelectionBackColor = Color.Red;
            this.rtbClassViewer.SelectionColor = Color.White;

            this.logLoader = new LogLoader(this);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            this.workerThread.RunWorkerAsync();
        }

        private void comboxLogSelector_SelectedValueChanged(object sender, EventArgs e)
        {
            this.logLoader.ShowLog();
        }

        private void toolTip_Popup(object sender, PopupEventArgs e)
        {
        }

        private void SelectLineInrtbClassViewer()
        {
            var startIndex = this.rtbClassViewer.GetFirstCharIndexFromLine(this.selectedLine);
            var selectionLength = this.rtbClassViewer.Text.IndexOf('\n', startIndex) - startIndex;
            if (selectionLength > 0)
            {
                this.rtbClassViewer.Select(startIndex, selectionLength);
            }

            this.rtbClassViewer.ScrollToCaret();
        }

        private void rtbClassViewer_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.rtbClassViewer.SelectionLength > 0)
            {
                this.rtbClassViewer.DeselectAll();
            }

            this.SelectLineInrtbClassViewer();
        }

        private void workerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            this.logLoader.OpenFile(this.openFileDialog.FileName, this.workerThread);
        }

        private void workerThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.tStripPbarFileLoading.Value = e.ProgressPercentage;
        }

        private void workerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.comboxLogSelector.Items.Clear();
            this.comboxLogSelector.Items.AddRange(this.logLoader.LogCollectionListing);
            this.tStripLblFileStatus.Text = "Loaded";
            this.lblProgramLogged.Text = LABEL_PROGRAM_NAME_CAPTION + this.logLoader.ProgramName;
            this.lblProgramVersion.Text = LABEL_PROGRAM_VERSION_CAPTION + this.logLoader.ProgramVersion;
        }
    }
}
