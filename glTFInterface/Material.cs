using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    internal class Material
    {
        /*
         * 
name

string

The user-defined name of this object.

No

extensions

extension

JSON object with extension-specific objects.

No

extras

extras

Application-specific data.

No

pbrMetallicRoughness

material.pbrMetallicRoughness

A set of parameter values that are used to define the metallic-roughness material model from Physically Based Rendering (PBR) methodology. When undefined, all the default values of pbrMetallicRoughness MUST apply.

No

normalTexture

material.normalTextureInfo

The tangent space normal texture.

No

occlusionTexture

material.occlusionTextureInfo

The occlusion texture.

No

emissiveTexture

textureInfo

The emissive texture.

No

emissiveFactor

number [3]

The factors for the emissive color of the material.

No, default: [0,0,0]

alphaMode

string

The alpha rendering mode of the material.

No, default: "OPAQUE"

alphaCutoff

number

The alpha cutoff value of the material.

No, default: 0.5

doubleSided

boolean

Specifies whether the material is double sided.

No, default: false         * 
         */
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
