using BigFileBrowser.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BigFileBrowser.Providers
{
    

    public class FileSearcher
    {
        /// <summary>
        /// 从字符串中检索特定位置之后的首个匹配结果，如果到达字符串末尾，则返回首个
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <param name="beginPosition"></param>
        /// <returns></returns>
        public static (int index, int length, int matchCount, int matchIndex) Search(string key, string content, int beginPosition)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(content)) return (-1, 0,0,0);
            try
            {
                Regex regex = new Regex(key);
                var match = regex.Matches(content);
                if (match.Count > 0)
                {
                    for(int i=0;i<match.Count;i++)
                    {
                        var item = match[i];
                        if (item.Index > beginPosition) return (item.Index, item.Length, match.Count, i + 1);
                    }
                    return (match.First().Index, match.First().Groups[0].Length, match.Count, 1);
                }
                else
                {
                    return (-1, 0, 0, 0);
                }
            }
            catch(Exception ex)
            {

            }
            return (-1, 0, 0, 0);
        }




        /// <summary>
        /// 流式读取并匹配关键词
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async static Task<List<SearchResult>> SearchFilesForKeywordAsync(string directoryPath, string keyword, sendSearchResultEvent report)
        {
            List<SearchResult> results = new List<SearchResult>();

            try
            {
                var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    StreamFile sf = new StreamFile(file);
                    await sf.Search(keyword, report);


                    //var fileInfo = new FileInfo(file);

                    //using (var reader = new StreamReader(file))
                    //{
                    //    string content = await reader.ReadToEndAsync();

                    //    // !Regex.Escape() 允许传入正则
                    //    var matches = Regex.Matches(content, $"(.{{0,30}}({keyword}).{{0,30}})", RegexOptions.Singleline);

                    //    foreach (Match match in matches)
                    //    {

                    //        string context = match.Value; // 包含关键词的上下文
                    //        var result = new SearchResult
                    //        {
                    //            FileInfo = fileInfo,
                    //            Key = keyword,
                    //            MatchedContext = context,
                    //            MatchedPositionInContext = match.Groups[2].Index - match.Index,
                    //            MatchedPositionInFile = match.Groups[2].Index
                    //        };
                    //        report(result);
                    //        //await Task.Run(() => sendStringEvent([result.FileName, result.Context]));
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return results;
        }





    }

}
