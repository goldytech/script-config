using ScriptConfig;
using System;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var scriptConfig = new Config().Create<AppConfiguration>();
            Console.WriteLine(scriptConfig.Number);
            Console.WriteLine(scriptConfig.Text);
            Console.ReadLine();
        }
    }
}
