using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.TestOnCS
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Utilities测试 ▼▼▼▼▼

            Console.WriteLine("Utilities测试 ▼▼▼▼▼");

            Utilities.JsonUtilityTest.Do();

            Console.WriteLine("Utilities测试 ▲▲▲▲▲");

            #endregion Utilities测试 ▲▲▲▲▲






            while (true) Console.ReadKey();
        }
    }
}
