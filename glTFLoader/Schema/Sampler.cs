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
    
    
    public class Sampler : GltfChildOfRootProperty {
        
        /// <summary>
        /// Backing field for MagFilter.
        /// </summary>
        private System.Nullable<MagFilterEnum> m_magFilter;
        
        /// <summary>
        /// Backing field for MinFilter.
        /// </summary>
        private System.Nullable<MinFilterEnum> m_minFilter;
        
        /// <summary>
        /// Backing field for WrapS.
        /// </summary>
        private WrapSEnum m_wrapS = WrapSEnum.REPEAT;
        
        /// <summary>
        /// Backing field for WrapT.
        /// </summary>
        private WrapTEnum m_wrapT = WrapTEnum.REPEAT;
        
        /// <summary>
        /// Magnification filter.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("magFilter")]
        public System.Nullable<MagFilterEnum> MagFilter {
            get {
                return this.m_magFilter;
            }
            set {
                this.m_magFilter = value;
            }
        }
        
        /// <summary>
        /// Minification filter.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("minFilter")]
        public System.Nullable<MinFilterEnum> MinFilter {
            get {
                return this.m_minFilter;
            }
            set {
                this.m_minFilter = value;
            }
        }
        
        /// <summary>
        /// S (U) wrapping mode.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("wrapS")]
        public WrapSEnum WrapS {
            get {
                return this.m_wrapS;
            }
            set {
                this.m_wrapS = value;
            }
        }
        
        /// <summary>
        /// T (V) wrapping mode.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("wrapT")]
        public WrapTEnum WrapT {
            get {
                return this.m_wrapT;
            }
            set {
                this.m_wrapT = value;
            }
        }
        
        public bool ShouldSerializeMagFilter() {
            return ((m_magFilter == null) 
                        == false);
        }
        
        public bool ShouldSerializeMinFilter() {
            return ((m_minFilter == null) 
                        == false);
        }
        
        public bool ShouldSerializeWrapS() {
            return ((m_wrapS == WrapSEnum.REPEAT) 
                        == false);
        }
        
        public bool ShouldSerializeWrapT() {
            return ((m_wrapT == WrapTEnum.REPEAT) 
                        == false);
        }
        
        public enum MagFilterEnum {
            
            NEAREST = 9728,
            
            LINEAR = 9729,
        }
        
        public enum MinFilterEnum {
            
            NEAREST = 9728,
            
            LINEAR = 9729,
            
            NEAREST_MIPMAP_NEAREST = 9984,
            
            LINEAR_MIPMAP_NEAREST = 9985,
            
            NEAREST_MIPMAP_LINEAR = 9986,
            
            LINEAR_MIPMAP_LINEAR = 9987,
        }
        
        public enum WrapSEnum {
            
            CLAMP_TO_EDGE = 33071,
            
            MIRRORED_REPEAT = 33648,
            
            REPEAT = 10497,
        }
        
        public enum WrapTEnum {
            
            CLAMP_TO_EDGE = 33071,
            
            MIRRORED_REPEAT = 33648,
            
            REPEAT = 10497,
        }
    }
}
