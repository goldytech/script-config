using System;
using System.Linq;
using EmitCode;

namespace ScriptConfig.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var scriptConfig = new ScriptConfig<AppConfiguration>().Create();
            Console.ReadLine();
        }
    }
}
