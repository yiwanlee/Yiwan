using System;

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
            

            Console.WriteLine(dir);
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
