using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class AnimationChannelTarget
    {
        // Type: integer
        // The index of the node to animate.When undefined, the animated object MAY be defined by an extension.
        // Required: No
        public int node { get; set; } = -1;

        // Type: string
        // The name of the node’s TRS property to animate, or the "weights" of the Morph Targets it instantiates.For the "translation" property, the values that are provided by the sampler are the translation along the X, Y, and Z axes.For the "rotation" property, the values are a quaternion in the order (x, y, z, w), where w is the scalar. For the "scale" property, the values are the scaling factors along the X, Y, and Z axes.
        // Required: Yes
        public string path { get; set; } = string.Empty;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public List<Extra> extras { get; set; } = new List<Extra>();
    }
}
