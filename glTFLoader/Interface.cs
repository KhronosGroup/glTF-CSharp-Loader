using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using glTFLoader.Schema;
using Newtonsoft.Json;

namespace glTFLoader
{
    public static class Interface
    {
        public static Gltf LoadModel(string filePath)
        {
            var path = Path.GetFullPath(filePath);
            CallContext.LogicalSetData("UriRootPath", Path.GetDirectoryName(path));
            var bytes = File.ReadAllBytes(path);

            // Load a normal gltf model
            if (bytes[0] != 'g' || bytes[1] != 'l' || bytes[2] != 'T' || bytes[3] != 'F')
            {
                return JsonConvert.DeserializeObject<Gltf>(Encoding.UTF8.GetString(bytes));
            }

            var version = BitConverter.ToUInt32(bytes, 4);
            if (version != 2)
            {
                throw new NotImplementedException($"Unknown version number {version}");
            }

            var length = (int)BitConverter.ToUInt32(bytes, 8);
            if (length != bytes.Length)
            {
                throw new InvalidDataException($"The specified length of the file ({length}) is not equal to the actual length of the file ({bytes.Length}).");
            }

            var chunkLength = (int)BitConverter.ToUInt32(bytes, 12);

            if (bytes[16] != 'J' || bytes[17] != 'S' || bytes[18] != 'O' || bytes[19] != 'N')
            {
                throw new NotImplementedException("The first chunk must be format 'JSON'.");
            }

            var model = JsonConvert.DeserializeObject<Gltf>(Encoding.UTF8.GetString(bytes, 20, chunkLength));

            // TODO determine if I need to read the binary chunks
            // Sample in TS: https://github.com/BabylonJS/Babylon.js/blob/master/loaders/src/glTF/babylon.glTFFileLoader.ts#L153

            return model;
        }

        public static string SerializeModel(Gltf model)
        {
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            return json;
        }

        public static void SaveModel(Gltf model, string path)
        {
            File.WriteAllText(path, SerializeModel(model));
        }

    }



}
