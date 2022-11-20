using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using glTFLoader.Schema;
using Newtonsoft.Json;

namespace glTFSerializer
{
    internal class Program
    {
        /// <summary>
        ///  This is a test harness to load a glTF file and then to re-serialize it in order to verify the serialization
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string name = "Box.gltf";
            string fullName = Path.Combine("d:", "git", "glTF-CSharp-Loader", "Box", name);
            Gltf model = glTFLoader.Interface.LoadModel(fullName);
            string json = JsonConvert.SerializeObject(model);

            // create empty model - define tangent point

            // add relief

            // add water

            // add roads

            // add vegetation

            // add furniture

            // add building

            // add mobiles

            //


        }
    }
}