using BigFileBrowser.Events;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigFileBrowser.Providers
{
    class StreamFile
    {
        private string _path = "";
        private long _size;
        private long _position = 0;
        private long _length = 0;
        private Encoding _encoding = Encoding.UTF8;
        private FileInfo _fileInfo = null;
        private object ReadMutex = new();

        /// <summary>
        /// 文件大小，单位是byte
        /// </summary>
        public long Size
        {
            get
            {
                return _size;
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }

        public string Name
        {
            get
            {
                if (_fileInfo == null) return "";
                return _fileInfo.Name;
            }
        }

        public string SizeString
        {
            get
            {
                if (Size > 1024 * 1024 * 1024 * 1) return $"{(Size / 1024.0 / 1024 / 1024):N2} GB";
                else if (Size > 1024 * 1024 * 1) return $"{(Size / 1024.0 / 1024):N2} MB";
                else if (Size > 1024 * 1) return $"{(Size / 1024.0):N2} KB";
                else return $"{Size} B";
            }
        }

        /// <summary>
        /// 文件编码格式
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                _encoding = value;

            }
        }

        public StreamFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;
            try
            {
                _path = path;
                
                _fileInfo = new FileInfo(_path);
                
                _size = _fileInfo.Length;

                // encoding
                AutoSetEncoding();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        /// <summary>
        /// 自动选择文件编码
        /// </summary>
        private void AutoSetEncoding()
        {
            try
            {
                long testLength = Math.Min(Size, 1000);
                var testBytes = Read(0, 1000);
                _encoding = EncodingHelper.AutoChoose(testBytes);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }


        }



        private byte[] Read(long begin, long len)
        {
            byte[] buffer = new byte[len];

            try
            {
                using (FileStream fs = File.Open(Path, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(begin, SeekOrigin.Begin);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read = fs.Read(buffer, 0, buffer.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


            return buffer;
        }


        /// <summary>
        /// 从文件读取特定位置的一段数据，并根据相应编码格式解码之
        /// </summary>
        /// <param name="beginnum"></param>
        /// <returns></returns>
        public async Task<string> ReadTextAsync(long begin, int length)
        {
            try
            {
                //long beginindex = beginnum * pagelen;
                //Encoding encoding = Encoding.GetEncoding(getEncoding(filepath).BodyName);

                int len = length;
                len += 2;   // 后都多取 2 字符，检查是否有汉字截断
                begin = begin < 0 ? 0 : begin;

                byte[] buffer = Read(begin, len);
                return EncodingHelper.byteToString(buffer, length, Encoding)
                    .Replace("\0", " ")
                    .Replace("\r\n", "\n")
                    .Replace("\n", Environment.NewLine)
                    ;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        public async Task SearchAsync(string key, int pageSize, int searchMaxResult, sendSearchResultEvent report)
        {
            try
            {
                int resultCount = 0;
                byte[] keyb = Encoding.GetBytes(key);
                //List<SearchItem> res = new List<SearchItem>();

                using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
                {
                    long nowPosition = 0;
                    int dl = 1024 * 1024 * 60;
                    byte[] blockb = new byte[dl];
                    do
                    {
                        fs.Seek(nowPosition, SeekOrigin.Begin);
                        fs.Read(blockb, 0, dl);
                        if (Size - nowPosition < dl) for (long i = Size - nowPosition; i < dl; i++) blockb[i] = 0;
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
                                string context = Encoding.GetString(blockb, tbegin, tcount).Replace("\0", " ");
                                long MatchPosition = nowPosition + i;
                                //int pageNum = (int)((double)MatchPosition / pageSize);
                                var item = new SearchResult
                                {
                                    FileInfo=this._fileInfo,
                                    MatchedContext = context,
                                    MatchedPositionInFile = MatchPosition,
                                    
                                };
                                report(item);
                                resultCount++;
                                if (resultCount > searchMaxResult) return;
                                //break;
                            }
                        }
                        //Print(string.Format("{0}/{1},{2:N}%,{3}", nowPosition, filelen, (double)nowPosition * 100 / filelen, res.Count));
                        //if (res.Count <= searchMaxResult || nowPosition == 0)
                        //{
                        //    //ShowSearchResult(res);
                        //}

                        nowPosition += (int)(0.95 * dl);
                    } while (nowPosition < Size);
                }
                //Print(string.Format("{0}个结果。{1}", res.Count, res.Count > searchMaxResult ? string.Format("前{0}个如下", searchMaxResult) : ""));
                ///ShowSearchResult(res);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Dispose()
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
