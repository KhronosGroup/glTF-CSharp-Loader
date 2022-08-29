using KhronosGroup.Gltf.Generator.JsonSchema;

using NUnit.Framework;

using System.IO;

namespace KhronosGroup.Gltf.Generator.UnitTests
{
    [TestFixture]
    public class CodeGeneratorTest
    {
        private const string RelativePathToSchemaDir = @"..\..\..\..\..\glTF\specification\2.0\schema\";
        private string AbsolutePathToSchemaDir;

        [SetUp]
        public void Init()
        {
            AbsolutePathToSchemaDir = Path.Combine(TestContext.CurrentContext.TestDirectory, RelativePathToSchemaDir);
        }

        [Test]
        public void ParseSchemas_DirectReferences()
        {
            var loader = new SchemaLoader(AbsolutePathToSchemaDir + "glTFProperty.schema.json");
            loader.ParseSchemas();

            Assert.AreEqual(3, loader.FileSchemas.Keys.Count);
        }

        [Test]
        public void ParseSchemas_IndirectReferences()
        {
            var loader = new SchemaLoader(AbsolutePathToSchemaDir + "glTF.schema.json");
            loader.ParseSchemas();

            Assert.AreEqual(33, loader.FileSchemas.Keys.Count);
        }

        [Test]
        public void ExpandSchemaReferences_DirectReferences()
        {
            var loader = new SchemaLoader(AbsolutePathToSchemaDir + "glTFProperty.schema.json");
            loader.ParseSchemas();

            Assert.AreEqual(3, loader.FileSchemas.Keys.Count);

            loader.ExpandSchemaReferences();

            Assert.IsNull(loader.FileSchemas["glTFProperty.schema.json"].Properties["extensions"].ReferenceType);
            Assert.IsNull(loader.FileSchemas["glTFProperty.schema.json"].Properties["extras"].ReferenceType);
        }

        [Test]
        public void CSharpGenTest()
        {
            var loader = new SchemaLoader(AbsolutePathToSchemaDir + "glTF.schema.json");
            loader.ParseSchemas();
            loader.ExpandSchemaReferences();
            loader.EvaluateInheritance();
            loader.PostProcessSchema();
            var generator = new CodeGenerator(loader.FileSchemas);
            generator.CSharpCodeGen(TestContext.CurrentContext.TestDirectory);
        }
    }
}
