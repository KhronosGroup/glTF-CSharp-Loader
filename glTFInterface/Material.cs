using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Material
    {
        // Type: material.pbrMetallicRoughness
        // A set of parameter values that are used to define the metallic-roughness material model from Physically Based Rendering (PBR) methodology.
        // When undefined, all the default values of pbrMetallicRoughness MUST apply.
        // Required: No
        public PbrMetallicRoughness? pbrMetallicRoughness { get; set; } = null;

        // Type: material.normalTextureInfo
        // The tangent space normal texture.
        // Required: No
        public NormalTextureInfo? normalTexture { get; set; } = null;

        // Type: material.occlusionTextureInfo
        // The occlusion texture.
        // Required: No
        public OcclusionTextureInfo? occlusionTexture { get; set; } = null;

        // Type: textureInfo
        // The emissive texture.
        // Required: No
        public TextureInfo? emmissiveTexture { get; set; } = null;

        // Type: number[3]
        // The factors for the emissive color of the material.
        /// Required: No, default: [0,0,0]
        public double[] emmisveFactor { get; set; } = new double[0];

        // Type: string
        // The alpha rendering mode of the material.
        // Required: No, default: "OPAQUE"
        public string alphaMode { get; set; } = "OPAQUE";

        // Type: number
        // The alpha cutoff value of the material.
        // Required: No, default: 0.5
        public double alphaCutoff { get; set; } = 0.5;

        // Type: boolean
        // Specifies whether the material is double sided.
        // Required: No, default: false
        public bool doubleSided { get; set; } = false;

        // Type: string
        // The user-defined name of this object.
        // Required: No
        public string name { get; set; } = "not set";

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public Extension[] extensions { get; set; } = new Extension[0];
        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[] extras { get; set; } = new Extra[0];
    }

     public class PbrMetallicRoughness
    {
        // Type: number[4]
        // The factors for the base color of the material.
        // Required: No, default: [1,1,1,1]
        public int[] baseColorFactor { get; set; } = new int[0];

        // Type: textureInfo
        // The base color texture.
        // Required: No
        public TextureInfo? baseColorTexture { get; set; } = null;

        // Type: number
        // The factor for the metalness of the material.
        // Required: No, default: 1
        public double metallicFactor { get; set; } = 1.0;

        // Type: number
        // The factor for the roughness of the material.
        // Required: No, default: 1
        public double roughnessFactor { get; set; } = 1.0;

        // Type: textureInfo
        // The metallic-roughness texture.
        // Required: No
        public TextureInfo? metallicRoughnessTexture { get; set; } = null;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public Extension[] extensions { get; set; } = new Extension[0];
        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[] extras { get; set; } = new Extra[0];

    }

    public class NormalTextureInfo
    {
        // Type: integer
        // The index of the texture.
        // Required: Yes
        public int index { get; set; } = -1;

        // Type: integer
        // The set index of texture’s TEXCOORD attribute used for texture coordinate mapping.
        // Required: No, default: 0
        public int texCoord { get; set; } = 0;

        // Type: number
        // The scalar parameter applied to each normal vector of the normal texture.
        // Required: No, default: 1
        public double scale { get; set; } = 1.0;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public Extension[] extensions { get; set; } = new Extension[0];
        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[] extras { get; set; } = new Extra[0];

    }

    public class OcclusionTextureInfo
    {
        // Type: integer
        // The index of the texture.
        // Required: Yes
        public int index { get; set; } = -1;

        // Type: integer
        // The set index of texture’s TEXCOORD attribute used for texture coordinate mapping.
        // Required: No, default: 0
        public int texCoord { get; set; } = 0;

        // Type: number
        // A scalar multiplier controlling the amount of occlusion applied.
        // Required: No, default: 1
        public double strength { get; set; } = 1.0;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public Extension[] extensions { get; set; } = new Extension[0];
        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[] extras { get; set; } = new Extra[0];

    }

}
