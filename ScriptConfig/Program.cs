using System;
using System.Linq;

namespace ScriptConfig.Sample
{
    using System.Collections.Generic;
    using System.Reflection;

    class Program
    {
        static void Main(string[] args)
        {
            var scriptConfig = new ScriptConfig().Create<AppConfiguration>().Result;

            Console.WriteLine("Number: {0}", scriptConfig.Number);
            Console.WriteLine("Text: {0}", scriptConfig.Text);
            Console.WriteLine("======================");

            var scriptConfig2 =
                new ScriptConfig().WithScript("config2.csx")
                    .WithNamespaces(typeof(DataTarget).Namespace)
                    .Create<MyAppConfig>().Result;

            Console.WriteLine("DataTarget: {0}", scriptConfig2.Target);
            Console.WriteLine("AppUrl: {0}", scriptConfig2.AppUrl);
            Console.WriteLine("CacheTime: {0}", scriptConfig2.CacheTime);
            Console.WriteLine("======================");

            var mynamespaces = new List<string> { "System.Windows.Forms" };
            var myAssemblies = new List<Assembly>();
            myAssemblies.Add(Assembly.LoadFrom(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Windows.Forms.dll"));
            var code =
                new ScriptConfig().WithNamespaces(mynamespaces.ToArray()).WithReferences(myAssemblies.ToArray()).WithScript("code.csx").Create(new object()).Result;

            Console.ReadLine();
        }
    }
}
