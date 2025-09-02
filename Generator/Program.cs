using KhronosGroup.Gltf.Generator.JsonSchema;

using System.Collections.Generic;
using System.IO;

namespace KhronosGroup.Gltf.Generator
{
    class Program
    {
        static string defaultSchemaPath = Path.Combine(
            // glTF-CSharp-Loader\Generator\bin\Debug\net7.0
            "..", "..", "..", "..", "..",
            "glTF", "specification", "2.0", "schema", "glTF.schema.json");
        static string defaultOutputPath = Path.Combine(
            "..", "..", "..", "..", "glTFLoader", "Schema");

        static void Usage(int exitCode)
        {
            var executableName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            System.Console.WriteLine($"Usage: {executableName} [options] [root schema file]");
            System.Console.WriteLine("\t[options]");
            System.Console.WriteLine("\t\t-h Display this help and exit");
            System.Console.WriteLine("\t\t-search <directory> Search <directory> for schema file references");
            System.Console.WriteLine($"\t\t-output <directory> Output to <directory> (default {defaultOutputPath})");
            System.Console.WriteLine($"\t[root schema file] Schema file to parse (default {defaultSchemaPath})");
            System.Environment.Exit(exitCode);
        }

        static void Error(string message)
        {
            System.Console.Error.WriteLine(message);
            System.Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            string rootSchemaPath = null;
            string outputDirPath = null;
            var searchDirs = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-h" || args[i] == "--help" || args[i] == "/?")
                {
                    Usage(0);
                }
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-search")
                {
                    i++;
                    if (args.Length > i && Path.Exists(args[i]) &&
                        (File.GetAttributes(args[i]) & FileAttributes.Directory) != 0)
                    {
                        searchDirs.Add(args[i]);
                    }
                    else
                    {
                        Error("-search: Requires a directory");
                    }
                }
                else if (args[i] == "-output")
                {
                    i++;
                    if (args.Length > i && Path.Exists(args[i]) &&
                        (File.GetAttributes(args[i]) & FileAttributes.Directory) != 0)
                    {
                        outputDirPath = args[i];
                    }
                    else
                    {
                        Error("-output: Requires a directory");
                    }
                 }
                else
                {
                    if (File.Exists(args[i]) &&
                        (File.GetAttributes(args[i]) & FileAttributes.Normal) != 0)
                    {
                        if (rootSchemaPath == null)
                        {
                            rootSchemaPath = args[i];
                        }
                        else
                        {
                            Error("Only one root schema can be specified");
                        }
                    }
                    else
                    {
                        Error($"Unhandled argument: {args[i]}");
                    }
                }
            }

            rootSchemaPath = rootSchemaPath ?? defaultSchemaPath;
            outputDirPath = outputDirPath ?? defaultOutputPath;

            if (!File.Exists(rootSchemaPath))
                throw new FileNotFoundException(
                    $"Could not find '{Path.GetFileName(rootSchemaPath)}' " +
                    $"at '{Path.GetDirectoryName(rootSchemaPath)}' " +
                    $"(full path {Path.GetFullPath(Path.GetDirectoryName(rootSchemaPath))}).");
            var loader = new SchemaLoader(rootSchemaPath);

            for (int i = 0; i < searchDirs.Count; i++)
            {
                loader.AppendSchemaSearchDirectory(searchDirs[i]);
            }

            loader.ParseSchemas();
            loader.ExpandSchemaReferences();
            loader.EvaluateInheritance();
            loader.PostProcessSchema();
            var generator = new CodeGenerator(loader.FileSchemas);
            generator.CSharpCodeGen(Path.GetFullPath(outputDirPath));
        }
    }
}
