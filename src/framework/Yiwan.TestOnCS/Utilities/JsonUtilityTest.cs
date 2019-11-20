using System;
using Yiwan.Helpers.Entities;

namespace Yiwan.TestOnCS.Utilities
{
    public class JsonUtilityTest
    {
        public static void Do()
        {
            string nullStr = null;
            string nullStrRes = Helpers.Utilities.JsonUtility.ConvertToJson(nullStr);
            string intRes = Helpers.Utilities.JsonUtility.ConvertToJson(7288);
            string stringRes = Helpers.Utilities.JsonUtility.ConvertToJson("测试字符串");
            JsonResult objJr = new JsonResult(1, "success");
            string objRes = Helpers.Utilities.JsonUtility.ConvertToJson(objJr);
            Console.WriteLine($"1.测试JsonUtility.ConvertToJson()：NullString({nullStrRes})；Int({intRes})；String({stringRes})；Object({objRes})");

            string nullStrDes = Helpers.Utilities.JsonUtility.ConvertToObject<string>(nullStrRes);
            int intDes = Helpers.Utilities.JsonUtility.ConvertToObject<int>(intRes);
            string stringDes = Helpers.Utilities.JsonUtility.ConvertToObject<string>(stringRes);
            JsonResult objDes = Helpers.Utilities.JsonUtility.ConvertToObject<JsonResult>(objRes);
            Console.WriteLine($"2.测试JsonUtility.ConvertToObject()：NullString({nullStrDes == null})；Int({intDes == 7288})；String({stringDes == "测试字符串"})；Object({objDes.code == objJr.code && objDes.data == objJr.data && objDes.msg == objJr.msg && objDes.description == objJr.description})");
        }
    }
}
