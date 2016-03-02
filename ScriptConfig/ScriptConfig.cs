using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptConfig.Sample
{
    public class ScriptConfig<TConfig> where TConfig : new()
    {
        private readonly TConfig _config;
        private string _rootPath = AppDomain.CurrentDomain.BaseDirectory;
        private string _scriptName = "config.csx";

        private IEnumerable<Assembly> _assemblies = new[] { typeof(object).Assembly, typeof(Enumerable).Assembly, typeof(TConfig).Assembly };
        private IEnumerable<string> _namespaces = new[] { "System", "System.IO", "System.Linq", "System.Collections.Generic" };

        public ScriptConfig()
        {
            _config = new TConfig();
        }

        public ScriptConfig(TConfig config)
        {
            _config = config;
        }

        public ScriptConfig<TConfig> WithScript(string scriptName)
        {
            _scriptName = scriptName;
            return this;
        }

        public ScriptConfig<TConfig> WithRootPath(string rootPath)
        {
            _rootPath = rootPath;
            return this;
        }

        public ScriptConfig<TConfig> WithReferences(params Assembly[] assemblies)
        {
            _assemblies = assemblies.Union(_assemblies);
            return this;
        }

        public ScriptConfig<TConfig> WithNamespaces(params string[] namespaces)
        {
            _namespaces = namespaces.Union(_namespaces);
            return this;
        }

        public TConfig Create()
        {
            var code = File.ReadAllText(_rootPath + "/" + _scriptName);
            var opts = ScriptOptions.Default.
                AddImports(_namespaces).
                AddReferences(_assemblies);

            var result = CSharpScript.RunAsync(code, opts, _config, typeof(TConfig)).Result;
            return _config;
        }
    }
}
