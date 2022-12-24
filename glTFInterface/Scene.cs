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
        private List<int>? m_nodes = null;
        public List<int> nodes
        {
            get
            {
                if(m_nodes == null)
                {
                    m_nodes = new List<int>();
                }
                return m_nodes;
            }
        }

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>?  extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public List<Extra>? extras { get; set; } = null;

        /*
         * *** ***
         */

        public string ToJSON(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            // name
            sb.Append("\r\n\t" + indent + "\"name\": \"" + name + "\"");
            // nodes
            sb.Append(",\r\n\t" + indent + "\"nodes\": [");
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
                    sb.Append("\r\n" + indent + "    ");
                }
                sb.Append(" " + nodeIndex.ToString());
            }
            sb.Append("\r\n\t" + indent + "]");
            // extensions
            sb.Append(",\r\n\t" + indent + "\"extensions\": {");
            isFirst = true;
            foreach (KeyValuePair<string, object> ext in extensions)
            {
                if (!isFirst)
                {
                    sb.Append(",");
                }
                else
                {
                    isFirst = false;
                    sb.Append("\r\n\t" + indent + "    ");
                }
                //sb.Append(ext.ToJSON(indent + "\t\t"));
            }
            sb.Append("\r\n\t" + indent + "}");

            //
            sb.Append("\r\n" + indent + "}");
            return sb.ToString();
        }
    }
}
