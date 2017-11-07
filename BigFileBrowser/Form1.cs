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
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }



        string filepath = "";
        long filelen = 1;
        Encoding encoding = Encoding.UTF8;

        ///// <summary>
        ///// 返回流的编码格式
        ///// </summary>
        ///// <param name="stream"></param>
        ///// <returns></returns>
        //private static Encoding getEncoding(string streamName)
        //{
        //    Encoding encoding = Encoding.Default;
        //    using (Stream stream = new FileStream(streamName, FileMode.Open))
        //    {
        //        MemoryStream msTemp = new MemoryStream();
        //        int len = 0;
        //        byte[] buff = new byte[512];
        //        while ((len = stream.Read(buff, 0, 512)) > 0)
        //        {
        //            msTemp.Write(buff, 0, len);
        //        }
        //        if (msTemp.Length > 0)
        //        {
        //            msTemp.Seek(0, SeekOrigin.Begin);
        //            byte[] PageBytes = new byte[msTemp.Length];
        //            msTemp.Read(PageBytes, 0, PageBytes.Length);
        //            msTemp.Seek(0, SeekOrigin.Begin);
        //            int DetLen = 0;
        //            UniversalDetector Det = new UniversalDetector(null);
        //            byte[] DetectBuff = new byte[4096];
        //            while ((DetLen = msTemp.Read(DetectBuff, 0, DetectBuff.Length)) > 0 && !Det.IsDone())
        //            {
        //                Det.HandleData(DetectBuff, 0, DetectBuff.Length);
        //            }
        //            Det.DataEnd();
        //            if (Det.GetDetectedCharset() != null)
        //            {
        //                encoding = Encoding.GetEncoding(Det.GetDetectedCharset());
        //            }
        //        }
        //        msTemp.Close();
        //        msTemp.Dispose();
        //        return encoding;
        //    }
        //}

        private void getFileLen(string path)
        {
            FileInfo fi = new FileInfo(path);
            filelen = fi.Length;
        }

        private void open(string path)
        {
            this.filepath = path;
            label1.Text = "文件名：" + path;
            getFileLen(path);
            trackBar1.Maximum = (int)Math.Floor((double)(filelen / int.Parse(numericUpDown1.Value.ToString())));
            trackBar1.Value = 0;
            getEncodingAuto();
            read();
        }


        public static int GetHanNum(string str)
        {
            int count = 0;
            Regex regex = new Regex(@"^[\u4E00-\u9FA5]{0,}$");
            for (int i = 0; i < str.Length; i++)
            {
                if (regex.IsMatch(str[i].ToString()))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 常见汉字个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private int getCommonHanNum(string str)
        {
            int count = 0;

            foreach (char c in str)
            {
                if (Resources.commonChineseWords.Contains(c)) count++;
            }

            return count;
        }

        /// <summary>
        /// 根据字节转化为对应的字符串，并且避免（多字节码的）截断。
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private string byteToString(byte[] buffer)
        {
            int len = int.Parse(numericUpDown1.Value.ToString());
            string res = "";
            int begin = 0;
            int end = buffer.Length;
            if (encoding == Encoding.UTF8)
            {
                // 首个多字节符号头
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] >= 192 || buffer[i] < 128)   // 多字节utf-8的首字节或英文字母
                    {
                        begin = i;
                        break;
                    }
                }
                // 最后的多字节符号头（仅检查尾部2字节的余量）
                for (int i = buffer.Length - 1; i > buffer.Length - 3; i--)
                {
                    if (buffer[i] >= 192 || buffer[i] < 128)   // 多字节utf-8的首字节或英文字母
                    {
                        end = i;
                        break;
                    }
                }
            }
            else if (encoding == Encoding.GetEncoding("gb2312"))
            {
                int firstASCII = -1;
                int lastASCII = -1;
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] <= 128)
                    {
                        if (firstASCII < 0) firstASCII = i;
                        lastASCII = i;
                    }
                }
                if (firstASCII < 0)
                {
                    // no ASCII
                    // 选取常见字更多的那一种情况视为正确分割
                    byte[] tmp1 = new byte[buffer.Length - 1];
                    Array.Copy(buffer, 1, tmp1, 0, tmp1.Length);

                    if (getCommonHanNum(encoding.GetString(tmp1)) > getCommonHanNum(encoding.GetString(buffer)))
                    {
                        begin = 1;
                        end = end - 1;
                    }
                }
                else
                {
                    // have ASCII
                    // 两侧如果不是偶数长度，则必有截断。
                    if (firstASCII % 2 != 0) begin = 1;
                    if ((buffer.Length - lastASCII + 1) % 2 != 0) end = end - 1;
                }
                // 防止余量2字节以上，造成重复
                if (end - 1 > len) end -= 2;
            }
            // 防止余量的字节是ASCII码字符时，造成重复
            if (end > len && buffer[end] <= 128) end -= 1;

            byte[] buf1 = new byte[end - begin];
            Array.Copy(buffer, begin, buf1, 0, buf1.Length);
            res = encoding.GetString(buf1);
            return res;
        }

        private void getEncodingAuto()
        {
            byte[] test = readByte(0, long.Parse(numericUpDown1.Value.ToString()));
            int selectindex = 0;
            int maxhannum = -1;
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                string str = Encoding.GetEncoding(comboBox1.Items[i].ToString()).GetString(test);
                int thishannum = getCommonHanNum(str);
                if (maxhannum < thishannum)
                {
                    maxhannum = thishannum;
                    selectindex = i;
                }
            }
            this.comboBox1.SelectedIndex = selectindex;

        }

        private byte[] readByte(long begin, long len)
        {
            byte[] buffer = new byte[len];
            using (FileStream fs = File.Open(filepath, FileMode.Open))
            {
                fs.Seek(begin, SeekOrigin.Begin);
                using (MemoryStream ms = new MemoryStream())
                {
                    int read = fs.Read(buffer, 0, buffer.Length);
                }
            }
            return buffer;
        }

        private void read()
        {
            try
            {
                long beginindex = long.Parse(numericUpDown1.Value.ToString()) * trackBar1.Value;
                //Encoding encoding = Encoding.GetEncoding(getEncoding(filepath).BodyName);

                int len = int.Parse(numericUpDown1.Value.ToString());
                len += 2;   // 后都多取 2 字符，检查是否有汉字截断
                beginindex = beginindex < 0 ? 0 : beginindex;

                byte[] buffer = readByte(beginindex, len);
                string output = byteToString(buffer).Replace("\0", " ");
                textBox1.ResetText();
                textBox1.AppendText(output);
            }
            catch (Exception e)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            open(openFileDialog1.FileName);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            read();
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
            read();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            comboBox1.Select(0, 1);
        }

        private long maxindex;
        private long maxnum;
        private void backwork()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < maxindex; i++)
            {
                //if (i > 100000) break;
                long len = maxnum + 2;
                long beginindex = i * maxnum;
                beginindex = beginindex < 0 ? 0 : beginindex;
                byte[] buffer = readByte(beginindex, len);
                string output = byteToString(buffer);

                for (int j = 0; j < output.Length; j++)
                {
                    if ((output[j] >= 'A' && output[j] <= 'Z')
                        || (output[j] >= '0' && output[j] <= '9')
                        || output[j] == '-'
                        || output[j] == ' '
                        || GetHanNum(output[j].ToString()) > 0)
                    {
                        sb.Append(output[j]);
                    }
                    else
                    {
                        if (sb.Length > 20)
                        {
                            using (FileStream fs = File.Open(Path.GetDirectoryName(filepath) + "/" + Path.GetFileName(filepath) + "_output.txt", FileMode.Append))
                            {
                                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                                {
                                    sw.WriteLine(sb.ToString());
                                }
                            }
                        }
                        sb.Clear();
                    }
                }
            }
        }

        private bool isEWord(char c)
        {
            if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')) return true;
            return false;
        }

        private void backwork2()
        {
            string[] lines = File.ReadAllLines(filepath, Encoding.Default);
            Dictionary<string, Item> items = new Dictionary<string, Item>();
            foreach (var line in lines)
            {
                int hanbegin = 0;
                int hanend = 0;
                for (int i = 0; i < line.Length; i++) if (GetHanNum(line[i].ToString()) > 0) { hanbegin = i; break; }
                for (int i = line.Length - 1; i >= 0; i--) if (GetHanNum(line[i].ToString()) > 0) { hanend = i; break; }
                if (hanbegin > 0)
                {
                    string tmp = line.Substring(0, hanbegin);
                    int cutbegin = tmp.IndexOf("000");
                    string name = line.Substring(hanbegin, hanend - hanbegin + 1).Replace(" ", "");
                    if (cutbegin > 0)
                    {
                        string id = tmp.Substring(0, cutbegin);
                        string phone = tmp.Substring(cutbegin + 3);
                        if (id.Length == 5
                          || (isEWord(id[0]) && id.Length == 9)
                          || (!isEWord(id[0]) && id.Length == 8)
                          || id.Length == 18) ;
                        else
                        {
                            id = id + '0';
                        }

                        if (!items.ContainsKey(id)) items[id] = new Item(id, name, phone);
                    }
                }
            }
            using (FileStream fs = File.Open(Path.GetDirectoryName(filepath) + "/" + Path.GetFileName(filepath) + "_output.txt", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    foreach (var item in items.Values)
                    {
                        sw.WriteLine(string.Format("{0},{1},{2}", item.id, item.name, item.phone));
                    }
                }
            }

        }

        private void backwork3()
        {
            string[] lines = File.ReadAllLines(filepath, Encoding.Default);
            List<Item> res = new List<Item>();
            foreach (var line in lines)
            {
                string[] tmp = line.Split(',');
                res.Add(new Item(tmp[0], tmp[1], tmp[2]));
            }
            res.Sort();
            
            
            using (FileStream fs = File.Open(Path.GetDirectoryName(filepath) + "/" + Path.GetFileName(filepath) + "_output.txt", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    foreach (var item in res)
                    {
                        sw.WriteLine(string.Format("{0},{1},{2}", item.id, item.name, item.phone));
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            maxindex = trackBar1.Maximum;
            maxnum = long.Parse(numericUpDown1.Value.ToString());
            new Thread(backwork3).Start();

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
