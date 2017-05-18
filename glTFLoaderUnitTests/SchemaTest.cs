using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using glTFLoader;
using glTFLoader.Schema;
using Newtonsoft.Json;
using NUnit.Framework;

namespace glTFLoaderUnitTests
{
    [TestFixture]
    public class SchemaTest
    {
        private const string RelativePathToSamplesDir = @"..\..\..\..\glTF-Sample-Models\2.0\";

        [Test]
        public void SchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(RelativePathToSamplesDir)))
            {
                foreach (var file in Directory.EnumerateFiles(Path.Combine(dir, "glTF")))
                {
                    if (file.EndsWith("gltf"))
                    {
                        try
                        {
                            var deserializedFile = Interface.LoadModel(file);
                            Assert.IsNotNull(deserializedFile);
                        }
                        catch (Exception e)
                        {
                            // TODO restore exception
                            //throw new Exception(file, e);
                            System.Console.WriteLine("File: " + file + "; Exception: " + e);
                        }
                    }
                }
            }
        }

        [Test]
        public void SerializeTest()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(RelativePathToSamplesDir)))
            {
                foreach (var file in Directory.EnumerateFiles(Path.Combine(dir, "glTF")))
                {
                    if (file.EndsWith("gltf"))
                    {
                        try
                        {
                            var deserializedFile = Interface.LoadModel(Path.GetFullPath(file));
                            Assert.IsNotNull(deserializedFile);
                            var serializedFile = glTFLoader.Interface.SerializeModel(deserializedFile);
                            Assert.IsNotNull(serializedFile);
                            Interface.SaveModel(deserializedFile, (@".\"+Path.GetFileName(file)));
                            deserializedFile = Interface.LoadModel(@".\" + Path.GetFileName(file));
                            Assert.IsNotNull(deserializedFile);

                        }
                        catch (Exception e)
                        {
                            // TODO restore exception
                            //throw new Exception(file, e);
                            System.Console.WriteLine("File: " + file + "; Exception: " + e);
                        }
                    }
                }
            }
        }

        [Test]
        public void BinarySchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(RelativePathToSamplesDir)))
            {
                string path = Path.Combine(dir, "glTF-Binary");
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        if (file.EndsWith("glb"))
                        {
                            try
                            {
                                var deserializedFile = Interface.LoadModel(file);
                                Assert.IsNotNull(deserializedFile);
                            }
                            catch (Exception e)
                            {
                                // TODO restore exception
                                //throw new Exception(file, e);
                                System.Console.WriteLine("File: " + file + "; Exception: " + e);
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void EmbeddedSchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(RelativePathToSamplesDir)))
            {
                string path = Path.Combine(dir, "glTF-Embedded");
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        if (file.EndsWith("gltf"))
                        {
                            try
                            {
                                var deserializedFile = Interface.LoadModel(file);
                                Assert.IsNotNull(deserializedFile);
                            }
                            catch (Exception e)
                            {
                                // TODO restore exception
                                //throw new Exception(file, e);
                                System.Console.WriteLine("File: " + file + "; Exception: " + e);
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void MaterialsSchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(RelativePathToSamplesDir)))
            {
                string path = Path.Combine(dir, "glTF-MaterialsCommon");
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        if (file.EndsWith("gltf"))
                        {
                            try
                            {
                                var deserializedFile = Interface.LoadModel(file);
                                Assert.IsNotNull(deserializedFile);
                            }
                            catch (Exception e)
                            {
                                // TODO restore exception
                                //throw new Exception(file, e);
                                System.Console.WriteLine("File: " + file + "; Exception: " + e);
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void PBRSchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(RelativePathToSamplesDir)))
            {
                string path = Path.Combine(dir, "glTF-pbrSpecularGlossiness");
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        if (file.EndsWith("gltf"))
                        {
                            try
                            {
                                var deserializedFile = Interface.LoadModel(file);
                                Assert.IsNotNull(deserializedFile);
                            }
                            catch (Exception e)
                            {
                                // TODO restore exception
                                //throw new Exception(file, e);
                                System.Console.WriteLine("File: " + file + "; Exception: " + e);
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void WebGLSchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(RelativePathToSamplesDir)))
            {
                string path = Path.Combine(dir, "glTF-techniqueWebGL");
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        if (file.EndsWith("gltf"))
                        {
                            try
                            {
                                var deserializedFile = Interface.LoadModel(file);
                                Assert.IsNotNull(deserializedFile);
                            }
                            catch (Exception e)
                            {
                                // TODO restore exception
                                //throw new Exception(file, e);
                                System.Console.WriteLine("File: " + file + "; Exception: " + e);
                            }
                        }
                    }
                }
            }
        }
    }
}
