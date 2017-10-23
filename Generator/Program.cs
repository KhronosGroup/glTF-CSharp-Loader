using System.IO;
using GeneratorLib;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new CodeGenerator(@"..\..\..\..\..\glTF\specification\2.0\schema\glTF.schema.json");
            generator.ParseSchemas();
            generator.ExpandSchemaReferences();
            generator.EvaluateInheritance();
            generator.PostProcessSchema();
            generator.CSharpCodeGen(Path.GetFullPath(@"..\..\..\..\glTFLoader\Schema"));
        }
    }
}
