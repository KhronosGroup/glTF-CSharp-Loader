using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    /*
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
    public class MeshPrimitive
    {
        // Type: integer
        // The topology type of primitives to render.
        // Required: No, default: 4
        // 0 POINTS
        // 1 LINES
        // 2 LINE_LOOP
        // 3 LINE_STRIP
        // 4 TRIANGLES
        // 5 TRIANGLE_STRIP
        // 6 TRIANGLE_FAN
        // default is 4
        public int mode { get; set; } = 4;

        // Type: integer
        // The index of the material to apply to this primitive when rendering.
        // Required: No
        public int material { get; set; } = -1;

        // Type: object
        // A plain JSON object, where each key corresponds to a mesh attribute semantic and each value is the index of the accessor containing attribute’s data.
        // Required: Yes
        public Dictionary<string, int> attributes { get; set; } = new Dictionary<string, int>();

        // The index of the accessor that contains the vertex indices.
        // When this is undefined, the primitive defines non-indexed geometry.
        // When defined, the accessor MUST have SCALAR type and an unsigned integer component type.
        // Type: integer
        // The index of the accessor that contains the vertex indices.
        // Required: No
        public int? indices { get; set; } = null; // accessor index for vertex indices

        // An array of morph targets
        // Type: object[1 - *]
        // An array of morph targets.
        // Required: No
        public MorphTarget[]? targets { get; set; } = null;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public Extension[]? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[]? extras { get; set; } = null;
    }
    public class PrimitiveAttribute
    {
        public PrimitiveAttribute(string key, int value)
        {
            this.key = key;
            this.value = value;
        }
        public string key { get; set; } = "";
        public int value { get; set; } = -1;

    }
    public class MorphTarget
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
