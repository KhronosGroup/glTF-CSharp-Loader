using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    /*
     * 
magFilter

integer

Magnification filter.

No

minFilter

integer

Minification filter.

No

wrapS

integer

S (U) wrapping mode.

No, default: 10497

wrapT

integer

T (V) wrapping mode.

No, default: 10497

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

No     * 
     */
    internal class Sampler
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
