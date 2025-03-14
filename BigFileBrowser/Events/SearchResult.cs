using System;
using System.IO;

namespace BigFileBrowser.Events
{
    //public class Item : IComparable<Item>
    //{
    //    public string id;
    //    public string name;
    //    public string phone;
    //    public Item(string _id, string _name, string _phone)
    //    {
    //        id = _id;
    //        name = _name;
    //        phone = _phone;
    //    }

    //    public int CompareTo(Item other)
    //    {
    //        return id.Length.CompareTo(other.id.Length);
    //    }
    //}


    public class SearchResult
    {
        /// <summary>
        /// 所属文件
        /// </summary>
        public FileInfo FileInfo = null;

        /// <summary>
        /// 连带上下文的匹配结果
        /// </summary>
        public string MatchedContext = string.Empty;

        /// <summary>
        /// 在该上下文中的相对位置，用于高亮等
        /// </summary>
        public int MatchedPositionInContext = 0;

        /// <summary>
        /// 在该文件中的相对位置，单位是字节
        /// </summary>
        public long MatchedPositionInFile = 0;

        /// <summary>
        /// 所匹配的关键词
        /// </summary>
        public string Key = string.Empty;

        

        //public SearchItem(string content, int pagenum, long index)
        //{
        //    this.Content = content;
        //    this.PageNum = pagenum;
        //    this.Index = index;
        //}
    }

}
