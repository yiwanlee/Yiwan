using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GenPbCore
{
    /// <summary>
    /// appsettings.json文件读写
    /// <para>实际是调用JsonFileHelper</para>
    /// </summary>
    public static class AppSettingHelper
    {
        #region 同步方法

        /// <summary>
        /// 解析appsettings.json文件，并获取值
        /// <para>解析失败则返回default(T)</para>
        /// <para>同步方法</para>
        /// </summary>
        /// <param name="key">json键，层级键值使用冒号(例 system:website:seo)</param>
        /// <param name="file">文件路径 支持1完整路径2文件名3文件名不含扩展名；非完整路径时默认在程序根目录查找</param>
        public static string Get(string key)
        {
            return JsonFileHelper.Get(key, "appsettings.json");
        }

        /// <summary>
        /// 解析appsettings.json文件，并获取值
        /// <para>解析失败则返回default(T)</para>
        /// <para>同步方法</para>
        /// </summary>
        /// <typeparam name="T">返回类型，如果涉及返回List，请试用GetList</typeparam>
        /// <param name="key">json键，层级键值使用冒号(例 system:website:seo)</param>
        /// <param name="file">文件路径 支持1完整路径2文件名3文件名不含扩展名；非完整路径时默认在程序根目录查找</param>
        public static T Get<T>(string key)
        {
            return JsonFileHelper.Get<T>(key, "appsettings.json");
        }

        /// <summary>
        /// 解析appsettings.json文件，并获取集合
        /// <para>解析失败则返回default(T)</para>
        /// <para>同步方法</para>
        /// </summary>
        /// <typeparam name="T">返回集合元素类型</typeparam>
        /// <param name="key">json键，层级键值使用冒号(例 system:website:seo)</param>
        /// <param name="file">文件路径 支持1完整路径2文件名3文件名不含扩展名；非完整路径时默认在程序根目录查找</param>
        public static List<T> GetList<T>(string key)
        {
            return JsonFileHelper.GetList<T>(key, "appsettings.json");
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 解析appsettings.json文件，并获取值
        /// <para>解析失败则返回default(T)</para>
        /// <para>异步方法</para>
        /// </summary>
        /// <param name="key">json键，层级键值使用冒号(例 system:website:seo)</param>
        /// <param name="file">文件路径 支持1完整路径2文件名3文件名不含扩展名；非完整路径时默认在程序根目录查找</param>
        public static async Task<string> GetAsync(string key)
        {
            return await JsonFileHelper.GetAsync(key, "appsettings.json");
        }

        /// <summary>
        /// 解析appsettings.json文件，并获取值
        /// <para>解析失败则返回default(T)</para>
        /// <para>异步方法</para>
        /// </summary>
        /// <typeparam name="T">返回类型，如果涉及返回List，请试用GetList</typeparam>
        /// <param name="key">json键，层级键值使用冒号(例 system:website:seo)</param>
        /// <param name="file">文件路径 支持1完整路径2文件名3文件名不含扩展名；非完整路径时默认在程序根目录查找</param>
        public static async Task<T> GetAsync<T>(string key)
        {
            return await JsonFileHelper.GetAsync<T>(key, "appsettings.json");
        }

        /// <summary>
        /// 解析appsettings.json文件，并获取集合
        /// <para>解析失败则返回default(T)</para>
        /// <para>异步方法</para>
        /// </summary>
        /// <typeparam name="T">返回集合元素类型</typeparam>
        /// <param name="key">json键，层级键值使用冒号(例 system:website:seo)</param>
        /// <param name="file">文件路径 支持1完整路径2文件名3文件名不含扩展名；非完整路径时默认在程序根目录查找</param>
        public static async Task<List<T>> GetListAsync<T>(string key)
        {
            return await JsonFileHelper.GetListAsync<T>(key, "appsettings.json");
        }

        #endregion 异步方法
    }
}
