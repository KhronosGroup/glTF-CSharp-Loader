using System;
using System.IO;
using glTFLoader;
using NUnit.Framework;

namespace glTFLoaderUnitTests
{
    [TestFixture]
    public class SampleModelsTest
    {
        private const string RelativePathToSchemaDir = @"..\..\..\..\glTF-Sample-Models\2.0\";
        private string AbsolutePathToSchemaDir;

        [SetUp]
        public void Init()
        {
            AbsolutePathToSchemaDir = Path.Combine(TestContext.CurrentContext.TestDirectory, RelativePathToSchemaDir);
        }

        private glTFLoader.Schema.Gltf TestLoadFile(string filePath)
        {
            if (!filePath.EndsWith("gltf") && !filePath.EndsWith("glb")) return null;

            try
            {
                var deserializedFile = Interface.LoadModel(filePath);
                Assert.IsNotNull(deserializedFile);

                // read all buffers
                for(int i=0; i < deserializedFile.Buffers?.Length; ++i)                
                {
                    var bufferBytes = deserializedFile.LoadBinaryBuffer(filePath, i);
                    Assert.IsNotNull(bufferBytes);
                    Assert.IsTrue(deserializedFile.Buffers[i].ByteLength <= bufferBytes.Length);
                }                

                // open all images
                for(int i=0; i < deserializedFile.Images?.Length; ++i)
                {
                    using (var s = deserializedFile.OpenImageFile(filePath, i))
                    {
                        Assert.IsNotNull(s);

                        var imageHeader = new Byte[16];
                        s.Read(imageHeader, 0, 16);                        

                        // TODO: here we could check actual image headers against the expected mime type.
                    }
                }

                return deserializedFile;                
            }            
            catch (Exception e)
            {
                throw new Exception(filePath, e);
            }
        }

        [Test]
        public void SchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(AbsolutePathToSchemaDir)))
            {
                foreach (var file in Directory.EnumerateFiles(Path.Combine(dir, "glTF")))
                {
                    TestLoadFile(file);
                }
            }
        }

        [Test]
        public void SerializeTest()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(AbsolutePathToSchemaDir)))
            {
                foreach (var file in Directory.EnumerateFiles(Path.Combine(dir, "glTF")))
                {
                    if (file.EndsWith("gltf"))
                    {
                        try
                        {
                            string outPath = Path.Combine(TestContext.CurrentContext.TestDirectory, Path.GetFileName(file));
                            var deserializedFile = Interface.LoadModel(Path.GetFullPath(file));
                            Assert.IsNotNull(deserializedFile);
                            var serializedFile = glTFLoader.Interface.SerializeModel(deserializedFile);
                            Assert.IsNotNull(serializedFile);
                            Interface.SaveModel(deserializedFile, outPath);
                            deserializedFile = Interface.LoadModel(outPath);
                            Assert.IsNotNull(deserializedFile);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(file, e);
                        }
                    }
                }
            }
        }

        [Test]
        public void BinarySchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(AbsolutePathToSchemaDir)))
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
                                var len = new FileInfo(file).Length;
                                Assert.IsTrue((len & 3) == 0);

                                var deserializedFile = TestLoadFile(file);
                                Assert.IsNotNull(deserializedFile);

                                var jsonChunk = Interface.LoadModel(file);
                                Assert.IsNotNull(jsonChunk);

                                var binChunk = Interface.LoadBinaryBuffer(file);

                                Assert.IsNotNull(binChunk);
                                Assert.IsTrue((binChunk.Length & 3) == 0);

                                Assert.IsTrue(jsonChunk.Buffers[0].ByteLength <= binChunk.Length);
                                // should we check padding as with jsonChunk? some reference files fail!

                                // write to memory and reload again
                                using (var wm = new MemoryStream())
                                {
                                    jsonChunk.SaveBinaryModel(binChunk, wm);

                                    using (var rm = new MemoryStream(wm.ToArray()))
                                    {
                                        Interface.LoadModel(rm);
                                    }

                                    using (var rm = new MemoryStream(wm.ToArray()))
                                    {
                                        Interface.LoadBinaryBuffer(rm);
                                    }
                                }


                            }
                            catch (Exception e)
                            {
                                throw new Exception(file, e);
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void EmbeddedSchemaLoad()
        {
            TestLoadFile(@"D:\(_GitHub_)\glTF-Sample-Models\2.0\BoxTextured\glTF-Embedded\BoxTextured.gltf");

            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(AbsolutePathToSchemaDir)))
            {
                string path = Path.Combine(dir, "glTF-Embedded");
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        TestLoadFile(file);
                    }
                }
            }
        }

        [Test]
        public void MaterialsSchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(AbsolutePathToSchemaDir)))
            {
                string path = Path.Combine(dir, "glTF-MaterialsCommon");
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        TestLoadFile(file);
                    }
                }
            }
        }

        [Test]
        public void PBRSchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(AbsolutePathToSchemaDir)))
            {
                string path = Path.Combine(dir, "glTF-pbrSpecularGlossiness");
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        TestLoadFile(file);
                    }
                }
            }
        }

        [Test]
        public void WebGLSchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(AbsolutePathToSchemaDir)))
            {
                string path = Path.Combine(dir, "glTF-techniqueWebGL");
                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        TestLoadFile(file);
                    }
                }
            }
        }        
    }
}
