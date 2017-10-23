using System.IO;
using GeneratorLib;
using NUnit.Framework;

namespace GeneratorUnitTests
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
            var generator = new CodeGenerator(AbsolutePathToSchemaDir + "glTFProperty.schema.json");
            generator.ParseSchemas();

            Assert.AreEqual(3, generator.FileSchemas.Keys.Count);
        }

        [Test]
        public void ParseSchemas_IndirectReferences()
        {
            var generator = new CodeGenerator(AbsolutePathToSchemaDir + "glTF.schema.json");
            generator.ParseSchemas();

            Assert.AreEqual(33, generator.FileSchemas.Keys.Count);
        }

        [Test]
        public void ExpandSchemaReferences_DirectReferences()
        {
            var generator = new CodeGenerator(AbsolutePathToSchemaDir + "glTFProperty.schema.json");
            generator.ParseSchemas();

            Assert.AreEqual(3, generator.FileSchemas.Keys.Count);

            generator.ExpandSchemaReferences();

            Assert.IsNull(generator.FileSchemas["glTFProperty.schema.json"].Properties["extensions"].ReferenceType);
            Assert.IsNull(generator.FileSchemas["glTFProperty.schema.json"].Properties["extras"].ReferenceType);
        }

        [Test]
        public void CSharpGenTest()
        {
            var generator = new CodeGenerator(AbsolutePathToSchemaDir + "glTF.schema.json");
            generator.ParseSchemas();
            generator.ExpandSchemaReferences();
            generator.EvaluateInheritance();
            generator.PostProcessSchema();
            generator.CSharpCodeGen(TestContext.CurrentContext.TestDirectory);
        }
    }
}
