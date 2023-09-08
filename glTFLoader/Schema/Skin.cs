//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace glTFLoader.Schema {
    using System.Linq;
    using System.Runtime.Serialization;
    
    
    public class Skin : GltfChildOfRootProperty {
        
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
        /// The index of the accessor containing the floating-point 4x4 inverse-bind matrices.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("inverseBindMatrices")]
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
        [System.Text.Json.Serialization.JsonPropertyName("skeleton")]
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
        [System.Text.Json.Serialization.JsonRequired()]
        [System.Text.Json.Serialization.JsonPropertyName("joints")]
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
    }
}
