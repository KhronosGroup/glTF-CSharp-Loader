using System;
using System.IO;

#if NET462
using System.Runtime.Remoting.Messaging;
#endif

using System.Text;
using glTFLoader.Schema;
using Newtonsoft.Json;

namespace glTFLoader
{
    public static class Interface
    {
        const uint GLTF = 0x46546C67;
        const uint JSON = 0x4E4F534A;
        const uint BIN = 0x004E4942;
        const uint VERSION2 = 2;

        const string EMBEDDEDOCTETSTREAM = "data:application/octet-stream;base64,";
        const string EMBEDDEDPNG = "data:image/png;base64,";
        const string EMBEDDEDBMP = "data:image/bmp;base64,";
        const string EMBEDDEDGIF = "data:image/gif;base64,";
        const string EMBEDDEDJPEG = "data:image/jpeg;base64,";
        const string EMBEDDEDTIFF = "data:image/tiff;base64,";

        public static Gltf LoadModel(string filePath)
        {
            var path = Path.GetFullPath(filePath);

#if NET462
            CallContext.LogicalSetData("UriRootPath", Path.GetDirectoryName(path));
#endif

            using (Stream stream = File.OpenRead(filePath))
            {
                return LoadModel(stream);
            }
        }

        public static Gltf LoadModel(Stream stream)
        {
            bool binaryFile = false;

            using (BinaryReader binaryReader = new BinaryReader(stream,Encoding.ASCII,true))
            {
                uint magic = binaryReader.ReadUInt32();
                if (magic == GLTF)
                {
                    binaryFile = true;
                }
            }
            
            stream.Position = 0; // restart read position
            
            string fileData;
            if (binaryFile)
            {
                fileData = ParseBinary(stream);
            }
            else
            {
                fileData = ParseText(stream);
            }

            return JsonConvert.DeserializeObject<Gltf>(fileData);            
        }

        private static string ParseText(Stream stream)
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }

