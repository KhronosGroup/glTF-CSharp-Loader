using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticObjects
{
    /*
     * 
                material.name = "Transparent Dome";
                material.alphaMode = "BLEND";
                material.doubleSided = true;
                PbrMetallicRoughness pbrMetallicRoughness = new PbrMetallicRoughness();
                pbrMetallicRoughness.roughnessFactor = 0.1;
                pbrMetallicRoughness.metallicFactor = 0.1;
                pbrMetallicRoughness.baseColorFactor = new double[4] { 0.4, 0.4, 0.4, 0.45 };
                pbrMetallicRoughness.roughnessFactor = 0.1;
                pbrMetallicRoughness.metallicFactor = 0.1;
                material.pbrMetallicRoughness = pbrMetallicRoughness;
     * 
     */
    public class Material
    {
        public string Name { get; set; } = string.Empty;
        public string AlphaMode { get; set; } = "OPAQUE";
        public bool DoubleSided { get; set; } = true;
        public PBRMetallicRoughness? PBRMetallicRougness { get; set; } = null;
    }
    public class PBRMetallicRoughness
    {
        public float RoughnessFactor { get; set; } = 0.1f;
        public float MetallicFactor { get; set; } = 0.1f;
        public float[] BaseColorFactor { get; set; } = new float[4] { 0.4f, 0.4f, 0.4f, 0.45f };
    }
}
