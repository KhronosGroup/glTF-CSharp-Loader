using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    internal class Node
    {
        // The user-defined name of this object.
        public string name { get; set; } = "not set";
        // The index of the camera referenced by this node.
        public Camera camera { get; set; } = new Camera();
        // child node indices - note that all nodes are stored in the top level glTF container
        public int[] children = new int[0];
        // The following four transformations are either a matrix or one or more of the other three
        // This can be tested by checking the length of the arrays

        // the matrix is in column major order default is 1, 0, 0, 0,  0, 1, 0, 0,   0, 0, 1, 0,  0, 0, 0, 1
        public double[] matrix { get; set; } = new double[0];
        // translation is dx, dy, dz with default is 0, 0, 0
        public double[] translation { get; set; } = new double[0];
        // rotation is a unit quaternion x, y, z, w default is 0, 0, 0, 1
        public double[] rotation { get; set; } = new double[0];
        // scale is in x, y, z axis order default is 1, 1, 1
        public double[] scale { get; set; } = new double[0];
        // The index of the skin referenced by this node.
        public int skin { get; set; } = -1;
        // The weights of the instantiated morph target.
        // The number of array elements MUST match the number of morph targets of the referenced mesh.
        // When defined, mesh MUST also be defined.
        public double[] weights { get; set; } = new double[0];
        public Extension[] extensions { get; set; } = new Extension[0];
        public Extra[] extras { get; set; } = new Extra[0];


    }
}
