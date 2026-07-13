using glTFLoader;

using System;
using System.Collections.Generic;
using System.IO;

using Xunit;

namespace glTFLoaderUnitTests
{
    public class SampleModelsTest
    {
        private const string RelativePathToSchemaDir = @"..\..\..\..\..\glTF-Sample-Assets\Models\";
        private string AbsolutePathToSchemaDir;

        // Models that require glTF extensions this loader does not yet support, so they cannot
        // be loaded. Tracked by https://github.com/KhronosGroup/glTF-CSharp-Loader/issues/60.
        private static readonly HashSet<string> UnsupportedModels = new HashSet<string>
        {
            "AnimatedColorsCube",         // KHR_animation_pointer
            "AnimationPointerUVs",        // KHR_animation_pointer
            "CubeVisibility",             // KHR_animation_pointer
            "LightVisibility",            // KHR_animation_pointer
            "PotOfCoalsAnimationPointer", // KHR_animation_pointer
            "SheenWoodLeatherSofa",       // EXT_texture_webp
        };

        public SampleModelsTest()
        {
            AbsolutePathToSchemaDir = Path.Combine(Directory.GetCurrentDirectory(), RelativePathToSchemaDir);
        }

        private IEnumerable<String> GetTestFiles(string subdir)
        {
            foreach (var dir in Directory.EnumerateDirectories(Path.GetFullPath(AbsolutePathToSchemaDir)))
            {
                if (UnsupportedModels.Contains(Path.GetFileName(dir))) continue;

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
                Assert.NotNull(deserializedFile);

                // read all buffers
                for (int i = 0; i < deserializedFile.Buffers?.Length; ++i)
                {
                    var expectedLength = deserializedFile.Buffers[i].ByteLength;

                    var bufferBytes = deserializedFile.LoadBinaryBuffer(i, filePath);
                    Assert.NotNull(bufferBytes);
                }

                // open all images
                for (int i = 0; i < deserializedFile.Images?.Length; ++i)
                {
                    using (var s = deserializedFile.OpenImageFile(i, filePath))
                    {
                        Assert.NotNull(s);

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



        [Theory]
        [InlineData("glTF")]
        [InlineData("glTF-Binary")]
        [InlineData("glTF-Embedded")]
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
                        Assert.NotNull(gltf);
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

        [Theory]
        [InlineData("glTF")]
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

        [Theory]
        [InlineData("glTF-Binary")]
        public void UnpackBinary(string subdirectory)
        {
            // Use a unique directory so concurrent test runs (e.g. the net462 and net8.0
            // target frameworks running in parallel) do not share and delete each other's output.
            string gltfUnpackDir = Path.Combine(Path.GetTempPath(), "glTF-unpacked-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(gltfUnpackDir);

            try
            {
                foreach (var file in GetTestFiles(subdirectory))
                {
                    var gltf = TestLoadFile(file);

                    string modelName = Path.GetFileName(file);
                    string gltfUnpackDirModel = Path.Combine(gltfUnpackDir, modelName);
                    Directory.CreateDirectory(gltfUnpackDirModel);

                    Interface.Unpack(file, gltfUnpackDirModel);

                    TestLoadFile(Path.Combine(gltfUnpackDirModel, Path.ChangeExtension(modelName, "gltf")));
                }
            }
            finally
            {
                Directory.Delete(gltfUnpackDir, true);
            }
        }


    }
}
