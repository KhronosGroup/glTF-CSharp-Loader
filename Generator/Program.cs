using System;
using System.IO;
using System.Linq;

using GeneratorLib;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var schemaPath = Path.Combine(
                // glTF-CSharp-Loader\Generator\bin\Debug\net7.0
                "..", "..", "..", "..", "..",
                "glTF", "specification", "2.0", "schema", "glTF.schema.json");
            if (!File.Exists(schemaPath))
                throw new FileNotFoundException(
                    $"Could not find '{Path.GetFileName(schemaPath)}' " +
                    $"at '{Path.GetDirectoryName(schemaPath)}' " +
                    $"(full path {Path.GetFullPath(Path.GetDirectoryName(schemaPath))}).");
            var generator = new CodeGenerator(schemaPath);
            generator.ParseSchemas();
            generator.ExpandSchemaReferences();
            generator.EvaluateInheritance();
            generator.PostProcessSchema();
            var outputDirPath = Path.Combine("..", "..", "..", "..", "glTFLoader", "Schema");
            generator.CSharpCodeGen(Path.GetFullPath(outputDirPath));
        }
    }
}
