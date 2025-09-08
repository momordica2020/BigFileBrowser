using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BigFileBrowser.Providers;
using MaterialSkin.Controls;
using MaterialSkin;
using BigFileBrowser.Events;


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
        int PageNum = 0;
        int PageSize = 0;
        int PageIndex = 0;

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
                    if (TextBoxPageContent.TextLength > TextMaxLength)
                    {
                        TextBoxPageContent.Text = TextBoxPageContent.Text.Substring(TextMaxLength / 2);
                    }
                    TextBoxPageContent.AppendText(text);
                }
                else
                {
                    TextBoxPageContent.Text = text;
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
            PageSize = int.Parse(ComboBoxPageSize.SelectedItem.ToString());
            ComboBoxEncodings.Items.Clear();
            foreach (var encoding in EncodingHelper.Encodings)
            {
                ComboBoxEncodings.Items.Add(encoding.WebName);
            }
            ComboBoxEncodings.SelectedIndex = 0;
            SliderPage.Enabled = false;
            NumericPage.Enabled = false;

            listView2.DrawItem += ListView2_DrawItem;
            listView2.DrawSubItem += ListView2_DrawSubItem;
            TextBoxPageContent.HideSelection = false;

            //fileSearcher = new FileSearcher(ShowSearchResult2);

            //textBox3.Text = Environment.CurrentDirectory;


            //SetDarkMode(this);
        }

        private void ListView2_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void ListView2_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

            //e.DrawBackground(); // 绘制背景
            //if (e.ColumnIndex == 1) // 只处理第二列（Column2）
            //{
            //    // 获取当前行的数据
            //    ListViewItem item = e.Item;
            //    string text = e.SubItem.Text;
            //    SearchResult tag = ((SearchResult)item.Tag);
            //    int offset = tag.MatchedPositionInContext;
            //    int length = tag.Key.Length;

            //    // 确保偏移量在文本范围内
            //    if (offset >= 0 && offset < text.Length)
            //    {
            //        // 绘制高亮部分
            //        string beforeHighlight = text.Substring(0, offset);
            //        string highlight = text.Substring(offset, length);
            //        string afterHighlight = text.Substring(offset + length);

            //        e.Graphics.DrawString(beforeHighlight, e.SubItem.Font, Brushes.White, e.Bounds.Left, e.Bounds.Top);

            //        // 计算高亮部分的区域
            //        SizeF beforeSize = e.Graphics.MeasureString(beforeHighlight, e.SubItem.Font);
            //        Rectangle highlightRect = new Rectangle(
            //            e.Bounds.Left + (int)beforeSize.Width,
            //            e.Bounds.Top,
            //            (int)e.Graphics.MeasureString(highlight, e.SubItem.Font).Width,
            //            e.Bounds.Height);

            //        // 绘制高亮
            //        e.Graphics.FillRectangle(Brushes.DarkBlue, highlightRect);
            //        e.Graphics.DrawString(highlight, e.SubItem.Font, Brushes.White, highlightRect.Left, highlightRect.Top);

            //        e.Graphics.DrawString(afterHighlight, e.SubItem.Font, Brushes.White, highlightRect.Left + highlightRect.Width, e.Bounds.Top);
            //    }
            //    else
            //    {
            //        // 如果偏移量无效，直接绘制文本
            //        e.DrawText();
            //    }
            //}
            //else
            //{
            //    // 其他列默认绘制
            //    e.DrawText();
            //}
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
        private void LoadFile(string path)
        {
            if (file != null) file.Dispose();
            file = new StreamFile(path);

            this.Text = $"大文件查看 - {file.Name} - {file.SizeString}";
            LabelFilePath.Text = $"路径：{file.Path}";


            foreach (var item in ComboBoxEncodings.Items)
            {
                if (item.ToString() == file.TextEncoding.WebName)
                {
                    ComboBoxEncodings.SelectedItem = item;
                    break;
                }
            }

            UpdatePage(0);

        }


        /// <summary>
        /// 刷新页码相关的界面显示
        /// </summary>
        /// <param name="pageIndex"></param>
        private void UpdatePage(int pageIndex)
        {
            try
            {
                if (file == null) return;
                PageSize = int.Parse(ComboBoxPageSize.SelectedItem.ToString());
                PageNum = (int)Math.Ceiling((double)file.Size / PageSize);
                int RangeMax = Math.Max(1, PageNum - 1);
                PageIndex = Math.Min(pageIndex, RangeMax);
                //SliderPage.Enabled = false;
                NumericPage.Enabled = false;

                NumericPage.Maximum = RangeMax;
                NumericPage.Value = PageIndex;

                SliderPage.Maximum = RangeMax;
                SliderPage.Value = PageIndex;

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

                UpdatePageContent();
            }
            catch (Exception ex)
            {
                PrintStatus($"UpdatePageIndex ERROR: {ex.Message}");
                Console.WriteLine(ex);
            }

        }


        /// <summary>
        /// 刷新所显示的页面
        /// </summary>
        private void UpdatePageContent()
        {
            if (file == null) return;
            Invoke(() =>
            {

                int pageSize = int.Parse(ComboBoxPageSize.SelectedItem.ToString());
                int beginNum = SliderPage.Value;
                long beginindex = beginNum * pageSize;
                string pageContent = file.ReadTextAsync(beginindex, pageSize).Result;
                ShowTextToMainTextbox(pageContent);
            });

        }

        /// <summary>
        /// 根据搜索结果，打开文件，定位页码，高亮特定位置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        private void ChooseAndSelectPage(SearchResult result)
        {
            if (result == null) return;
            if (file == null || result.FileInfo.FullName != file.Path)
            {
                // new file
                LoadFile(result.FileInfo.FullName);
            }
            var pageIndex = (int)(result.MatchedPositionInFile / PageSize);
            UpdatePage(pageIndex);
            string nowPageContent = TextBoxPageContent.Text;
            (int matchIndex, int matchLength, int matchCount, int matchIndexNum) = FileSearcher.Search(result.Key, nowPageContent, 0);
            if (matchIndex >= 0)
            {
                tabControl1.SelectedIndex = 0;
                TextBoxSearchInPage.Text = result.Key;
                PrintStatus($"在当前页面第{matchIndexNum}/{matchCount}个匹配结果： {result.Key}");
                TextBoxPageContent.ReadOnly = false;
                TextBoxPageContent.Select(matchIndex, matchLength);
                TextBoxPageContent.ReadOnly = true;


            }
            else
            {
                PrintStatus($"在当前页面没找到什么东西能匹配： {result.Key}");
            }
            //int position = (int)(result.MatchedPositionInFile % PageSize)
            //this.Invoke(() =>
            //{


            //    TextBoxPageContent.ReadOnly = false;
            //    TextBoxPageContent.Select(matchIndex, matchLength);
            //    TextBoxPageContent.ReadOnly = true;
            //};
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
                    toolStripStatusLabel1.Text = $"[{DateTime.Now.ToString("HH:mm:ss")}] {str}";
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 单文件搜索结果
        /// </summary>
        /// <param name="result"></param>
        private void AppendSearchResult(SearchResult result)
        {
            Invoke(() =>
            {
                if (listView1.Items.Count >= searchMaxResult) return;
                PrintStatus($"{result.MatchedPositionInFile}/{file.Size},{((double)result.MatchedPositionInFile / file.Size):N}%,{listView1.Items.Count}");
                var pageIndex = result.MatchedPositionInFile / PageSize;
                var item = new ListViewItem(new string[] { pageIndex.ToString(), result.MatchedContext, });
                item.Tag = result;
                listView1.Items.Add(item);
                listView1.Show();
                ////listView1.Items.Clear();
                //for (int i = 0; i < Math.Min(searchMaxResult, item.Count); i++)
                //{
                //    listView1.Items.Add(new ListViewItem(new string[] { items[i].text, items[i].num.ToString() }));
                //}
                //listView1.Show();
            });
        }
        public string TruncateString(string input, int headLength = 15, int tailLength = 20)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= headLength + tailLength)
                return input;

            return input.Substring(0, headLength) + "..." + input.Substring(input.Length - tailLength);
        }

        /// <summary>
        /// 文件夹搜索结果
        /// </summary>
        /// <param name="result"></param>
        private void AppendSearchResult2(SearchResult result)
        {
            Invoke(() =>
            {
                var item = new ListViewItem(new[] {
                    TruncateString(result.FileInfo.FullName),
                    result.MatchedPositionInFile.ToString(),
                    result.MatchedContext,
                });
                item.Tag = result;
                listView2.Items.Add(item);
                listView2.Show();
            });
        }


        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            LoadFile(openFileDialog1.FileName);
        }



        private void Form1_Shown(object sender, EventArgs e)
        {
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
                LoadFile(s[0]);
            }
        }




        private async void button3_Click(object sender, EventArgs e)
        {
            if (file == null) return;
            button3.Enabled = false;
            listView1.Items.Clear();
            string key = textBox2.Text;
            if (string.IsNullOrWhiteSpace(key)) return;
            key = key.Trim();
            int pageSize = int.Parse(ComboBoxPageSize.SelectedItem.ToString());

            Task.Run(async () =>
            {
                await file.Search(key, AppendSearchResult);
                Invoke(() =>
                {
                    PrintStatus($"检索完毕，共{listView1.Items.Count}个结果");
                    button3.Enabled = true;

                });

            });


        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Environment.Exit(0);
        }




        private void SearchInDictionary()
        {
            button6.Enabled = false;
            listView2.Items.Clear();
            string path = textBox4.Text;
            string key = textBox5.Text.Trim();
            PrintStatus($"开始从{path}检索{key}");
            Task.Run(() =>
            {
                _ = FileSearcher.SearchFilesForKeywordAsync(path, key, AppendSearchResult2).Result;
                Invoke(() =>
                {
                    PrintStatus($"检索完毕，共{listView2.Items.Count}个结果");
                    button6.Enabled = true;

                });

            });
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SearchInDictionary();

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
            UpdatePage(SliderPage.Value);
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
            TextBoxPageContent.WordWrap = SwitchAutoWordwarp.Checked;
            UpdatePageContent();
        }

        private void ComboBoxEncodings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (file == null) return;

            file.TextEncoding = Encoding.GetEncoding(ComboBoxEncodings.SelectedItem.ToString());
            UpdatePageContent();


        }

        private void NumericPage_ValueChanged(object sender, EventArgs e)
        {
            if (NumericPage.Enabled == false) return;
            UpdatePage((int)NumericPage.Value);
        }

        private void SearchInPage()
        {
            string key = TextBoxSearchInPage.Text;
            if (string.IsNullOrWhiteSpace(key)) return;
            string content = TextBoxPageContent.Text;
            if (string.IsNullOrWhiteSpace(content)) return;

            int beginIndex = TextBoxPageContent.SelectionStart + TextBoxPageContent.SelectionLength;
            (int matchIndex, int matchLength, int matchCount, int matchIndexNum) = FileSearcher.Search(key, content, beginIndex);
            if (matchIndex >= 0)
            {
                TextBoxPageContent.ReadOnly = false;
                TextBoxPageContent.Select(matchIndex, matchLength);
                TextBoxPageContent.ReadOnly = true;
                PrintStatus($"在当前页面第{matchIndexNum}/{matchCount}个匹配结果： {key}");
            }
            else
            {
                PrintStatus($"在当前页面没找到什么东西能匹配： {key}");
            }
        }

        private void ButtonSearchInPage_Click(object sender, EventArgs e)
        {
            SearchInPage();
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count <= 0) return;
                var searchResult = (SearchResult)(listView1.SelectedItems[0].Tag);
                //var pageIndex = (int)(searchResult.MatchedPositionInFile / PageSize);
                //UpdatePage(pageIndex);
                ChooseAndSelectPage(searchResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }



        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count <= 0) return;
            //string path = listView2.SelectedItems[0].Text;


            var searchResult = (SearchResult)(listView2.SelectedItems[0].Tag);
            ChooseAndSelectPage(searchResult);
            //var pageIndex = (int)(searchResult.MatchedPositionInFile / PageSize);


            //LoadFile(searchResult.FileInfo.FullName);
            //UpdatePage(pageIndex);
        }



        private void TextBoxSearchInPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchInPage();

            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchInDictionary();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (SliderPage.Enabled == false) return;
            UpdatePage(SliderPage.Value);
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            if (listView1.Columns.Count > 0)
            {
                listView1.Columns[listView1.Columns.Count - 1].Width = -2;
            }
        }
    }
}