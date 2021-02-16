using System.IO;
using GeneratorLib;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var schemaPath = Path.Combine("..", "..", "..", "..", "..", "glTF", "specification", "2.0", "schema", "glTF.schema.json");
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
