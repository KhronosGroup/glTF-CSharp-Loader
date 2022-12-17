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
        public Extension[] extensions { get; set; } = new Extension[0];

        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[] extras { get; set; } = new Extra[0];
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
