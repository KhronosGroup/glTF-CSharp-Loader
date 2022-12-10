using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    internal class Mesh
    {
        public string name { get; set; } = "not set";
        // An array of primitives, each defining geometry to be rendered.
        public MeshPrimitive[] primitives { get; set; } = new MeshPrimitive[0];
        // The material index must fall in the range of material elements in the glTF container
        // the default is set to -1, which is not a valid material index

        // weights, extensions, extras
        // Array of weights to be applied to the morph targets.The number of array elements MUST match the number of morph targets.
        public double[] weights { get; set; } = new double[0];
        public Extension[] extensions { get; set; } = new Extension[0];
        public Extra[] extras { get; set; } = new Extra[0];
    }

    /*
     * 

     * 
     * 
 {
    "mode": 4,
    "indices": 0,
    "attributes":
    {
        "POSITION": 1,
        "NORMAL": 2
    },
    "material": 2
 }
     * 
     */
    internal class MeshPrimitive
    {
        // 0 POINTS
        // 1 LINES
        // 2 LINE_LOOP
        // 3 LINE_STRIP
        // 4 TRIANGLES
        // 5 TRIANGLE_STRIP
        // 6 TRIANGLE_FAN
        // default is 4
        public int mode { get; set; } = 4;
        // The index of the material to apply to this primitive when rendering.
        public int material { get; set; } = -1;
        // A plain JSON object, where each key corresponds to a mesh attribute semantic and each value is the index of the accessor containing attribute’s data.
        public PrimitiveAttribute[] attributes { get; set; } = new PrimitiveAttribute[0];
        // The index of the accessor that contains the vertex indices.
        // The index of the accessor that contains the vertex indices. When this is undefined, the primitive defines non-indexed geometry.
        // When defined, the accessor MUST have SCALAR type and an unsigned integer component type.
        public int indices { get; set; } = -1; // accessor index for vertex indices
        // An array of morph targets
        public MorphTarget[] targets { get; set; } = new MorphTarget[0];    
        public Extension[] extensions { get; set; } = new Extension[0];
        public Extra[] extras { get; set; } = new Extra[0];
    }
    internal class PrimitiveAttribute
    {
        public PrimitiveAttribute(string key, int value)
        {
            this.key = key;
            this.value = value;
        }
        public string key { get; set; } = "";
        public int value { get; set; } = -1;

    }
    internal class MorphTarget
    {
        public MorphTarget(string key, int index)
        {
            this.key = key;
            this.index = index;
        }
        public string key { get; set; } = "";
        public int index { get; set; } = -1;

    }
}
