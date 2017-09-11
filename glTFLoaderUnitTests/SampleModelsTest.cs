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

        [Test]
        public void SchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(AbsolutePathToSchemaDir)))
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
                            throw new Exception(file, e);
                        }
                    }
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
                                var deserializedFile = Interface.LoadModel(file);
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
        }

        [Test]
        public void EmbeddedSchemaLoad()
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(AbsolutePathToSchemaDir)))
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
                                throw new Exception(file, e);
                            }
                        }
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
                        if (file.EndsWith("gltf"))
                        {
                            try
                            {
                                var deserializedFile = Interface.LoadModel(file);
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
                        if (file.EndsWith("gltf"))
                        {
                            try
                            {
                                var deserializedFile = Interface.LoadModel(file);
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
                        if (file.EndsWith("gltf"))
                        {
                            try
                            {
                                var deserializedFile = Interface.LoadModel(file);
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
        }

        [Test]
        public void BinaryLoad()
        {
            foreach (var file in Directory.EnumerateFiles(AbsolutePathToSchemaDir, "*.glb", SearchOption.AllDirectories))
            {
                try
                {
                    var len = new FileInfo(file).Length;

                    Assert.IsTrue((len & 3) == 0);

                    var jsonChunk = Interface.LoadModel(file);
                    Assert.IsNotNull(jsonChunk);

                    var binChunk = Interface.LoadBinaryBuffer(file);
                    Assert.IsNotNull(binChunk);
                    Assert.IsTrue((binChunk.Length & 3) == 0);

                    var buffer = jsonChunk.Buffers[0];
                    Assert.IsTrue(buffer.ByteLength <= binChunk.Length);
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
