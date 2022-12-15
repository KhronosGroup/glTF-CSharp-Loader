using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class BufferView
    {
        // Type: integer
        // The index of the buffer.
        // Required: Yes
        public int buffer { get; set; } = -1;

        // Type: integer
        // The offset into the buffer in bytes.
        // Required: No, default: 0
        public int byteOffset { get; set; } = -1;

        // Type: integer
        // The length of the bufferView in bytes.
        // Required: Yes
        public int byteLength { get; set; } = 0;

        // Type: integer
        // The stride, in bytes.
        // Required: No
        // The stride, in bytes, between vertex attributes.
        // When this is not defined, data is tightly packed.
        // When two or more accessors use the same buffer view, this field MUST be defined
        // min 4, max 252
        public int byteStride { get; set; } = 0;

        // Type: integer
        // The hint representing the intended GPU buffer type to use with this buffer view.
        // Required: No
        // Allowed values: 34962 ARRAY_BUFFER, 34963 ELEMENT_ARRAY_BUFFER
        public int target { get; set; } = 0;

        // Type: string
        // The user-defined name of this object.
        // Required: No
        public string name { get; set; } = string.Empty;    

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
