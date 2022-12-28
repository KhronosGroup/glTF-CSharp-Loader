using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class AccessorSparse
    {
        // Number of deviating accessor values stored in the sparse array.
        // Required: Yes
        public int count { get; set; } = 0;

        // Type: accessor.sparse.indices
        // An object pointing to a buffer view containing the indices of deviating accessor values.
        // The number of indices is equal to count.
        // Indices MUST strictly increase.
        // Required: Yes
        public List<AccessorSparseIndices>? indices { get; set; } = null;

        // Type: accessor.sparse.values
        // An object pointing to a buffer view containing the deviating accessor values.
        // Required:  Yes
        public List<AccessorSparseValues>? values { get; set; } = null;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public List<Extra>? extras { get; set; } = null;
        private bool isLocked = false;
        public void Lock()
        {
            isLocked = true;
        }
        public void Unlock()
        {
            isLocked = false;
        }

    }

}
