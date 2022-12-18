using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Mesh
    {
        // mesh.primitive[1 - *]
        // An array of primitives, each defining geometry to be rendered.
        // Required: Yes
        public MeshPrimitive[] primitives { get; set; } = new MeshPrimitive[0];

        // Type: number[1 - *]
        // Array of weights to be applied to the morph targets.The number of array elements MUST match the number of morph targets.
        // Required: No
        public double[]? weights { get; set; } = null;

        // Type: string
        // The user-defined name of this object.
        // Required: No
        public string name { get; set; } = "not set";

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[]? extras { get; set; } = null;
    }

 

}
