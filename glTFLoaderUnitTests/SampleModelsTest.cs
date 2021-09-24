using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using glTFLoader;
using NUnit.Framework;

namespace glTFLoaderUnitTests
{
    [TestFixture]
    public class SampleModelsTest
    {
        private const string RelativePathToSchemaDir = @"..\..\..\..\..\glTF-Sample-Models\2.0\";
        private string AbsolutePathToSchemaDir;

        [SetUp]
        public void Init()
        {
            AbsolutePathToSchemaDir = Path.Combine(TestContext.CurrentContext.TestDirectory, RelativePathToSchemaDir);
        }

        private IEnumerable<String> GetTestFiles(string subdir)
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(AbsolutePathToSchemaDir)))
            {
                var xdir = Path.Combine(dir, subdir);

                if (!Directory.Exists(xdir)) continue;

                foreach (var file in Directory.EnumerateFiles(xdir))
                {
                    if (!file.EndsWith("gltf") && !file.EndsWith("glb")) continue;

                    yield return file;
                }
            }
        }

        private static glTFLoader.Schema.Gltf TestLoadFile(string filePath)
        {
            if (!filePath.EndsWith("gltf") && !filePath.EndsWith("glb")) return null;

            try
            {
                var deserializedFile = Interface.LoadModel(filePath);
                Assert.IsNotNull(deserializedFile);

                // read all buffers
                for (int i = 0; i < deserializedFile.Buffers?.Length; ++i)
                {
                    var expectedLength = deserializedFile.Buffers[i].ByteLength;

                    var bufferBytes = deserializedFile.LoadBinaryBuffer(i, filePath);
                    Assert.IsNotNull(bufferBytes);
                }

                // open all images
                for (int i = 0; i < deserializedFile.Images?.Length; ++i)
                {
                    using (var s = deserializedFile.OpenImageFile(i, filePath))
                    {
                        Assert.IsNotNull(s);

                        using (var rb = new BinaryReader(s))
                        {
                            uint header = rb.ReadUInt32();

                            if (header == 0x474e5089) continue; // PNG
                            if ((header & 0xffff) == 0xd8ff) continue; // JPEG                            

                            Assert.Fail($"Invalid image in Image index {i}");
                        }
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
        [TestCase("glTF")]
        [TestCase("glTF-Binary")]
        [TestCase("glTF-Embedded")]
        public void SchemaLoad(string subdirectory)
        {
            foreach (var file in GetTestFiles(subdirectory))
            {
                var gltf = TestLoadFile(file);
                
                // serialization as glTF (in memory)
                using (var wm = new MemoryStream())
                {
                    Interface.SaveModel(gltf, wm);

                    using (var rm = new MemoryStream(wm.ToArray()))
                    {
                        gltf = Interface.LoadModel(rm);
                        Assert.IsNotNull(gltf);
                    }
                }

                // serialization as GLB if compatible (in memory)
                if (gltf.Buffers?.Length == 1 && gltf.Buffers[0].Uri == null)
                {
                    using (var wm = new MemoryStream())
                    {
                        var binChunk = gltf.LoadBinaryBuffer(0, file);

                        gltf.SaveBinaryModel(binChunk, wm);

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
            }
        }

        [Test]
        [TestCase("glTF")]
        public void PackBinary(string subdirectory)
        {
            foreach (var file in GetTestFiles(subdirectory))
            {
                string tempOutputFile = Path.GetTempFileName();
                string glbOutputFile = Path.ChangeExtension(tempOutputFile, "glb");

                // pack file
                Interface.Pack(file, glbOutputFile);
                TestLoadFile(glbOutputFile);

                // pack model test
                var gltf = TestLoadFile(file);

                // pack model
                Interface.SaveBinaryModelPacked(gltf, glbOutputFile, file);
                TestLoadFile(glbOutputFile);

                File.Delete(tempOutputFile);
                File.Delete(glbOutputFile);

            }
        }

        [Test]
        [TestCase("glTF-Binary")]
        public void UnpackBinary(string subdirectory)
        {
            string gltfUnpackDir = Path.Combine(Path.GetTempPath(), "glTF-unpacked");
            Directory.CreateDirectory(gltfUnpackDir);

            foreach (var file in GetTestFiles(subdirectory))
            {
                var gltf = TestLoadFile(file);

                string modelName = Path.GetFileName(file);
                string gltfUnpackDirModel = Path.Combine(gltfUnpackDir, modelName);
                Directory.CreateDirectory(gltfUnpackDirModel);

                Interface.Unpack(file, gltfUnpackDirModel);

                TestLoadFile(Path.Combine(gltfUnpackDirModel, Path.ChangeExtension(modelName, "gltf")));


            }
            Directory.Delete(gltfUnpackDir, true);
        }


    }
}
