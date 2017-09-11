﻿using System;
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

            using (BinaryReader binaryReader = new BinaryReader(stream,Encoding.UTF8,true))
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

                var data =  ReadBinaryJsonChunk(binaryReader);

                return Encoding.UTF8.GetString(data);
            }
        }

        private static byte[] ReadBinaryJsonChunk(BinaryReader binaryReader)
        {
            uint chunkLength = binaryReader.ReadUInt32();
            if ((chunkLength & 3) != 0)
            {
                throw new NotImplementedException($"The first chunk must be padded to 4 bytes: {chunkLength}");
            }

            uint chunkFormat = binaryReader.ReadUInt32();
            if (chunkFormat != JSON)
            {
                throw new NotImplementedException($"The first chunk must be format 'JSON': {chunkFormat}");
            }

            return binaryReader.ReadBytes((int)chunkLength);
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

                // skip JSON chunk
                ReadBinaryJsonChunk(binaryReader);

                uint chunkLength = binaryReader.ReadUInt32();
                if ((chunkLength & 3) != 0)
                {
                    throw new NotImplementedException($"The second chunk must be padded to 4 bytes: {chunkLength}");
                }

                uint chunkFormat = binaryReader.ReadUInt32();
                if (chunkFormat != BIN)
                {
                    throw new NotImplementedException($"The second chunk must be format 'BIN': {chunkFormat}");
                }

                return binaryReader.ReadBytes((int)chunkLength);
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
            if (version != 2)
            {
                throw new NotImplementedException($"Unknown version number: {version}");
            }

            uint length = binaryReader.ReadUInt32();
            long fileLength = binaryReader.BaseStream.Length;
            if (length != fileLength)
            {
                throw new InvalidDataException($"The specified length of the file ({length}) is not equal to the actual length of the file ({fileLength}).");
            }
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
            var jsonText = JsonConvert.SerializeObject(model, Formatting.None);
            var jsonChunk = Encoding.UTF8.GetBytes(jsonText);
            var jsonPadding = jsonChunk.Length & 3; if (jsonPadding != 0) jsonPadding = 4 - jsonPadding;

            var binPadding = buffer.Length & 3; if (binPadding != 0) binPadding = 4 - binPadding;

            int fullLength = 4 + 4 + 4;            

            fullLength += 8 + jsonChunk.Length + jsonPadding;
            fullLength += 8 + buffer.Length + binPadding;

            binaryWriter.Write(GLTF);
            binaryWriter.Write((UInt32)2);
            binaryWriter.Write(fullLength);

            binaryWriter.Write(jsonChunk.Length + jsonPadding);
            binaryWriter.Write(JSON);            
            binaryWriter.Write(jsonChunk);
            for (int i = 0; i < jsonPadding; ++i) binaryWriter.Write((Byte)0);
            
            binaryWriter.Write(buffer.Length + binPadding);
            binaryWriter.Write(BIN);
            binaryWriter.Write(buffer);
            for (int i = 0; i < binPadding; ++i) binaryWriter.Write((Byte)0);
        }

    }



}
