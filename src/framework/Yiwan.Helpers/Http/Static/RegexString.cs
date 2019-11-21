using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yiwan.Helpers.Http.Static
{
    /// <summary>
    /// 正则表达式静态类
    /// </summary>
    internal class RegexString
    {
        /// <summary>
        /// 获取所有的A链接
        /// </summary>
        internal const string Alist = "<a[\\s\\S]+?href[=\"\']([\\s\\S]+?)[\"\'\\s+][\\s\\S]+?>([\\s\\S]+?)</a>";
        /// <summary>
        /// 获取所有的Img标签
        /// </summary>
        internal const string ImgList = "<img[\\s\\S]*?src=[\"\']([\\s\\S]*?)[\"\'][\\s\\S]*?>([\\s\\S]*?)>";
        /// <summary>
        /// 所有的Nscript
        /// </summary>
        internal const string Nscript = "<nscript[\\s\\S]*?>[\\s\\S]*?</nscript>";
        /// <summary>
        /// 所有的Style
        /// </summary>
        internal const string Style = "<style[\\s\\S]*?>[\\s\\S]*?</style>";
        /// <summary>
        /// 所有的Script
        /// </summary>
        internal const string Script = "<script[\\s\\S]*?>[\\s\\S]*?</script>";
        /// <summary>
        /// 所有的Html
        /// </summary>
        internal const string Html = "<[\\s\\S]*?>";
        /// <summary>
        /// 换行符号
        /// </summary>
        internal static readonly string NewLine = Environment.NewLine;
        /// <summary>
        ///获取网页编码
        /// </summary>
        internal const string Enconding = "<meta[^<]*charset=([^<]*)[\"']";
        /// <summary>
        /// 所有Html
        /// </summary>
        internal const string AllHtml = "([\\s\\S]*?)";
        /// <summary>
        /// title
        /// </summary>
        internal const string HtmlTitle = "<title>([\\s\\S]*?)</title>";
    }
}
