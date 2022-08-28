//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace glTFLoader.Schema {
    using System.Linq;
    using System.Runtime.Serialization;
    
    
    public class Skin {
        
        /// <summary>
        /// Backing field for InverseBindMatrices.
        /// </summary>
        private System.Nullable<int> m_inverseBindMatrices;
        
        /// <summary>
        /// Backing field for Skeleton.
        /// </summary>
        private System.Nullable<int> m_skeleton;
        
        /// <summary>
        /// Backing field for Joints.
        /// </summary>
        private int[] m_joints;
        
        /// <summary>
        /// Backing field for Name.
        /// </summary>
        private string m_name;
        
        /// <summary>
        /// Backing field for Extensions.
        /// </summary>
        private System.Collections.Generic.Dictionary<string, object> m_extensions;
        
        /// <summary>
        /// Backing field for Extras.
        /// </summary>
        private Extras m_extras;
        
        /// <summary>
        /// The index of the accessor containing the floating-point 4x4 inverse-bind matrices.
        /// </summary>
        [Newtonsoft.Json.JsonPropertyAttribute("inverseBindMatrices")]
        public System.Nullable<int> InverseBindMatrices {
            get {
                return this.m_inverseBindMatrices;
            }
            set {
                if ((value < 0)) {
                    throw new System.ArgumentOutOfRangeException("InverseBindMatrices", value, "Expected value to be greater than or equal to 0");
                }
                this.m_inverseBindMatrices = value;
            }
        }
        
        /// <summary>
        /// The index of the node used as a skeleton root.
        /// </summary>
        [Newtonsoft.Json.JsonPropertyAttribute("skeleton")]
        public System.Nullable<int> Skeleton {
            get {
                return this.m_skeleton;
            }
            set {
                if ((value < 0)) {
                    throw new System.ArgumentOutOfRangeException("Skeleton", value, "Expected value to be greater than or equal to 0");
                }
                this.m_skeleton = value;
            }
        }
        
        /// <summary>
        /// Indices of skeleton nodes, used as joints in this skin.
        /// </summary>
        [Newtonsoft.Json.JsonConverterAttribute(typeof(glTFLoader.Shared.ArrayConverter))]
        [Newtonsoft.Json.JsonRequiredAttribute()]
        [Newtonsoft.Json.JsonPropertyAttribute("joints")]
        public int[] Joints {
            get {
                return this.m_joints;
            }
            set {
                if ((value == null)) {
                    this.m_joints = value;
                    return;
                }
                if ((value.Length < 1u)) {
                    throw new System.ArgumentException("Array not long enough");
                }
                int index = 0;
                for (index = 0; (index < value.Length); index = (index + 1)) {
                    if ((value[index] < 0)) {
                        throw new System.ArgumentOutOfRangeException();
                    }
                }
                this.m_joints = value;
            }
        }
        
        /// <summary>
        /// The user-defined name of this object.
        /// </summary>
        [Newtonsoft.Json.JsonPropertyAttribute("name")]
        public string Name {
            get {
                return this.m_name;
            }
            set {
                this.m_name = value;
            }
        }
        
        /// <summary>
        /// JSON object with extension-specific objects.
        /// </summary>
        [Newtonsoft.Json.JsonPropertyAttribute("extensions")]
        public System.Collections.Generic.Dictionary<string, object> Extensions {
            get {
                return this.m_extensions;
            }
            set {
                this.m_extensions = value;
            }
        }
        
        /// <summary>
        /// Application-specific data.
        /// </summary>
        [Newtonsoft.Json.JsonPropertyAttribute("extras")]
        public Extras Extras {
            get {
                return this.m_extras;
            }
            set {
                this.m_extras = value;
            }
        }
        
        public bool ShouldSerializeInverseBindMatrices() {
            return ((m_inverseBindMatrices == null) 
                        == false);
        }
        
        public bool ShouldSerializeSkeleton() {
            return ((m_skeleton == null) 
                        == false);
        }
        
        public bool ShouldSerializeJoints() {
            return ((m_joints == null) 
                        == false);
        }
        
        public bool ShouldSerializeName() {
            return ((m_name == null) 
                        == false);
        }
        
        public bool ShouldSerializeExtensions() {
            return ((m_extensions == null) 
                        == false);
        }
        
        public bool ShouldSerializeExtras() {
            return ((m_extras == null) 
                        == false);
        }
    }
}
