using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeneratorLib;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GeneratorUnitTests
{
    [TestFixture]
    public class SchemaTest
    {
        private const string RelativePathToSchemaDir = @"..\..\..\..\..\glTF\specification\2.0\schema\";
        private string AbsolutePathToSchemaDir;

        [SetUp]
        public void Init()
        {
            AbsolutePathToSchemaDir = Path.Combine(TestContext.CurrentContext.TestDirectory, RelativePathToSchemaDir);
        }

        [Test]
        public void SimpleSchema()
        {
            var contents = ReadContents(AbsolutePathToSchemaDir + "glTFProperty.schema.json");
            var result = JsonConvert.DeserializeObject<Schema>(contents);
            Assert.IsNotNull(result);
        }

        [Test]
        public void AllSchemas()
        {
            List<string> failingFiles = new List<string>();
            foreach (var file in Directory.EnumerateFiles(Path.GetFullPath(AbsolutePathToSchemaDir)))
            {
                if (!file.EndsWith("schema.json"))
                {
                    continue;
                }

                var contents = ReadContents(file);
                try
                {
                    var result = JsonConvert.DeserializeObject<Schema>(contents);
                }
                catch (Exception)
                {
                    failingFiles.Add(file.Replace(AbsolutePathToSchemaDir, ""));
                }
            }
            CollectionAssert.AreEqual(new string[] { }, failingFiles);
        }

        [Test]
        public void AllPropertyNames()
        {
            List<string> propertyNames = new List<string>();
            List<string> excludedNames = new List<string>();
            foreach (var file in Directory.EnumerateFiles(Path.GetFullPath(AbsolutePathToSchemaDir)))
            {
                if (!file.EndsWith("schema.json"))
                {
                    continue;
                }

                var contents = ReadContents(file);
                var reader = new JsonTextReader(new StringReader(contents));
                while (reader.Read())
                {
                    if (reader.TokenType != JsonToken.PropertyName) continue;
                    propertyNames.Add((string)reader.Value);
                }

                var result = JsonConvert.DeserializeObject<Schema>(contents);
                if (result.Properties != null)
                {
                    foreach (var key in result.Properties.Keys)
                    {
                        excludedNames.Add(key.ToLower());
                    }
                }
            }
            propertyNames = propertyNames.Select((p) => p.ToLower()).Distinct().ToList();
            var knownPropertyNames = typeof(Schema).GetProperties().Select((p) => p.Name.ToLower());
            propertyNames = propertyNames.Except(knownPropertyNames).Except(excludedNames)
                .Except(new[] { "$schema", "__ref__", "additionalproperties", "gltf_webgl", "gltf_detaileddescription", "gltf_enumnames", "gltf_uritype" }).ToList();

            CollectionAssert.AreEquivalent(new string[] { }, propertyNames);
        }

        private string ReadContents(string path)
        {
            return File.ReadAllText(Path.GetFullPath(path)).Replace("\"additionalProperties\" : false,", "").Replace("\"additionalProperties\" : false", "").Replace("\"$ref\"", "__ref__");
        }
    }
}
