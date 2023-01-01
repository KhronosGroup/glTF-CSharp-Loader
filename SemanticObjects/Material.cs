using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
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
        public PBRMetallicRoughness? PBRMetallicRoughness { get; set; } = null;
    }
    public class PBRMetallicRoughness
    {
        public float RoughnessFactor { get; set; } = 0.1f;
        public float MetallicFactor { get; set; } = 0.1f;
        public float[] BaseColorFactor { get; set; } = new float[4] { 0.4f, 0.4f, 0.4f, 0.45f };
    }
    public class GenericMaterial : Material
    {
        public GenericMaterial()
        {
            Name = "generic";
            AlphaMode = "OPAQUE";
            DoubleSided = true;
            PBRMetallicRoughness = new PBRMetallicRoughness();
            PBRMetallicRoughness.RoughnessFactor = 0.1f;
            PBRMetallicRoughness.MetallicFactor = 0.1f;
            PBRMetallicRoughness.BaseColorFactor = new float[4] { 0.4f, 0.4f, 0.4f, 0.45f };
        }
    }
    public class BoundingSphereMaterial : Material
    {
        public BoundingSphereMaterial()
        {
            Name = "generic";
            AlphaMode = "BLEND";
            DoubleSided = true;
            PBRMetallicRoughness = new PBRMetallicRoughness();
            PBRMetallicRoughness.RoughnessFactor = 0.1f;
            PBRMetallicRoughness.MetallicFactor = 0.1f;
            PBRMetallicRoughness.BaseColorFactor = new float[4] { 0.4f, 0.4f, 0.4f, 0.45f };
        }
    }
    public class TerrainMaterial : Material
    {
        public TerrainMaterial()
        {
            Name = "generic";
            AlphaMode = "OPAQUE";
            DoubleSided = true;
            PBRMetallicRoughness = new PBRMetallicRoughness();
            PBRMetallicRoughness.RoughnessFactor = 0.1f;
            PBRMetallicRoughness.MetallicFactor = 0.1f;
            PBRMetallicRoughness.BaseColorFactor = new float[4] { 0.4f, 0.4f, 0.0f, 1.0f };
        }
    }
}
