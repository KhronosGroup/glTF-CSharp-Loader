using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Mesh
    {
        // mesh.primitive[1 - *]
        // An array of primitives, each defining geometry to be rendered.
        // Required: Yes
        private List<MeshPrimitive>? m_primitives = null;
        public List<MeshPrimitive> primitives
        {
            get
            {
                if(m_primitives == null)
                {
                    m_primitives = new List<MeshPrimitive>();
                }
                return m_primitives;    
            }
        }

        // Type: number[1 - *]
        // Array of weights to be applied to the morph targets.The number of array elements MUST match the number of morph targets.
        // Required: No
        private List<double> m_weights = new List<double>();
        public List<double>? weights
        {
            get
            {
                if(isLocked && m_weights.Count < 1)
                {
                    return null;
                }
                return this.m_weights;
            }
        }
        // Type: string
        // The user-defined name of this object.
        // Required: No
        public string name { get; set; } = "not set";

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra? extras { get; set; } = null;

        private bool isLocked = false;
        public void Lock()
        {
            if(primitives != null)
            {
                foreach(MeshPrimitive primitive in primitives)
                {
                    primitive.Lock();
                }
            }
            isLocked = true;
        }
        public void Unlock()
        {
            if (primitives != null)
            {
                foreach (MeshPrimitive primitive in primitives)
                {
                    primitive.Unlock();
                }
            }
            isLocked = false;
        }

    }



}
