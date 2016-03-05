using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace ScriptConfig
{
    public class Config
    {
        private string _rootPath = AppContext.BaseDirectory;
        private string _scriptName = "config.csx";

        private IEnumerable<Assembly> _assemblies = new[] {typeof (object).GetTypeInfo().Assembly, typeof (Enumerable).GetTypeInfo().Assembly};
        private IEnumerable<string> _namespaces = new[] {"System", "System.IO", "System.Linq", "System.Collections.Generic"};

        public Config WithScript(string scriptName)
        {
            _scriptName = scriptName;
            return this;
        }

        public Config WithRootPath(string rootPath)
        {
            _rootPath = rootPath;
            return this;
        }

        public Config WithReferences(params Assembly[] assemblies)
        {
            _assemblies = assemblies.Union(_assemblies);
            return this;
        }

        public Config WithNamespaces(params string[] namespaces)
        {
            _namespaces = namespaces.Union(_namespaces);
            return this;
        }

        public TConfig Create<TConfig>() where TConfig : new()
        {
            return Create(new TConfig());
        }

        public unsafe TConfig Create<TConfig>(TConfig config)
        {
            var code = File.ReadAllText(_rootPath + "/" + _scriptName);

            byte* b;
            int length;
            var assembly = typeof (TConfig).GetTypeInfo().Assembly;
            if (assembly.TryGetRawMetadata(out b, out length))
            {
                var moduleMetadata = ModuleMetadata.CreateFromMetadata((IntPtr) b, length);
                var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                var reference = assemblyMetadata.GetReference();

                var opts = ScriptOptions.Default.AddImports(_namespaces).AddReferences(_assemblies).AddReferences(reference);
                var script = CSharpScript.Create(code, opts, typeof (TConfig));
                var result = script.RunAsync(config).Result;
            }

            return config;
        }
    }
}