using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Asset
    {
        // Type: string
        // A copyright message suitable for display to credit the content creator.
        // Required: No
        public string copyright { get; set; } = string.Empty;

        // Type: string
        // Tool that generated this glTF model. Useful for debugging.
        // Required: No
        public string generator { get; set; } = string.Empty;

        // Type: string
        // The glTF version in the form of <major>.<minor> that this asset targets.
        // Required: Yes
        public string version { get; set; } = string.Empty;

        // Type: string
        // The minimum glTF version in the form of <major>.<minor> that this asset targets.
        // This property MUST NOT be greater than the asset version.
        // Required: No
        public string minVersion { get; set; } = String.Empty;

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
