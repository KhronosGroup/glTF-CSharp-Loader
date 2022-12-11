using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    internal class Buffer
    {
        /*
         * 
uri

string

The URI (or IRI) of the buffer.

No

byteLength

integer

The length of the buffer in bytes.

 Yes

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

No         * 
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
