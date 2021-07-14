using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SystemCommonLibrary.Reflect
{
    public static class CompilerHelper
    {
        public static Assembly Compile(string assemblyName, string text, params Assembly[] referencedAssemblies)
        {
            var references = referencedAssemblies.Select(it => MetadataReference.CreateFromFile(it.Location));
            var syntaxTrees = new SyntaxTree[] { CSharpSyntaxTree.ParseText(text) };
            var compilation = CSharpCompilation.Create(assemblyName, syntaxTrees, references, GetCompilationOptions());
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                    Path.GetRandomFileName().Replace(".", string.Empty).ToLower() + ".dll"
                                    ).Replace("\\", "/");
            var compilationResult = compilation.Emit(file);
            if (compilationResult.Success)
            {
                return Assembly.LoadFrom(file);
            }
            else
            {
                var msg = string.Join(',', compilationResult.Diagnostics.Select(d => d.ToString()));
                throw new InvalidOperationException($"Compilation error: {msg}");
            }

        }

        private readonly static ConcurrentDictionary<string, ReportDiagnostic> _globalSuppressDiagnostics = new ConcurrentDictionary<string, ReportDiagnostic>();
        public static void AddGlobalSupperess(string errorcode)
        {
            _globalSuppressDiagnostics[errorcode] = ReportDiagnostic.Suppress;
        }
        private static CSharpCompilationOptions GetCompilationOptions()
        {
            AddGlobalSupperess("CS1701");
            AddGlobalSupperess("CS1702");
            AddGlobalSupperess("CS1705");
            AddGlobalSupperess("CS162");
            AddGlobalSupperess("CS219");
            AddGlobalSupperess("CS0414");
            AddGlobalSupperess("CS0616");
            AddGlobalSupperess("CS0649");
            AddGlobalSupperess("CS0693");
            AddGlobalSupperess("CS1591");
            AddGlobalSupperess("CS1998");

            var compilationOptions = new CSharpCompilationOptions(
                                   concurrentBuild: true,
                                   moduleName: Guid.NewGuid().ToString("D"),
                                   reportSuppressedDiagnostics: false,
                                   metadataImportOptions: MetadataImportOptions.All,
                                   outputKind: OutputKind.DynamicallyLinkedLibrary,
                                   optimizationLevel: OptimizationLevel.Release,
                                   allowUnsafe: true,
                                   platform: Platform.X64,
                                   checkOverflow: false,
                                   assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default,
                                   specificDiagnosticOptions: _globalSuppressDiagnostics);

            return compilationOptions;

        }
    }
}
