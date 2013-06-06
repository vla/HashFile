using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HasFile
{
    public partial class FrmMain : Form
    {
        private MessageInfo messageInfo = new MessageInfo();

        private AsyncFunc<string[],MessageInfo> answer;

        public FrmMain() {
            InitializeComponent();

            answer = new AsyncFunc<string[], MessageInfo>(RunProgerss);
            answer.Completed += answer_Completed;
            answer.ProgressChanged += answer_ProgressChanged;
            BarTime.Step = 1;
            BarTime.Value = 0;
            BarCount.Value = 0;
            CmdStop.Enabled = false;
        }

        private void FrmMain_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void FrmMain_DragDrop(object sender, DragEventArgs e) {
            string[] array = e.Data.GetData(DataFormats.FileDrop) as string[];

            base.Activate();

            BarTime.Value = 0;
            BarCount.Value = 0;

            CmdStop.Enabled = true;

            answer.TryInvokeAsync(array);
        }

        private MessageInfo RunProgerss(string[] files, Func<bool> isCancelled, Action<int> reportProgress) {
            IList<string> list = new List<string>(3);

            if (ChkMD5.Checked) {
                list.Add("MD5");
            }
            if (ChkSHA1.Checked) {
                list.Add("SHA1");
            }
            if (ChkCRC32.Checked) {
                list.Add("CRC32");
            }

            string seed = TxtSeed.Text;
            bool clearLetter = ChkNumber.Checked;

            StringBuilder result = new StringBuilder();
            int countIdx = 1;
            if (files.Length > 0)
                SetProcessbarCount(files.Length);

            for (int i=0; i < files.Length; i++) {
                if (!File.Exists(files[i])) continue;
                if (isCancelled()) return null;

                if (i > 0)
                    result.AppendLine(Environment.NewLine);

                FileInfo fileInfo = new FileInfo(files[i]);

                result.AppendLine("文件:  " + fileInfo.FullName);
                result.AppendLine("大小:  " + fileInfo.Length + " 字节," + FormatBytesStr(fileInfo.Length));

                if (ChkTime.Checked)
                    result.AppendLine("修改时间: " + fileInfo.LastWriteTime.ToString("yyyy年MM月dd日 HH:mm:ss"));

                using (Stream stream = fileInfo.OpenRead()) {
                    int taskCount = list.Count;
                    int taskProgessCount = 100 / taskCount;
                    for (int t=0; t < list.Count; t++) {
                        if (isCancelled()) return null;

                        string hash = FileCheck.GetHash(list[t], stream, delegate(int num)
                        {
                            reportProgress(num / taskCount + (taskProgessCount * t));

                            return !isCancelled();
                        });

                        result.AppendLine(list[t] + ": " + FixValue(hash, clearLetter, seed));
                    }
                }

                if (isCancelled()) return null;
                SetProcessbar(countIdx++);
            }

            if (result.Length > 0) {
                return messageInfo.Append(result);
            } else
                return messageInfo;
        }

        private static string FixValue(string value, bool clearLetter, string seed) {
            byte[] bytes = Encoding.UTF8.GetBytes(value + seed);
            string result = string.Empty;
            if (string.IsNullOrEmpty(seed)) {
                result = value;
            } else {
                result = BitConverter.ToString(SHA1.Create().ComputeHash(bytes)).Replace("-", string.Empty);
            }
            return clearLetter ? Regex.Replace(result, @"[^\d]", string.Empty) : result;
        }

        private static string FormatBytesStr(long bytes) {
            if (bytes > 1073741824)
                return string.Format("{0}G", (((double)bytes / 1073741824d)).ToString("F2"));
            if (bytes > 1048576)
                return string.Format("{0}MB", (((double)bytes / 1048576d)).ToString("F2"));
            if (bytes > 1024)
                return string.Format("{0}KB", (((double)bytes / 1024d)).ToString("F2"));
            return string.Empty;
        }

        private void answer_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            if (e.ProgressPercentage <= BarTime.Maximum) {
                BarTime.Value = e.ProgressPercentage;
            }
        }

        private void answer_Completed(object sender, AsyncFuncCompletedEventArgs<MessageInfo> e) {
            if (!e.Cancelled) {
                if (e.Error == null) {
                    BarTime.Value = 100;
                    TxtView.Text = e.Result.ToString();
                    TxtView.SelectionStart = TxtView.TextLength;
                    TxtView.ScrollToCaret();
                    e.Result.AppendLine(Environment.NewLine);
                } else {
                    MessageBox.Show(e.Error.Message, "Info");
                    BarTime.Value = 0;
                    BarCount.Value = 0;
                }
            } else {
                BarCount.Value = 0;
                BarTime.Value = 0;
            }
            CmdStop.Enabled = false;
        }

        private void SetProcessbar(int value) {
            Action<int> act = null;
            try {
                if (act == null) {
                    act = delegate(int s)
                    {
                        if (value <= BarCount.Maximum) {
                            this.BarCount.Value = value;
                            Application.DoEvents();
                        }
                    };
                }
                Action<int> method = act;
                base.Invoke(method, new object[] { value });
            } catch (Exception exception) {
                throw exception;
            }
        }

        private void SetProcessbarCount(int value) {
            Action<int> act = null;
            try {
                if (act == null) {
                    act = delegate(int s)
                    {
                        if (value <= BarCount.Maximum) {
                            this.BarCount.Maximum = value;
                            Application.DoEvents();
                        }
                    };
                }
                Action<int> method = act;
                base.Invoke(method, new object[] { value });
            } catch (Exception exception) {
                throw exception;
            }
        }

        private void CmdStop_Click(object sender, EventArgs e) {
            answer.Cancel();
        }

        private void CmdClear_Click(object sender, EventArgs e) {
            messageInfo.Clear();
            TxtView.Text = string.Empty;
        }

        private void CmdClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) {
            answer.Cancel();
        }

        private void CmdCopy_Click(object sender, EventArgs e) {
            if (TxtView.TextLength > 0) {
                Clipboard.SetDataObject(TxtView.Text);
            }
        }

        private void CmdSave_Click(object sender, EventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            string filename = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            dialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            dialog.FileName = filename + ".txt";
            if (dialog.ShowDialog() == DialogResult.OK) {
                string fileName = dialog.FileName;
                string text = this.TxtView.Text;
                FileStream stream = new FileStream(fileName, FileMode.Append);
                StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("gb2312"));
                writer.WriteLine(text);
                writer.Close();
                stream.Close();
            }
        }

        private void CmdBrowse_Click(object sender, EventArgs e) {
            OpenFileDialog fileDiaLog = new OpenFileDialog();
            fileDiaLog.Filter = "Hash文件(*.*)|*.*";
            fileDiaLog.CheckFileExists = true;  //验证路径有效性
            fileDiaLog.CheckPathExists = true; //验证文件有效性
            fileDiaLog.Multiselect = true;
            if (fileDiaLog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            answer.TryInvokeAsync(fileDiaLog.FileNames);
        }
    }
}