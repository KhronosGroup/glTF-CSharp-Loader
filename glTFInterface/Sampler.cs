using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Sampler
    {
        // Type: integer
        // Magnification filter.
        // Required: No; Allowed values: 9728 NEAREST, 9729 LINEAR
        public int magFilter { get; set; } = 9729;

        // Type: integer
        // Minification filter.
        // Required: No; Allowed values:
        //     9728 NEAREST, 9729 LINEAR, 9984 NEAREST_MIPMAP_NEAREST, 9985 LINEAR_MIPMAP_NEAREST, 9986 NEAREST_MIPMAP_LINEAR, 9987 LINEAR_MIPMAP_LINEAR
        public int minFilter { get; set; } = 9728;

        // Type: integer
        // S(U) wrapping mode.
        // Required: No, default: 10497
        // Allowed values: 33071 CLAMP_TO_EDGE, 33648 MIRRORED_REPEAT, 10497 REPEAT
        public int wrapS { get; set; } = 10497;

        // Type: integer
        // T(V) wrapping mode.
        // Required: No, default: 10497
        // Allowed values: 33071 CLAMP_TO_EDGE, 33648 MIRRORED_REPEAT, 10497 REPEAT
        public int wrapT { get; set; } = 10497;

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
        public List<Extra>? extras { get; set; } = null;
    }
}
