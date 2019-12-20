using System;
using System.Collections.Generic;

namespace Yis.Csl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Prov prov = null;
            ProvInner pi = prov?.PI;
            Console.WriteLine("Prov.Name:" + pi?.name);

            string dir = AppContext.BaseDirectory;

            if (true)
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                List<long> ids = new List<long>();
                for (int i = 0; i < 10000000; i++) ids.Add(IdGeneratorHelper.Instance.GetId());

                stopwatch.Stop();
                Console.WriteLine("消耗：" + stopwatch.ElapsedMilliseconds);
            }

            if (true)
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                List<Guid> ids = new List<Guid>();
                for (int i = 0; i < 10000000; i++) ids.Add(Guid.NewGuid());

                stopwatch.Stop();
                Console.WriteLine("消耗：" + stopwatch.ElapsedMilliseconds);
            }

            Console.WriteLine(2 & 2);

            // 总位 32 减去 符号位 实际 31
            // 机器位 3 最大8 剩余 28
            // 序号位 6 最大64 剩余 22
        }

        static string Bu0(string s, int l = 64)
        {
            if (s.Length >= l) return s;
            string c0 = "";
            for (int i = 0; i < l - s.Length; i++) c0 += "0";
            return c0 + s;
        }

        class Prov
        {
            public ProvInner PI { get; set; }
        }

        class ProvInner
        {
            public string name { get; set; }

            public override string ToString()
            {
                return name;
            }
        }
    }
}
