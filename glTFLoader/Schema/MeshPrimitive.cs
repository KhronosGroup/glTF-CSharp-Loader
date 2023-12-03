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
    
    
    public class MeshPrimitive : GltfProperty {
        
        /// <summary>
        /// Backing field for Attributes.
        /// </summary>
        private System.Collections.Generic.Dictionary<string, int> m_attributes;
        
        /// <summary>
        /// Backing field for Indices.
        /// </summary>
        private System.Nullable<int> m_indices;
        
        /// <summary>
        /// Backing field for Material.
        /// </summary>
        private System.Nullable<int> m_material;
        
        /// <summary>
        /// Backing field for Mode.
        /// </summary>
        private ModeEnum m_mode = ModeEnum.TRIANGLES;
        
        /// <summary>
        /// Backing field for Targets.
        /// </summary>
        private System.Collections.Generic.Dictionary<string, int>[] m_targets;
        
        /// <summary>
        /// A plain JSON object, where each key corresponds to a mesh attribute semantic and each value is the index of the accessor containing attribute's data.
        /// </summary>
        [System.Text.Json.Serialization.JsonRequired()]
        [System.Text.Json.Serialization.JsonPropertyName("attributes")]
        public System.Collections.Generic.Dictionary<string, int> Attributes {
            get {
                return this.m_attributes;
            }
            set {
                this.m_attributes = value;
            }
        }
        
        /// <summary>
        /// The index of the accessor that contains the vertex indices.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("indices")]
        public System.Nullable<int> Indices {
            get {
                return this.m_indices;
            }
            set {
                if ((value < 0)) {
                    throw new System.ArgumentOutOfRangeException("Indices", value, "Expected value to be greater than or equal to 0");
                }
                this.m_indices = value;
            }
        }
        
        /// <summary>
        /// The index of the material to apply to this primitive when rendering.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("material")]
        public System.Nullable<int> Material {
            get {
                return this.m_material;
            }
            set {
                if ((value < 0)) {
                    throw new System.ArgumentOutOfRangeException("Material", value, "Expected value to be greater than or equal to 0");
                }
                this.m_material = value;
            }
        }
        
        /// <summary>
        /// The topology type of primitives to render.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("mode")]
        public ModeEnum Mode {
            get {
                return this.m_mode;
            }
            set {
                this.m_mode = value;
            }
        }
        
        /// <summary>
        /// An array of morph targets.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("targets")]
        public System.Collections.Generic.Dictionary<string, int>[] Targets {
            get {
                return this.m_targets;
            }
            set {
                if ((value == null)) {
                    this.m_targets = value;
                    return;
                }
                if ((value.Length < 1u)) {
                    throw new System.ArgumentException("Array not long enough");
                }
                this.m_targets = value;
            }
        }
        
        public bool ShouldSerializeAttributes() {
            return ((m_attributes == null) 
                        == false);
        }
        
        public bool ShouldSerializeIndices() {
            return ((m_indices == null) 
                        == false);
        }
        
        public bool ShouldSerializeMaterial() {
            return ((m_material == null) 
                        == false);
        }
        
        public bool ShouldSerializeMode() {
            return ((m_mode == ModeEnum.TRIANGLES) 
                        == false);
        }
        
        public bool ShouldSerializeTargets() {
            return ((m_targets == null) 
                        == false);
        }
        
        public enum ModeEnum {
            
            POINTS = 0,
            
            LINES = 1,
            
            LINE_LOOP = 2,
            
            LINE_STRIP = 3,
            
            TRIANGLES = 4,
            
            TRIANGLE_STRIP = 5,
            
            TRIANGLE_FAN = 6,
        }
    }
}
