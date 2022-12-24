using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace glTFInterface
{
    public class Node
    {
        // The user-defined name of this object.
        // Type: string
        // The user-defined name of this object.
        // Required: No
        public string name { get; set; } = "not set";

        // The index of the camera referenced by this node.
        // Type: integer
        // The index of the camera referenced by this node.
        // Required: No
        public Camera? camera { get; set; } = null;

        // child node indices - note that all nodes are stored in the top level glTF container
        // Type: integer [1-*]
        // The indices of this node’s children.
        // Required:  No
        public List<int>? children = null;

        // The following four transformations are either a matrix or one or more of the other three
        // This can be tested by checking the length of the arrays

        // the matrix is in column major order default is 1, 0, 0, 0,  0, 1, 0, 0,   0, 0, 1, 0,  0, 0, 0, 1
        // Type: number [16]
        // A floating-point 4x4 transformation matrix stored in column-major order.
        // Required: No, default: [1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1]
        public List<double>? matrix { get; set; } = null;

        // translation is dx, dy, dz with default is 0, 0, 0
        // Type: number [3]
        // The node’s translation along the x, y, and z axes.
        // Required: No, default: [0,0,0]
        public List<double>? translation { get; set; } = null;

        // rotation is a unit quaternion x, y, z, w default is 0, 0, 0, 1
        // Type: number [4]
        // The node’s unit quaternion rotation in the order(x, y, z, w), where w is the scalar.
        // Required: No, default: [0,0,0,1]
        public List<double>? rotation { get; set; } = null;

        // scale is in x, y, z axis order default is 1, 1, 1
        // Type: number [3]
        // The node’s non-uniform scale, given as the scaling factors along the x, y, and z axes.
        // Required: No, default: [1,1,1]
        public List<double>? scale { get; set; } = null;

        // Type: integer
        // The index of the mesh in this node.
        // Required: No
        public int? mesh { get; set; } = null;

        // The index of the skin referenced by this node.
        // Type: integer
        // The index of the skin referenced by this node.
        // Required: No
        public int? skin { get; set; } = null;

        // The weights of the instantiated morph target.
        // The number of array elements MUST match the number of morph targets of the referenced mesh.
        // When defined, mesh MUST also be defined.
        // Type: number [1-*]
        // The weights of the instantiated morph target.
        // The number of array elements MUST match the number of morph targets of the referenced mesh.
        // When defined, mesh MUST also be defined.
        // Required: No
        public List<double>? weights { get; set; } = null;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public List<Extra>? extras { get; set; } = null;
        public string ToJSON(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            // name
            sb.Append("\r\n\t" + indent + "\"name\": \"" + name + "\"");
            // nodes
            sb.Append(",\r\n\t" + indent + "\"nodes\": [");
            bool isFirst = true;
 /*           foreach (int nodeIndex in nodes)
            {
                if (!isFirst)
                {
                    sb.Append(",");
                }
                else
                {
                    isFirst = false;
                    sb.Append("\r\n" + indent + "    ");
                }
                sb.Append(" " + nodeIndex.ToString());
            } */
            sb.Append("\r\n\t" + indent + "]");
            // extensions
            sb.Append(",\r\n\t" + indent + "\"extensions\": {");
            isFirst = true;
            //foreach (Extension ext in extensions)
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
