using KhronosGroup.Gltf.Generator.JsonSchema;

using System.IO;

namespace KhronosGroup.Gltf.Generator
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
            var loader = new SchemaLoader(schemaPath);
            loader.ParseSchemas();
            loader.ExpandSchemaReferences();
            loader.EvaluateInheritance();
            loader.PostProcessSchema();
            var generator = new CodeGenerator(loader.FileSchemas);
            var outputDirPath = Path.Combine("..", "..", "..", "..", "glTFLoader", "Schema");
            generator.CSharpCodeGen(Path.GetFullPath(outputDirPath));
        }
    }
}
