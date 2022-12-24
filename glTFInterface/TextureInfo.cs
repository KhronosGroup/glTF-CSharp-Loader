using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace glTFInterface
{
    public class TextureInfo
    {
        // Type: integer
        // The index of the texture.
        // Required: Yes
        public int? index { get; set; } = null;

        // Type: integer
        // The set index of texture’s TEXCOORD attribute used for texture coordinate mapping.
        // Required: No, default: 0
            public int? texCoord { get; set; } = null;

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
