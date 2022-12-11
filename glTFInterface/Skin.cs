using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    /*
     * 
inverseBindMatrices

integer

The index of the accessor containing the floating-point 4x4 inverse-bind matrices.

No

skeleton

integer

The index of the node used as a skeleton root.

No

joints

integer [1-*]

Indices of skeleton nodes, used as joints in this skin.

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

No     * 
     */
    public class Skin
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
