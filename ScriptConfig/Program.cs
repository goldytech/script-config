using System;
using System.Linq;

namespace ScriptConfig.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var scriptConfig = new ScriptConfig().Create<AppConfiguration>().Result;

            Console.WriteLine("Number: {0}", scriptConfig.Number);
            Console.WriteLine("Text: {0}", scriptConfig.Text);

            Console.ReadLine();
        }
    }
}
