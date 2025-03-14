using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net.NetworkInformation;
using BigFileBrowser.Providers;
using System.Runtime.InteropServices;
using MaterialSkin.Controls;
using MaterialSkin;
using System.Windows.Media.Animation;
using BigFileBrowser.Events;
using DryIoc.Messages;


namespace BigFileBrowser
{


    public partial class MainForm : MaterialForm
    {
        //// 定义 Windows API
        //[DllImport("dwmapi.dll", PreserveSig = false)]
        //private static extern void DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        //[DllImport("dwmapi.dll", PreserveSig = false)]
        //private static extern bool DwmIsCompositionEnabled();

        //private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        //FileSearcher fileSearcher;
        StreamFile file;

        const int TextMaxLength = 100000;
        //string filepath = "";
        //long filelen = 1;
        //int pagelen = 1000;
        //int pagenum = 1;
        //int encodingIndex = 0;
        //Encoding encoding = Encoding.UTF8;

        int searchMaxResult = 1000;

        private object FileShowMutex = new();
        Debouncer FilePageDebouncer = new Debouncer(100);

        //public ShowTextToMainFormEvent ShowTextToMainForm;



        /// <summary>
        /// 显示文件内容
        /// </summary>
        /// <param name="text"></param>
        /// <param name="append"></param>
        void ShowTextToMainTextbox(string text, bool append = false)
        {

            Invoke((Action)(() =>
            {
                if (append)
                {
                    if (FileContentTextBox.TextLength > TextMaxLength)
                    {
                        FileContentTextBox.Text = FileContentTextBox.Text.Substring(TextMaxLength / 2);
                    }
                    FileContentTextBox.AppendText(text);
                }
                else
                {
                    FileContentTextBox.Text = text;
                }

            }));
        }






        public MainForm()
        {
            //if (DwmIsCompositionEnabled())
            //{
            //    int useImmersiveDarkMode = 1; // 1 表示启用暗色模式
            //    DwmSetWindowAttribute(this.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useImmersiveDarkMode, sizeof(int));
            //}

            InitializeComponent();


            // dark mode
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.BlueGrey800, Primary.BlueGrey900,
                Primary.BlueGrey500, Accent.LightBlue200,
                TextShade.WHITE
            );


            // bind delegates
            //ShowTextToMainForm = _showTextToMainTextbox;


            // init contols
            ComboBoxPageSize.SelectedIndex = 2;
            ComboBoxEncodings.Items.Clear();
            foreach (var encoding in EncodingHelper.Encodings)
            {
                ComboBoxEncodings.Items.Add(encoding.WebName);
            }
            ComboBoxEncodings.SelectedIndex = 0;
            SliderPage.Enabled = false;
            NumericPage.Enabled = false;


            //fileSearcher = new FileSearcher(ShowSearchResult2);

            //textBox3.Text = Environment.CurrentDirectory;


            //SetDarkMode(this);
        }

        //private void SetDarkMode(System.Windows.Forms.Control control)
        //{
        //    // 设置控件的背景色和前景色
        //    control.BackColor = Color.FromArgb(64, 64, 64);
        //    control.ForeColor = Color.White;

        //    // 递归设置子控件的颜色
        //    foreach (System.Windows.Forms.Control childControl in control.Controls)
        //    {
        //        SetDarkMode(childControl);
        //    }
        //}







        /// <summary>
        /// 初始化读入新的文件信息
        /// </summary>
        /// <param name="path"></param>
        private void UpdateFileInfo(string path)
        {
            if (file != null) file.Dispose();
            file = new StreamFile(path);

            this.Text = $"大文件查看 - {file.Name} - {file.SizeString}";
            LabelFilePath.Text = $"路径：{file.Path}";


            foreach (var item in ComboBoxEncodings.Items)
            {
                if (item.ToString() == file.Encoding.EncodingName)
                {
                    ComboBoxEncodings.SelectedItem = item;
                    break;
                }
            }

            UpdatePageInfo();
            UpdateFileShown();
        }

        private void UpdatePageInfo()
        {
            if (file == null) return;
            int pageSize = int.Parse(ComboBoxPageSize.SelectedItem.ToString());
            int pageNum = (int)Math.Ceiling((double)file.Size / pageSize);
            int RangeMax = Math.Max(1, pageNum - 1);
            int val = Math.Min(SliderPage.Value, RangeMax);
            SliderPage.Enabled = false;
            NumericPage.Enabled = false;

            NumericPage.Maximum = RangeMax;
            NumericPage.Value = val;

            SliderPage.RangeMax = RangeMax;
            SliderPage.Value = val;

            if (RangeMax <= 1)
            {
                SliderPage.Enabled = false;
                NumericPage.Enabled = false;
            }
            else
            {
                SliderPage.Enabled = true;
                NumericPage.Enabled = true;
            }
        }

