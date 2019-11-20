using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.TestOnCS
{
    class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("样式", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
        static void Main(string[] args)
        {
            #region Utilities测试 ▼▼▼▼▼

            Console.WriteLine("Utilities测试 ▼▼▼▼▼\r\n");

            Utilities.JsonUtilityTest.Do();

            Console.WriteLine("\r\nUtilities测试 ▲▲▲▲▲");

            #endregion Utilities测试 ▲▲▲▲▲






            while (true) Console.ReadKey();
        }
    }
}
