using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptConfig.Sample
{
    public class ScriptConfig
    {
        private string _rootPath = AppDomain.CurrentDomain.BaseDirectory;
        private string _scriptName = "config.csx";

        private IEnumerable<Assembly> _assemblies = new[] { typeof(object).Assembly, typeof(Enumerable).Assembly };
        private IEnumerable<string> _namespaces = new[] { "System", "System.IO", "System.Linq", "System.Collections.Generic" };

        public ScriptConfig WithScript(string scriptName)
        {
            _scriptName = scriptName;
            return this;
        }

        public ScriptConfig WithRootPath(string rootPath)
        {
            _rootPath = rootPath;
            return this;
        }

        public ScriptConfig WithReferences(params Assembly[] assemblies)
        {
            _assemblies = assemblies.Union(_assemblies);
            return this;
        }

        public ScriptConfig WithNamespaces(params string[] namespaces)
        {
            _namespaces = namespaces.Union(_namespaces);
            return this;
        }

        public Task<TConfig> Create<TConfig>() where TConfig : new()
        {
            return Create(new TConfig());
        }

        public async Task<TConfig> Create<TConfig>(TConfig config)
        {
            var code = File.ReadAllText(Path.Combine(_rootPath, _scriptName));
            var opts = ScriptOptions.Default.AddImports(_namespaces).AddReferences(_assemblies).AddReferences(typeof(TConfig).Assembly);

            var script = CSharpScript.Create(code, opts, typeof (TConfig));
            var result = await script.RunAsync(config);

            return config;
        }
    }
}
