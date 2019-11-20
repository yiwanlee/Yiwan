using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.TestOnCS.Utilities
{
    public class JsonUtilityTest
    {
        public static void Do()
        {
            string s = null;
            string nullRes = Helpers.Utilities.JsonUtility.ConvertToJson(s);
            string intRes = Helpers.Utilities.JsonUtility.ConvertToJson(7288);
            string stringRes = Helpers.Utilities.JsonUtility.ConvertToJson("测试字符串");

            Console.WriteLine($"测试JsonUtility.ConvertToJson()：NullString({nullRes})；Int({intRes})；String({stringRes})");
        }
    }
}
