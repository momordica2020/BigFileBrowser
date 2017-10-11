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
        Encoding encoding=Encoding.UTF8;

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
            read(filelen / 1000 * trackBar1.Value);
        }

        private void read(long beginindex)
        {
            try
            {
                //Encoding encoding = Encoding.GetEncoding(getEncoding(filepath).BodyName);
                using (FileStream fs = File.Open(filepath, FileMode.Open))
                {
                    fs.Seek(beginindex, SeekOrigin.Begin);

                    int len = int.Parse(numericUpDown1.Value.ToString());
                    byte[] buffer = new byte[len];
                    using (MemoryStream ms = new MemoryStream())
                    {

                        int read = fs.Read(buffer,0, buffer.Length);
                        ms.Write(buffer, 0, read);
                        string res = encoding.GetString(ms.ToArray());
                        textBox1.Text = res;
                    }
                }

            }
            catch
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
            read(filelen / 1000* trackBar1.Value );
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
            read(filelen / 1000 * trackBar1.Value);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            comboBox1.Select(0, 1);
        }
    }
}