        /// <summary>
        /// 刷新所显示的页面
        /// </summary>
        private void UpdateFileShown()
        {
            if (file == null) return;
            FilePageDebouncer.Debounce(() =>
            {
                this.Invoke(() =>
                {
                    int pageSize = int.Parse(ComboBoxPageSize.SelectedItem.ToString());
                    int beginNum = SliderPage.Value;
                    long beginindex = beginNum * pageSize;
                    string pageContent = file.ReadTextAsync(beginindex, pageSize).Result;
                    ShowTextToMainTextbox(pageContent);
                });


            });

        }










        private void Print(string str)
        {
            try
            {
                Invoke((Action)(() =>
                {
                    label4.Text = str.Trim();
                }));
            }
            catch (Exception ex)
            {
                //Logger.Log(ex);
            }
        }

        private void PrintStatus(string str)
        {
            try
            {
                Invoke(() =>
                {
                    toolStripStatusLabel1.Text = $"[{DateTime.Now.ToString("HH:mm:ss")}]{str}";
                });
            }
            catch (Exception ex)
            {
                //Logger.Log(ex);
            }
        }


        private void AppendSearchResult(SearchResult result)
        {
            Invoke(() =>
            {
                if (listView1.Items.Count >= searchMaxResult) return;
                PrintStatus($"{result.MatchedPositionInFile}/{file.Size},{((double)result.MatchedPositionInFile / file.Size):N}%,{listView1.Items.Count}");
                var pagesize = int.Parse(ComboBoxPageSize.SelectedItem.ToString());
                var pagenum = result.MatchedPositionInFile / pagesize;
                listView1.Items.Add(new ListViewItem(new string[] { result.MatchedContext, pagenum.ToString() }));
                ////listView1.Items.Clear();
                //for (int i = 0; i < Math.Min(searchMaxResult, item.Count); i++)
                //{
                //    listView1.Items.Add(new ListViewItem(new string[] { items[i].text, items[i].num.ToString() }));
                //}
                //listView1.Show();
            });
        }

        private void AppendSearchResult2(SearchResult result)
        {
            Invoke(() =>
            {
                listView2.Items.Add(new ListViewItem(new string[] { result.MatchedContext, result.MatchedPositionInFile.ToString() }));
            });
        }


        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            UpdateFileInfo(openFileDialog1.FileName);
        }



        private void Form1_Shown(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //maxindex = trackBar1.Maximum;
            //maxnum = long.Parse(numericUpDown1.Value.ToString());
            //new Thread(backwork3).Start();

        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length > 0)
            {
                UpdateFileInfo(s[0]);
            }
        }




        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            listView1.Items.Clear();
            string key = textBox2.Text;
            int pageSize = int.Parse(ComboBoxPageSize.SelectedItem.ToString());
            await file.SearchAsync(key, pageSize, searchMaxResult, AppendSearchResult);
            button3.Enabled = true;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count <= 0) return;

                int pageNum = int.Parse(listView1.SelectedItems[0].SubItems[1].Text.ToString());
                SliderPage.Value = pageNum;
                UpdatePageInfo();
                UpdateFileShown();
                //FileContentTextBox.ResetText();
                //FileContentTextBox.Text = ReadText(pageNum);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }






        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            listView2.Items.Clear();

            string path = textBox4.Text;
            string key = textBox5.Text.Trim();
            _ = FileSearcher.SearchFilesForKeywordAsync(path, key, AppendSearchResult2).Result;
            PrintStatus($"检索完毕，共{listView2.Items.Count}个结果");
            button6.Enabled = true;
        }

        private void listView2_SizeChanged(object sender, EventArgs e)
        {
            if (listView2.Columns.Count > 0)
            {
                listView2.Columns[listView2.Columns.Count - 1].Width = -2;
            }
        }

        private void materialComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePageInfo();
            UpdateFileShown();
        }

        private void FIleSlider_onValueChanged(object sender, int newValue)
        {
            if (SliderPage.Enabled == false) return;
            UpdatePageInfo();
            UpdateFileShown();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void 测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //test?
        }

        private void SwitchAutoWordwarp_CheckedChanged(object sender, EventArgs e)
        {
            FileContentTextBox.WordWrap = SwitchAutoWordwarp.Checked;
            UpdateFileShown();
        }

        private void ComboBoxEncodings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (file == null) return;

            file.Encoding = Encoding.GetEncoding(ComboBoxEncodings.SelectedItem.ToString());
            UpdateFileShown();


        }

        private void NumericPage_ValueChanged(object sender, EventArgs e)
        {
            if (NumericPage.Enabled == false) return;
            SliderPage.Value = (int)NumericPage.Value;
            UpdatePageInfo();
            UpdateFileShown();
        }

        private void ButtonSearchInPage_Click(object sender, EventArgs e)
        {
            string key = TextBoxSearchInPage.Text;
        }
    }
}