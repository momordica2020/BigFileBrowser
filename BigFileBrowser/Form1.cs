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
using Mozilla.NUniversalCharDet;
using System.Text.RegularExpressions;
using BigFileBrowser.Properties;
using System.Threading;

namespace BigFileBrowser
{


    public partial class Form1 : Form
    {
        class SearchItem
        {
            public string text;
            public int num;
            public long index;

            public SearchItem(string text, int num,long index)
            {
                this.text = text;
                this.num = num;
                this.index = index;
            }
        }

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private delegate void sendString(string str);
        private delegate void sendItemList(List<SearchItem> items);


        string filepath = "";
        long filelen = 1;
        int pagelen = 1000;
        int pagenum = 1;
        int encodingIndex = 0;
        Encoding encoding = Encoding.UTF8;

      
        /// <summary>
        /// 初始化各项信息
        /// 包括文件长度，编码格式等
        /// </summary>
        /// <param name="path"></param>
        private void InitFileInfo(string path)
        {
            filepath = path;
            FileInfo fi = new FileInfo(filepath);
            filelen = fi.Length;
            pagelen = int.Parse(numericUpDown1.Value.ToString());
            pagenum = (int)(Math.Ceiling((double)filelen /pagelen));

            // encoding
            var b = ReadBytes(0, long.Parse(numericUpDown1.Value.ToString()));
            List<string> encodings = new List<string>();
            foreach (var item in comboBox1.Items) encodings.Add(item.ToString());
            encodingIndex = EncodingHelper.getEncodingAuto(b, encodings.ToArray());
            encoding = Encoding.GetEncoding(encodings[encodingIndex]);
        }

        private void Open(string path)
        {
            InitFileInfo(path);

            label1.Text = "文件名：" + filepath;
            trackBar1.Maximum = pagenum-1;
            trackBar1.Value = 0;
            comboBox1.SelectedIndex = encodingIndex;
            listView1.Items.Clear();

            textBox1.ResetText();
            textBox1.AppendText(ReadText(trackBar1.Value));
        }


        private byte[] ReadBytes(long begin, long len)
        {
            byte[] buffer = new byte[len];
            using (FileStream fs = File.Open(filepath, FileMode.Open,FileAccess.Read))
            {
                fs.Seek(begin, SeekOrigin.Begin);
                using (MemoryStream ms = new MemoryStream())
                {
                    int read = fs.Read(buffer, 0, buffer.Length);
                }
            }
            return buffer;
        }

