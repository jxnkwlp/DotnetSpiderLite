//using DotnetSpiderLite.Downloader;
//using DotnetSpiderLite.Html;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace DotnetSpiderLite.Extraction
//{
//    /// <summary>
//    ///  页面解析
//    /// </summary>
//    public interface IHtmlExtracter
//    {
//        /// <summary>
//        ///   提取所有 
//        /// </summary> 
//        IEnumerable<IHtmlElement> ExtractAllByXPath(string html, string path);

//        IEnumerable<IHtmlElement> ExtractAllByXPath(Page page, string path);


//        /// <summary>
//        ///  提取所有
//        /// </summary> 
//        IEnumerable<IHtmlElement> ExtractAllByCss(string html, string path);
//        IEnumerable<IHtmlElement> ExtractAllByCss(Page page, string path);

//        /// <summary>
//        ///  提取
//        /// </summary> 
//        IHtmlElement ExtractByXPath(string html, string path);
//        IHtmlElement ExtractByXPath(Page page, string path);

//        /// <summary>
//        ///  提取
//        /// </summary> 
//        IHtmlElement ExtractByCss(string html, string path);
//        IHtmlElement ExtractByCss(Page page, string path);


//    }
//}
