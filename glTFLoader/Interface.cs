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
                long fileLength = stream.Length;
                if (length != fileLength)
                {
                    throw new InvalidDataException($"The specified length of the file ({length}) is not equal to the actual length of the file ({fileLength}).");
                }

                uint chunkLength = binaryReader.ReadUInt32();
                uint chunkFormat = binaryReader.ReadUInt32();
                if (chunkFormat != JSON)
                {
                    throw new NotImplementedException($"The first chunk must be format 'JSON': {chunkFormat}");
                }

                return Encoding.UTF8.GetString(binaryReader.ReadBytes((int)chunkLength));
            }
        }

        public static Gltf DeserializeModel(string fileData)
        {
            return JsonConvert.DeserializeObject<Gltf>(fileData);
        }

        public static string SerializeModel(Gltf model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }

        public static void SaveModel(Gltf model, string path)
        {
            using (Stream stream = File.Create(path))
            {
                SaveModel(model, stream);
            }
        }

        public static void SaveModel(Gltf model, Stream stream)
        {
            string fileData = SerializeModel(model);

            using (var ts = new StreamWriter(stream))
            {
                ts.Write(fileData);
            }
        }

    }



}
