using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Texture
    {
        // Type: integer
        // The index of the sampler used by this texture.
        // When undefined, a sampler with repeat wrapping and auto filtering SHOULD be used.
        // Required: No
        public int sampler { get; set; } = -1;

        // Type: integer
        // The index of the image used by this texture.
        // When undefined, an extension or other mechanism SHOULD supply an alternate texture source, otherwise behavior is undefined.
        // Required: No
        public int source { get; set; } = -1;

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
        public List<Extra> extras { get; set; } = new List<Extra>();
    }
}