        private string ReadText(long beginnum)
        {
            try
            {
                long beginindex = beginnum * pagelen;
                //Encoding encoding = Encoding.GetEncoding(getEncoding(filepath).BodyName);

                int len = pagelen;
                len += 2;   // 后都多取 2 字符，检查是否有汉字截断
                beginindex = beginindex < 0 ? 0 : beginindex;

                byte[] buffer = ReadBytes(beginindex, len);
                string output = EncodingHelper.byteToString(buffer, pagelen, encoding).Replace("\0", " ").Replace("\r\n", "\n").Replace("\n", Environment.NewLine);

                return output;
                
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private void Print(string str)
        {
            if (this.InvokeRequired)
            {
                sendString te = new sendString(Print);
                Invoke(te, (object)str);
            }
            else
            {
                label4.Text = str.Trim();
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Open(openFileDialog1.FileName);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.ResetText();
            textBox1.AppendText(ReadText(trackBar1.Value));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    encoding = Encoding.UTF8;
                    break;
                case 1:
                    encoding = Encoding.GetEncoding("gb2312");
                    break;
                default:
                    break;
            }
            ReadText(trackBar1.Value);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            comboBox1.Select(0, 1);
        }

        //private long maxindex;
        //private long maxnum;
        //private void backwork()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < maxindex; i++)
        //    {
        //        //if (i > 100000) break;
        //        long len = maxnum + 2;
        //        long beginindex = i * maxnum;
        //        beginindex = beginindex < 0 ? 0 : beginindex;
        //        byte[] buffer = ReadBytes(beginindex, len);
        //        string output = EncodingHelper.byteToString(buffer, pagelen, encoding);

        //        for (int j = 0; j < output.Length; j++)
        //        {
        //            if ((output[j] >= 'A' && output[j] <= 'Z')
        //                || (output[j] >= '0' && output[j] <= '9')
        //                || output[j] == '-'
        //                || output[j] == ' '
        //                || EncodingHelper.GetHanNum(output[j].ToString()) > 0)
        //            {
        //                sb.Append(output[j]);
        //            }
        //            else
        //            {
        //                if (sb.Length > 20)
        //                {
        //                    using (FileStream fs = File.Open(Path.GetDirectoryName(filepath) + "/" + Path.GetFileName(filepath) + "_output.txt", FileMode.Append))
        //                    {
        //                        using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
        //                        {
        //                            sw.WriteLine(sb.ToString());
        //                        }
        //                    }
        //                }
        //                sb.Clear();
        //            }
        //        }
        //    }
        //}



        //private void backwork2()
        //{
        //    string[] lines = File.ReadAllLines(filepath, Encoding.Default);
        //    Dictionary<string, Item> items = new Dictionary<string, Item>();
        //    foreach (var line in lines)
        //    {
        //        int hanbegin = 0;
        //        int hanend = 0;
        //        for (int i = 0; i < line.Length; i++) if (EncodingHelper.GetHanNum(line[i].ToString()) > 0) { hanbegin = i; break; }
        //        for (int i = line.Length - 1; i >= 0; i--) if (EncodingHelper.GetHanNum(line[i].ToString()) > 0) { hanend = i; break; }
        //        if (hanbegin > 0)
        //        {
        //            string tmp = line.Substring(0, hanbegin);
        //            int cutbegin = tmp.IndexOf("000");
        //            string name = line.Substring(hanbegin, hanend - hanbegin + 1).Replace(" ", "");
        //            if (cutbegin > 0)
        //            {
        //                string id = tmp.Substring(0, cutbegin);
        //                string phone = tmp.Substring(cutbegin + 3);
        //                if (id.Length == 5
        //                  || (EncodingHelper.isEWord(id[0]) && id.Length == 9)
        //                  || (!EncodingHelper.isEWord(id[0]) && id.Length == 8)
        //                  || id.Length == 18) ;
        //                else
        //                {
        //                    id = id + '0';
        //                }

        //                if (!items.ContainsKey(id)) items[id] = new Item(id, name, phone);
        //            }
        //        }
        //    }
        //    using (FileStream fs = File.Open(Path.GetDirectoryName(filepath) + "/" + Path.GetFileName(filepath) + "_output.txt", FileMode.Create))
        //    {
        //        using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
        //        {
        //            foreach (var item in items.Values)
        //            {
        //                sw.WriteLine(string.Format("{0},{1},{2}", item.id, item.name, item.phone));
        //            }
        //        }
        //    }

        //}

        //private void backwork3()
        //{
        //    string[] lines = File.ReadAllLines(filepath, Encoding.Default);
        //    List<Item> res = new List<Item>();
        //    foreach (var line in lines)
        //    {
        //        string[] tmp = line.Split(',');
        //        res.Add(new Item(tmp[0], tmp[1], tmp[2]));
        //    }
        //    res.Sort();
            
            
        //    using (FileStream fs = File.Open(Path.GetDirectoryName(filepath) + "/" + Path.GetFileName(filepath) + "_output.txt", FileMode.Create))
        //    {
        //        using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
        //        {
        //            foreach (var item in res)
        //            {
        //                sw.WriteLine(string.Format("{0},{1},{2}", item.id, item.name, item.phone));
        //            }
        //        }
        //    }
        //}

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
            if(s.Length>0)
            {
                Open(s[0]);
            }
        }

        private void ShowSearchResult(List<SearchItem> items)
        {
            if (this.InvokeRequired)
            {
                sendItemList te = new sendItemList(ShowSearchResult);
                Invoke(te, (object)items);
            }
            else
            {
                int maxnum = 1000;
                listView1.Items.Clear();
                for(int i = 0; i < Math.Min(maxnum, items.Count); i++)
                {
                    listView1.Items.Add(new ListViewItem(new string[] { items[i].text, items[i].num.ToString() }));
                }
                listView1.Show();
            }
        }

        private void workSearch(object keyo)
        {
            try
            {
                string key = (string)keyo;
                byte[] keyb = encoding.GetBytes(key);
                List<SearchItem> res = new List<SearchItem>();

                using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    long nowp = 0;
                    int dl = 1024 * 1024 * 60;
                    byte[] blockb= new byte[dl];
                    do
                    {
                        fs.Seek(nowp,SeekOrigin.Begin);
                        fs.Read(blockb, 0, dl);
                        if (filelen - nowp < dl) for (long i = filelen - nowp; i < dl; i++) blockb[i] = 0;
                        bool find = false;
                        for (int i = 0; i < dl - keyb.Length; i++)
                        {
                            bool allsame = true;
                            for (int j = 0; j < keyb.Length; j++)
                            {
                                if (keyb[j] != blockb[i + j])
                                {
                                    allsame = false;
                                    break;
                                }
                            }
                            if (allsame)
                            {
                                int tbegin = Math.Max(0, i - 12);
                                int tcount = Math.Min(blockb.Length - tbegin, keyb.Length + 24);
                                string text = encoding.GetString(blockb, tbegin, tcount).Replace("\0"," ");
                                long index = nowp + i;
                                int num = (int)((double)index / pagelen);
                                if (res.Count <= 0 || res.Last().index != index) res.Add(new SearchItem(text, num, nowp + i));
                                //break;
                            }
                        }
                        Print(string.Format("{0}/{1},{2:N}%,{3}", nowp, filelen, (double)nowp * 100 / filelen, res.Count));
                        //ShowSearchResult(res);
                        nowp += (int)(0.95 * dl);
                    } while (nowp < filelen );
                }
                Print(string.Format("{0}个结果。{1}", res.Count, res.Count > 1000 ? "前1000个如下" : ""));
                ShowSearchResult(res);

            }
            catch(Exception e)
            {

            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Thread(workSearch).Start(textBox2.Text);
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count <= 0) return;

                int num = int.Parse(listView1.SelectedItems[0].SubItems[1].Text.ToString());
                trackBar1.Value = num;
                textBox1.ResetText();
                textBox1.AppendText(ReadText(num));
            }
            catch
            {

            }
           
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "不换行")
            {
                textBox1.WordWrap = false;
                button4.Text = "换行";
            }
            else
            {
                textBox1.WordWrap = true;
                button4.Text = "不换行";
            }
            
        }
    }

    public class Item : IComparable<Item>
    {
        public string id;
        public string name;
        public string phone;
        public Item(string _id, string _name, string _phone)
        {
            id = _id;
            name = _name;
            phone = _phone;
        }

        public int CompareTo(Item other)
        {
            return id.Length.CompareTo(other.id.Length);
        }
    }
}
