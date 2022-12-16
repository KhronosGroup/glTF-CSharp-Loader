using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Scene
    {
        // Type: string
        // The user-defined name of this object.
        // Required: No
        public string name { get; set; } = "default";

        // Type: integer [1-*]
        // The indices of each root node.
        // Required: No
        public int[] nodes { get; set; }   = new int[0];

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public Extension[] extensions { get; set; } = new Extension[0];

        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[] extras { get; set; } = new Extra[0];

        /*
         * *** ***
         */

        public string ToJSON()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            // name
            sb.Append("\r\n\t\t\t\"name\": \"" + name + "\"");
            // nodes
            sb.Append(",\r\n\t\t\t\"nodes\": [");
            bool isFirst = true;
            foreach(int nodeIndex in nodes)
            {
                if(!isFirst)
                {
                    sb.Append(",");
                }
                else
                {
                    isFirst = false;
                    sb.Append("\r\n\t\t    ");
                }
                sb.Append(" " + nodeIndex.ToString());
            }
            sb.Append("\r\n\t\t\t]");
            // extensions
            sb.Append(",\r\n\t\t\t\"extensions\": {");
            isFirst = true;
            foreach (Extension ext in extensions)
            {
                if (!isFirst)
                {
                    sb.Append(",");
                }
                else
                {
                    isFirst = false;
                    sb.Append("\r\n\t\t\t    ");
                }
                sb.Append(ext.ToJSON());
            }
            sb.Append("\r\n\t\t\t}");

            //
            sb.Append("\r\n\t\t}");
            return sb.ToString();
        }
    }
}
