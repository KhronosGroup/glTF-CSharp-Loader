using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Accessor
    {
        // The index of the buffer view.
        // When undefined, the accessor MUST be initialized with zeros;
        // sparse property or extensions MAY override zeros with actual values.
        // Type: integer; Required: No; Minimum: >= 0
        public int bufferView { get; set; } = -1;
        // he offset relative to the start of the buffer view in bytes.
        // This MUST be a multiple of the size of the component datatype.
        // This property MUST NOT be defined when bufferView is undefined.
        // Type: integer;  Required: No, default: 0; Minimum: >= 0
        public int? byteOffset { get; set; } = null;
        // The datatype of the accessor’s components.
        // UNSIGNED_INT type MUST NOT be used for any accessor that is not referenced by mesh.primitive.indices.
        // Type: integer; Required:  Yes;
        // Allowed values:
        // 5120 BYTE
        // 5121 UNSIGNED_BYTE
        // 5122 SHORT
        // 5123 UNSIGNED_SHORT
        // 5125 UNSIGNED_INT
        // 5126 FLOAT
        public int componentType { get; set; } = 0;
        // Specifies whether integer data values are normalized (true) to [0, 1] (for unsigned types)
        // or to [-1, 1] (for signed types) when they are accessed.
        // This property MUST NOT be set to true for accessors with FLOAT or UNSIGNED_INT component type.
        // Type: boolean; Required: No, default: false
        public bool normalized { get; set; } = false;
        // The number of elements referenced by this accessor, not to be confused with the number of bytes or number of components.
        // Type: integer; Required:  Yes; Minimum: >= 1
        public int count { get; set; } = 0;
        // Specifies if the accessor’s elements are scalars, vectors, or matrices.
        // Type: string; Required:  Yes
        // Allowed values:
            // "SCALAR"
            // "VEC2"
            // "VEC3"
            // "VEC4"
            // "MAT2"
            // "MAT3"
            // "MAT4"
        public string type { get; set; } = string.Empty;
        // Maximum value of each component in this accessor.
        // Array elements MUST be treated as having the same data type as accessor’s componentType.
        // Both min and max arrays have the same length.
        // The length is determined by the value of the type property; it can be 1, 2, 3, 4, 9, or 16.
        // normalized property has no effect on array values: they always correspond to the actual values stored in the buffer.
        // When the accessor is sparse, this property MUST contain maximum values of accessor data with sparse substitution applied.
        // Type: number[1 - 16]; Required: No
        public List<double>? max { get; set; } = null;
        // Minimum value of each component in this accessor.
        // Array elements MUST be treated as having the same data type as accessor’s componentType.
        // Both min and max arrays have the same length.
        // The length is determined by the value of the type property; it can be 1, 2, 3, 4, 9, or 16.
        // normalized property has no effect on array values: they always correspond to the actual values stored in the buffer.
        // When the accessor is sparse, this property MUST contain minimum values of accessor data with sparse substitution applied.
        // Type: number[1 - 16]; Required: No
        public List<double>? min { get; set; } = null;
        // The user-defined name of this object. This is not necessarily unique, e.g., an accessor and a buffer could have the same name, or two accessors could even have the same name.
        // Type: string; Required: No
        public string name { get; set; } = "not set";
        // Sparse storage of elements that deviate from their initialization value.
        // Type: accessor.sparse; Required: No
        public SparseAccessor? sparse { get; set; } = null;
        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        public List<Extra>? extras { get; set; } = null;
        /*
         * ************************************************************
         */
        public string ToJSON(string indent = "")
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }
    }
    public class SparseAccessor
    {
        // Number of deviating accessor values stored in the sparse array.
        // Required: Yes
        public int count { get; set; } = 0;

        // Type: accessor.sparse.indices
        // An object pointing to a buffer view containing the indices of deviating accessor values.
        // The number of indices is equal to count.
        // Indices MUST strictly increase.
        // Required: Yes
        public List<SparseAccessorIndices>? indices { get; set; } = null;

        // Type: accessor.sparse.values
        // An object pointing to a buffer view containing the deviating accessor values.
        // Required:  Yes
        public List<SparseAccessorValues>? values { get; set; } = null;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public List<Extra>? extras { get; set; } = null;
    }
    public class SparseAccessorIndices
    {

    }
    public class SparseAccessorValues
    {

    }
}
