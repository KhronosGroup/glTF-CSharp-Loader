using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Buffer
    {
        // Type: string
        // The URI(or IRI) of the buffer.
        // Required: No
        public string uri { get; set; } = String.Empty;

        // Type: integer
        // The length of the buffer in bytes.
        // Required: Yes
        public int byteLength { get; set; } = -1;

        // Type: string
        // The user-defined name of this object.
        // Required: No
        public string name { get; set; } = string.Empty;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public List<Extra>? extras { get; set; } = null;
        /*
         * **********************************************************
         */
        public string ToJSON(string indent = "")
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();   
        }
    }
}
