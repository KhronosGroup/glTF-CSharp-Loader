using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    /*
     * 




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

        // 

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
