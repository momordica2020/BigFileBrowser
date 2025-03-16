using BigFileBrowser.Events;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public Encoding TextEncoding
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
                var testBytes = Read(0, testLength);
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

                length += 2;   // 多取几个字节，检查是否有汉字截断
                begin = Math.Max(0,begin);

                byte[] data = Read(begin, length);

                var startIndex = EncodingHelper.FindCharacterBoundary(TextEncoding, data);
                string result = TextEncoding.GetString(data, startIndex, data.Length - startIndex);


                
                Decoder decoder = TextEncoding.GetDecoder();
                char[] buffer = new char[TextEncoding.GetMaxCharCount(length)];
                int bytesUsed, charsUsed;
                bool completed;

                // 解码字节数组
                decoder.Convert(data, startIndex, length - startIndex, buffer, 0, buffer.Length, false, out bytesUsed, out charsUsed, out completed);

                // 如果未完成解码，说明结尾截断了多字节字符
                if (!completed)
                {
                    // 找到最后一个完整字符的边界
                    int lastValidIndex = startIndex + bytesUsed;
                    return TextEncoding.GetString(data, startIndex, lastValidIndex - startIndex).TrimEnd();
                }
                else
                {
                    // 返回解码后的字符串
                    return new string(buffer, 0, charsUsed).TrimEnd();
                }
                    

                //return EncodingHelper.byteToString(buffer, length, Encoding)
                //    .Replace("\0", " ")
                //    .Replace("\r\n", "\n")
                //    .Replace("\n", Environment.NewLine)
                //    ;
                //return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return string.Empty;
            }
        }


        /// <summary>
        /// 从全文中搜索指定关键词，支持正则
        /// </summary>
        /// <param name="key"></param>
        /// <param name="report"></param>
        /// <returns></returns>
        public async Task Search(string key, sendSearchResultEvent report)
        {
            Dictionary<long, Match> results = new Dictionary<long, Match>();
            try
            {
                Regex regex = new Regex($"{key}", RegexOptions.Singleline | RegexOptions.Compiled);

                long nowPositionByte = 0;
                //long nowPositionString = 0;
                int blockSize = 1024 * 1024 * 60;
                byte[] block = new byte[blockSize];
                while (nowPositionByte < Size)
                {
                    string res = ReadTextAsync(nowPositionByte, blockSize).Result;

                    var matches = regex.Matches(res);
                    foreach(Match match in matches)
                    {

                        long deltaIndexByte = TextEncoding.GetByteCount(res.Substring(0, match.Groups[0].Index));
                        if (!results.ContainsKey(deltaIndexByte))
                        {
                            results.Add(deltaIndexByte, match);
                            report(new SearchResult
                            {
                                FileInfo = _fileInfo,
                                Key = match.Groups[0].Value,
                                MatchedContext = res.Substring(Math.Max(0, match.Groups[0].Index - 10), Math.Min(20, res.Length - Math.Max(0, match.Groups[0].Index))),
                                MatchedPositionInContext = match.Groups[2].Index - match.Index,
                                MatchedPositionInFile = nowPositionByte + deltaIndexByte
                            });
                        }
                    }

                    //nowPositionString += res.Length / 2;
                    nowPositionByte += blockSize / 2;
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }


        ///// <summary>
        ///// 这是以key的二进制形式去搜索，所以不支持regex
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="searchMaxResult"></param>
        ///// <param name="report"></param>
        ///// <returns></returns>
        //public async Task SearchAsync(string key, int pageSize, int searchMaxResult, sendSearchResultEvent report)
        //{
        //    try
        //    {
        //        int resultCount = 0;
        //        byte[] keyb = TextEncoding.GetBytes(key);
        //        //List<SearchItem> res = new List<SearchItem>();

        //        using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
        //        {
        //            long nowPosition = 0;
        //            int blockSize = 1024 * 1024 * 60;
        //            byte[] block = new byte[blockSize];
        //            do
        //            {
        //                fs.Seek(nowPosition, SeekOrigin.Begin);
        //                fs.Read(block, 0, blockSize);
        //                if (Size - nowPosition < blockSize) for (long i = Size - nowPosition; i < blockSize; i++) block[i] = 0;
        //                bool find = false;
        //                for (int i = 0; i < blockSize - keyb.Length; i++)
        //                {
        //                    bool allsame = true;
        //                    for (int j = 0; j < keyb.Length; j++)
        //                    {
        //                        if (keyb[j] != block[i + j])
        //                        {
        //                            allsame = false;
        //                            break;
        //                        }
        //                    }
        //                    if (allsame)
        //                    {
        //                        int tbegin = Math.Max(0, i - 12);
        //                        int tcount = Math.Min(block.Length - tbegin, keyb.Length + 24);
        //                        string context = TextEncoding.GetString(block, tbegin, tcount).Replace("\0", " ");
        //                        long MatchPosition = nowPosition + i;
        //                        //int pageNum = (int)((double)MatchPosition / pageSize);
        //                        var item = new SearchResult
        //                        {
        //                            FileInfo=this._fileInfo,
        //                            MatchedContext = context,
        //                            MatchedPositionInFile = MatchPosition,
                                    
        //                        };
        //                        report(item);
        //                        resultCount++;
        //                        if (resultCount > searchMaxResult) return;
        //                        //break;
        //                    }
        //                }
        //                //Print(string.Format("{0}/{1},{2:N}%,{3}", nowPosition, filelen, (double)nowPosition * 100 / filelen, res.Count));
        //                //if (res.Count <= searchMaxResult || nowPosition == 0)
        //                //{
        //                //    //ShowSearchResult(res);
        //                //}

        //                nowPosition += (int)(0.95 * blockSize);
        //            } while (nowPosition < Size);
        //        }
        //        //Print(string.Format("{0}个结果。{1}", res.Count, res.Count > searchMaxResult ? string.Format("前{0}个如下", searchMaxResult) : ""));
        //        ///ShowSearchResult(res);

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}

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
