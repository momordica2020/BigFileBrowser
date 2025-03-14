using BigFileBrowser.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BigFileBrowser.Providers
{
    

    public class FileSearcher
    {
        public static async Task Search(string key, string content, sendSearchResultEvent report)
        {
            try
            {

            }catch(Exception ex)
            {

            }

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
                    var fileInfo = new FileInfo(file);
                    using (var reader = new StreamReader(file))
                    {
                        string content = await reader.ReadToEndAsync();

                        // !Regex.Escape() 允许传入正则
                        var matches = Regex.Matches(content, $"(.{{0,30}}({keyword}.){{0,30}})", RegexOptions.Singleline);

                        foreach (Match match in matches)
                        {
                            
                            string context = match.Value; // 包含关键词的上下文
                            var result = new SearchResult
                            {
                                FileInfo = fileInfo,
                                Key = keyword,
                                MatchedContext = context,
                                MatchedPositionInContext = match.Groups[1].Index - match.Index
                            };
                            report(result);
                            //await Task.Run(() => sendStringEvent([result.FileName, result.Context]));
                        }
                    }
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
