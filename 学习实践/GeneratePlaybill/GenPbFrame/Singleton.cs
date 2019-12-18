using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenPbFrame
{
    public sealed class Singleton
    {
        static Singleton instance = null;

        public void Show()
        {
            Console.WriteLine("实例方法 函数");
        }
        private Singleton()
        {
            Console.WriteLine("Private 构造函数");
        }

        public static Singleton Instance
        {
            get
            {
                Console.WriteLine("静态属性 Get");
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }

    /// <summary>
    /// 静态内部类单例模式，线程安全
    /// </summary>
    public class StaticSingleton
    {
        static StaticSingleton instance = null;

        public string Id { get; set; }

        private StaticSingleton()
        {
            Console.WriteLine("构造函数 StaticSingleton");
            this.Id = Guid.NewGuid().ToString();
        }

        public static void Hello()
        {
            Console.WriteLine("静态函数 Hello");
        }

        public static StaticSingleton Instance
        {
            get
            {
                Console.WriteLine("静态属性 Get");
                if (instance == null)
                {
                    instance = Nested.instance;
                }
                return instance;
            }
        }

        private class Nested
        {
            internal static readonly StaticSingleton instance = null;
            static Nested()
            {
                Console.WriteLine("静态构造函数 StaticSingleton");
                instance = new StaticSingleton();
            }
        }
    }
}
