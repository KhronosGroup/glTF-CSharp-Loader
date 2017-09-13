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
        const uint GLTFHEADER = 0x46546C67;
        const uint GLTFVERSION2 = 2;
        const uint CHUNKJSON = 0x4E4F534A;
        const uint CHUNKBIN = 0x004E4942;        

        const string EMBEDDEDOCTETSTREAM = "data:application/octet-stream;base64,";
        const string EMBEDDEDPNG = "data:image/png;base64,";        
        const string EMBEDDEDJPEG = "data:image/jpeg;base64,";

        /// <summary>
        /// Loads a <code>Schema.Gltf</code> model from a file
        /// </summary>
        /// <param name="filePath">Source file path to a gltf/glb model</param>
        /// <returns><code>Schema.Gltf</code> model</returns>
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

        /// <summary>
        /// Reads a <code>Schema.Gltf</code> model from a stream
        /// </summary>
        /// <param name="stream">Readable stream to a gltf/glb model</param>
        /// <returns><code>Schema.Gltf</code> model</returns>
        public static Gltf LoadModel(Stream stream)
        {
            bool binaryFile = false;

            using (BinaryReader binaryReader = new BinaryReader(stream,Encoding.ASCII,true))
            {
                uint magic = binaryReader.ReadUInt32();
                if (magic == GLTFHEADER)
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

                var data = ReadBinaryChunk(binaryReader, CHUNKJSON);

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

        /// <summary>
        /// Loads the binary buffer chunk of a glb file
        /// </summary>
        /// <param name="filePath">Source file path to a glb model</param>
        /// <returns>Byte array of the buffer</returns>
        public static Byte[] LoadBinaryBuffer(string filePath)
        {
            using (Stream stream = File.OpenRead(filePath))
            {
                return LoadBinaryBuffer(stream);
            }
        }

        /// <summary>
        /// Reads the binary buffer chunk of a glb stream
        /// </summary>
        /// <param name="stream">Readable stream to a glb model</param>
        /// <returns>Byte array of the buffer</returns>
        public static Byte[] LoadBinaryBuffer(Stream stream)
        {
            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                ReadBinaryHeader(binaryReader);

                return ReadBinaryChunk(binaryReader, CHUNKBIN);
            }
        }

        private static void ReadBinaryHeader(BinaryReader binaryReader)
        {
            uint magic = binaryReader.ReadUInt32();
            if (magic != GLTFHEADER)
            {
                throw new InvalidDataException($"Unexpected magic number: {magic}");
            }

            uint version = binaryReader.ReadUInt32();
            if (version != GLTFVERSION2)
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

        /// <summary>
        /// Creates an External File Solver for a given gltf file path, so we can resolve references to associated files
        /// </summary>
        /// <param name="gltfFilePath">ource file path to a gltf/glb model</param>
        /// <returns>Lambda funcion to resolve dependencies</returns>
        private static Func<string,Byte[]> GetExternalFileSolver(string gltfFilePath)
        {
            return asset =>
            {
                if (string.IsNullOrEmpty(asset)) return LoadBinaryBuffer(gltfFilePath);
                var bufferFilePath = Path.Combine(Path.GetDirectoryName(gltfFilePath), asset);
                return File.ReadAllBytes(bufferFilePath);
            };
        }

        /// <summary>
        /// Gets a binary buffer referenced by a specific <code>Schema.Buffer</code>
        /// </summary>
        /// <param name="model">The <code>Schema.Gltf</code> model containing the <code>Schema.Buffer</code></param>
        /// <param name="bufferIndex">The index of the buffer</param>
        /// <param name="gltfFilePath">Source file path used to load the model</param>
        /// <returns>Byte array of the buffer</returns>
        public static Byte[] LoadBinaryBuffer(this Gltf model, int bufferIndex, string gltfFilePath)
        {
            return LoadBinaryBuffer(model, bufferIndex, GetExternalFileSolver(gltfFilePath));            
        }

        /// <summary>
        /// Opens a stream to the image referenced by a specific <code>Schema.Image</code>
        /// </summary>
        /// <param name="model">The <code>Schema.Gltf</code> model containing the <code>Schema.Buffer</code></param>
        /// <param name="imageIndex">The index of the image</param>
        /// <param name="gltfFilePath">Source file path used to load the model</param>
        /// <returns>An open stream to the image</returns>
        public static Stream OpenImageFile(this Gltf model, int imageIndex, string gltfFilePath)
        {
            return OpenImageFile(model, imageIndex, GetExternalFileSolver(gltfFilePath));
        }

        /// <summary>
        /// Gets a binary buffer referenced by a specific <code>Schema.Buffer</code>
        /// </summary>
        /// <param name="model">The <code>Schema.Gltf</code> model containing the <code>Schema.Buffer</code></param>
        /// <param name="bufferIndex">The index of the buffer</param>
        /// <param name="externalReferenceSolver">An user provided lambda function to resolve external assets</param>
        /// <returns>Byte array of the buffer</returns>
        /// <remarks>
        /// Binary buffers can be stored in three different ways:
        /// - As stand alone files.
        /// - As a binary chunk within a glb file.
        /// - Encoded to Base64 within the JSON.        
        /// 
        /// The external reference solver funcion is called when the buffer is stored in an external file,
        /// or when the buffer is in the glb binary chunk, in which case, the Argument of the function will be Null.
        /// 
        /// The Lambda function must return the byte array of the requested file or buffer.
        /// </remarks>        
        public static Byte[] LoadBinaryBuffer(this Gltf model, int bufferIndex, Func<string, Byte[]> externalReferenceSolver)
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

        /// <summary>
        /// Opens a stream to the image referenced by a specific <code>Schema.Image</code>
        /// </summary>
        /// <param name="model">The <code>Schema.Gltf</code> model containing the <code>Schema.Image</code></param>
        /// <param name="imageIndex">The index of the image</param>
        /// <param name="externalReferenceSolver">An user provided lambda function to resolve external assets</param>
        /// <returns>An open stream to the image</returns>
        /// <remarks>
        /// Images can be stored in three different ways:
        /// - As stand alone files.
        /// - As a part of binary buffer accessed via bufferView.
        /// - Encoded to Base64 within the JSON.
        /// 
        /// The external reference solver funcion is called when the image is stored in an external file,
        /// or when the image is in the glb binary chunk, in which case, the Argument of the function will be Null.
        /// 
        /// The Lambda function must return the byte array of the requested file or buffer.
        /// </remarks>
        public static Stream OpenImageFile(this Gltf model, int imageIndex, Func<string, Byte[]> externalReferenceSolver)
        {
            var image = model.Images[imageIndex];

            if (image.BufferView.HasValue)
            {
                var bufferView = model.BufferViews[image.BufferView.Value];

                var bufferBytes = model.LoadBinaryBuffer(bufferView.Buffer, externalReferenceSolver);

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
            if (image.Uri.StartsWith(EMBEDDEDJPEG)) content = image.Uri.Substring(EMBEDDEDJPEG.Length);            

            var bytes = Convert.FromBase64String(content);
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Parses a JSON formatted text content
        /// </summary>
        /// <param name="fileData">JSON text content</param>
        /// <returns><code>Schema.Gltf</code> model</returns>
        public static Gltf DeserializeModel(string fileData)
        {
            return JsonConvert.DeserializeObject<Gltf>(fileData);
        }

        /// <summary>
        /// Serializes a <code>Schema.Gltf</code> model to text
        /// </summary>
        /// <param name="model"><code>Schema.Gltf</code> model</param>
        /// <returns>JSON formatted text</returns>
        public static string SerializeModel(this Gltf model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }

        /// <summary>
        /// Saves a <code>Schema.Gltf</code> model to a gltf file
        /// </summary>
        /// <param name="model"><code>Schema.Gltf</code> model</param>
        /// <param name="path">Destination file path</param>
        public static void SaveModel(this Gltf model, string path)
        {
            using (Stream stream = File.Create(path))
            {
                SaveModel(model, stream);
            }
        }

        /// <summary>
        /// Writes a <code>Schema.Gltf</code> model to a writable stream
        /// </summary>
        /// <param name="model"><code>Schema.Gltf</code> model</param>
        /// <param name="stream">Writable stream</param>
        public static void SaveModel(this Gltf model, Stream stream)
        {
            string fileData = SerializeModel(model);

            using (var ts = new StreamWriter(stream))
            {
                ts.Write(fileData);
            }
        }

        /// <summary>
        /// Saves a <code>Schema.Gltf</code> model to a glb file
        /// </summary>
        /// <param name="model"><code>Schema.Gltf</code> model</param>
        /// <param name="buffer">Binary buffer to embed in the file, or null</param>
        /// <param name="filePath">Destination file path</param>
        public static void SaveBinaryModel(this Gltf model, byte[] buffer, string filePath)
        {
            using (Stream stream = File.Create(filePath))
            {
                SaveBinaryModel(model, buffer, stream);
            }
        }

        /// <summary>
        /// Writes a <code>Schema.Gltf</code> model to a writable stream
        /// </summary>
        /// <param name="model"><code>Schema.Gltf</code> model</param>
        /// <param name="buffer">Binary buffer to embed in the file, or null</param>
        /// <param name="stream">Writable stream</param>
        public static void SaveBinaryModel(this Gltf model, byte[] buffer, Stream stream)
        {
            using (var wb = new BinaryWriter(stream))
            {
                SaveBinaryModel(model, buffer, wb);
            }
        }

        /// <summary>
        /// Writes a <code>Schema.Gltf</code> model to a writable binary writer
        /// </summary>
        /// <param name="model"><code>Schema.Gltf</code> model</param>
        /// <param name="buffer">Binary buffer to embed in the file, or null</param>
        /// <param name="binaryWriter">Binary Writer</param>
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

            binaryWriter.Write(GLTFHEADER);
            binaryWriter.Write(GLTFVERSION2);
            binaryWriter.Write(fullLength);

            binaryWriter.Write(jsonChunk.Length + jsonPadding);
            binaryWriter.Write(CHUNKJSON);            
            binaryWriter.Write(jsonChunk);
            for (int i = 0; i < jsonPadding; ++i) binaryWriter.Write((Byte)0x20);

            if (buffer != null)
            {
                binaryWriter.Write(buffer.Length + binPadding);
                binaryWriter.Write(CHUNKBIN);
                binaryWriter.Write(buffer);
                for (int i = 0; i < binPadding; ++i) binaryWriter.Write((Byte)0);
            }
        }        
            
    }



}
