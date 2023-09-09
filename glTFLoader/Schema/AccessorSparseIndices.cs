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
    
    
    public class AccessorSparseIndices : GltfProperty {
        
        /// <summary>
        /// Backing field for BufferView.
        /// </summary>
        private int m_bufferView;
        
        /// <summary>
        /// Backing field for ByteOffset.
        /// </summary>
        private int m_byteOffset = 0;
        
        /// <summary>
        /// Backing field for ComponentType.
        /// </summary>
        private ComponentTypeEnum m_componentType;
        
        /// <summary>
        /// The index of the buffer view with sparse indices. The referenced buffer view **MUST NOT** have its `target` or `byteStride` properties defined. The buffer view and the optional `byteOffset` **MUST** be aligned to the `componentType` byte length.
        /// </summary>
        [System.Text.Json.Serialization.JsonRequired()]
        [System.Text.Json.Serialization.JsonPropertyName("bufferView")]
        public int BufferView {
            get {
                return this.m_bufferView;
            }
            set {
                if ((value < 0)) {
                    throw new System.ArgumentOutOfRangeException("BufferView", value, "Expected value to be greater than or equal to 0");
                }
                this.m_bufferView = value;
            }
        }
        
        /// <summary>
        /// The offset relative to the start of the buffer view in bytes.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("byteOffset")]
        public int ByteOffset {
            get {
                return this.m_byteOffset;
            }
            set {
                if ((value < 0)) {
                    throw new System.ArgumentOutOfRangeException("ByteOffset", value, "Expected value to be greater than or equal to 0");
                }
                this.m_byteOffset = value;
            }
        }
        
        /// <summary>
        /// The indices data type.
        /// </summary>
        [System.Text.Json.Serialization.JsonRequired()]
        [System.Text.Json.Serialization.JsonPropertyName("componentType")]
        public ComponentTypeEnum ComponentType {
            get {
                return this.m_componentType;
            }
            set {
                this.m_componentType = value;
            }
        }
        
        public bool ShouldSerializeByteOffset() {
            return ((m_byteOffset == 0) 
                        == false);
        }
        
        public enum ComponentTypeEnum {
            
            UNSIGNED_BYTE = 5121,
            
            UNSIGNED_SHORT = 5123,
            
            UNSIGNED_INT = 5125,
        }
    }
}
