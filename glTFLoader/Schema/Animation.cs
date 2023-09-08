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
    
    
    public class Animation : GltfChildOfRootProperty {
        
        /// <summary>
        /// Backing field for Channels.
        /// </summary>
        private AnimationChannel[] m_channels;
        
        /// <summary>
        /// Backing field for Samplers.
        /// </summary>
        private AnimationSampler[] m_samplers;
        
        /// <summary>
        /// An array of animation channels. An animation channel combines an animation sampler with a target property being animated. Different channels of the same animation **MUST NOT** have the same targets.
        /// </summary>
        [System.Text.Json.Serialization.JsonRequired()]
        [System.Text.Json.Serialization.JsonPropertyName("channels")]
        public AnimationChannel[] Channels {
            get {
                return this.m_channels;
            }
            set {
                if ((value == null)) {
                    this.m_channels = value;
                    return;
                }
                if ((value.Length < 1u)) {
                    throw new System.ArgumentException("Array not long enough");
                }
                this.m_channels = value;
            }
        }
        
        /// <summary>
        /// An array of animation samplers. An animation sampler combines timestamps with a sequence of output values and defines an interpolation algorithm.
        /// </summary>
        [System.Text.Json.Serialization.JsonRequired()]
        [System.Text.Json.Serialization.JsonPropertyName("samplers")]
        public AnimationSampler[] Samplers {
            get {
                return this.m_samplers;
            }
            set {
                if ((value == null)) {
                    this.m_samplers = value;
                    return;
                }
                if ((value.Length < 1u)) {
                    throw new System.ArgumentException("Array not long enough");
                }
                this.m_samplers = value;
            }
        }
        
        public bool ShouldSerializeChannels() {
            return ((m_channels == null) 
                        == false);
        }
        
        public bool ShouldSerializeSamplers() {
            return ((m_samplers == null) 
                        == false);
        }
    }
}
