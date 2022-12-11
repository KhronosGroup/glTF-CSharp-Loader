using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    internal class BufferView
    {
        /*
         * 
buffer

integer

The index of the buffer.

 Yes

byteOffset

integer

The offset into the buffer in bytes.

No, default: 0

byteLength

integer

The length of the bufferView in bytes.

 Yes

byteStride

integer

The stride, in bytes.

No

target

integer

The hint representing the intended GPU buffer type to use with this buffer view.

No

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
