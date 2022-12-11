using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    /*
     * 
uri

string

The URI (or IRI) of the image.

No

mimeType

string

The image’s media type. This field MUST be defined when bufferView is defined.

No

bufferView

integer

The index of the bufferView that contains the image. This field MUST NOT be defined when uri is defined.

No

name

string

The user-defined name of this object.

No


     */
    public class Image
    {
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