        private static string ParseBinary(Stream stream)
        {
            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                ReadBinaryHeader(binaryReader);

                var data = ReadBinaryChunk(binaryReader, JSON);

                return Encoding.UTF8.GetString(data);
            }
        }

        private static byte[] ReadBinaryChunk(BinaryReader binaryReader, uint format)
        {
            while (true) // keep reading until EndOfFile exception
            {
                uint chunkLength = binaryReader.ReadUInt32();
                if ((chunkLength & 3) != 0)
                {
                    throw new InvalidDataException($"The chunk must be padded to 4 bytes: {chunkLength}");
                }

                uint chunkFormat = binaryReader.ReadUInt32();

                var data = binaryReader.ReadBytes((int)chunkLength);

                if (chunkFormat == format) return data;                
            }            
        }        

        public static Byte[] LoadBinaryBuffer(string filePath)
        {
            using (Stream stream = File.OpenRead(filePath))
            {
                return LoadBinaryBuffer(stream);
            }
        }

        public static Byte[] LoadBinaryBuffer(Stream stream)
        {
            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                ReadBinaryHeader(binaryReader);

                return ReadBinaryChunk(binaryReader, BIN);
            }
        }

        private static void ReadBinaryHeader(BinaryReader binaryReader)
        {
            uint magic = binaryReader.ReadUInt32();
            if (magic != GLTF)
            {
                throw new InvalidDataException($"Unexpected magic number: {magic}");
            }

            uint version = binaryReader.ReadUInt32();
            if (version != VERSION2)
            {
                throw new InvalidDataException($"Unknown version number: {version}");
            }

            uint length = binaryReader.ReadUInt32();
            long fileLength = binaryReader.BaseStream.Length;
            if (length != fileLength)
            {
                throw new InvalidDataException($"The specified length of the file ({length}) is not equal to the actual length of the file ({fileLength}).");
            }
        }

        private static Func<string,Byte[]> GetExternalFileSolver(string gltfFilePath)
        {
            return asset =>
            {
                if (asset == null) return LoadBinaryBuffer(gltfFilePath);
                var bufferFilePath = Path.Combine(Path.GetDirectoryName(gltfFilePath), asset);
                return File.ReadAllBytes(bufferFilePath);
            };
        }

        public static Byte[] LoadBinaryBuffer(this Gltf model, string gltfFilePath, int bufferIndex)
        {
            return LoadBinaryBuffer(model, GetExternalFileSolver(gltfFilePath), bufferIndex);            
        }

        public static Stream OpenImageFile(this Gltf model, string gltfFilePath, int imageIndex)
        {
            return OpenImageFile(model, GetExternalFileSolver(gltfFilePath), imageIndex);
        }

        public static Byte[] LoadBinaryBuffer(this Gltf model, Func<string,Byte[]> externalReferenceSolver, int bufferIndex)
        {
            var buffer = model.Buffers[bufferIndex];

            if (buffer.Uri == null) return externalReferenceSolver(null);

            if (buffer.Uri.StartsWith(EMBEDDEDOCTETSTREAM))
            {
                var content = buffer.Uri.Substring(EMBEDDEDOCTETSTREAM.Length);
                return Convert.FromBase64String(content);
            }

            return externalReferenceSolver(buffer.Uri);
        }

        public static Stream OpenImageFile(this Gltf model, Func<string, Byte[]> externalReferenceSolver, int imageIndex)
        {
            var image = model.Images[imageIndex];

            if (image.BufferView.HasValue)
            {
                var bufferView = model.BufferViews[image.BufferView.Value];

                var bufferBytes = model.LoadBinaryBuffer(externalReferenceSolver, bufferView.Buffer);

                return new MemoryStream(bufferBytes, bufferView.ByteOffset, bufferView.ByteLength);
            }

            if (image.Uri.StartsWith("data:image/")) return OpenEmbeddedImage(image);

            var imageData = externalReferenceSolver(image.Uri);

            return new MemoryStream(imageData);
        }

        private static Stream OpenEmbeddedImage(Image image)
        {
            string content = null;

            if (image.Uri.StartsWith(EMBEDDEDPNG)) content = image.Uri.Substring(EMBEDDEDPNG.Length);
            if (image.Uri.StartsWith(EMBEDDEDBMP)) content = image.Uri.Substring(EMBEDDEDBMP.Length);
            if (image.Uri.StartsWith(EMBEDDEDGIF)) content = image.Uri.Substring(EMBEDDEDGIF.Length);
            if (image.Uri.StartsWith(EMBEDDEDJPEG)) content = image.Uri.Substring(EMBEDDEDJPEG.Length);
            if (image.Uri.StartsWith(EMBEDDEDTIFF)) content = image.Uri.Substring(EMBEDDEDTIFF.Length);

            var bytes = Convert.FromBase64String(content);
            return new MemoryStream(bytes);
        }

        public static Gltf DeserializeModel(string fileData)
        {
            return JsonConvert.DeserializeObject<Gltf>(fileData);
        }

        public static string SerializeModel(this Gltf model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }        

        public static void SaveModel(this Gltf model, string path)
        {
            using (Stream stream = File.Create(path))
            {
                SaveModel(model, stream);
            }
        }

        public static void SaveModel(this Gltf model, Stream stream)
        {
            string fileData = SerializeModel(model);

            using (var ts = new StreamWriter(stream))
            {
                ts.Write(fileData);
            }
        }

        public static void SaveBinaryModel(this Gltf model, byte[] buffer, string filePath)
        {
            using (Stream stream = File.Create(filePath))
            {
                SaveBinaryModel(model, buffer, stream);
            }
        }

        public static void SaveBinaryModel(this Gltf model, byte[] buffer, Stream stream)
        {
            using (var wb = new BinaryWriter(stream))
            {
                SaveBinaryModel(model, buffer, wb);
            }
        }

        public static void SaveBinaryModel(this Gltf model, byte[] buffer, BinaryWriter binaryWriter)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            var jsonText = JsonConvert.SerializeObject(model, Formatting.None);
            var jsonChunk = Encoding.UTF8.GetBytes(jsonText);
            var jsonPadding = jsonChunk.Length & 3; if (jsonPadding != 0) jsonPadding = 4 - jsonPadding;

            if (buffer != null && buffer.Length == 0) buffer = null;
            var binPadding = buffer == null ? 0 : buffer.Length & 3; if (binPadding != 0) binPadding = 4 - binPadding;

            int fullLength = 4 + 4 + 4;            

            fullLength += 8 + jsonChunk.Length + jsonPadding;
            if (buffer != null) fullLength += 8 + buffer.Length + binPadding;

            binaryWriter.Write(GLTF);
            binaryWriter.Write(VERSION2);
            binaryWriter.Write(fullLength);

            binaryWriter.Write(jsonChunk.Length + jsonPadding);
            binaryWriter.Write(JSON);            
            binaryWriter.Write(jsonChunk);
            for (int i = 0; i < jsonPadding; ++i) binaryWriter.Write((Byte)0x20);

            if (buffer != null)
            {
                binaryWriter.Write(buffer.Length + binPadding);
                binaryWriter.Write(BIN);
                binaryWriter.Write(buffer);
                for (int i = 0; i < binPadding; ++i) binaryWriter.Write((Byte)0);
            }
        }        
            
    }



}
