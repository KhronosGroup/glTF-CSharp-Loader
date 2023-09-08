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
    
    
    public class AnimationSampler : GltfProperty {
        
        /// <summary>
        /// Backing field for Input.
        /// </summary>
        private int m_input;
        
        /// <summary>
        /// Backing field for Interpolation.
        /// </summary>
        private InterpolationEnum m_interpolation = InterpolationEnum.LINEAR;
        
        /// <summary>
        /// Backing field for Output.
        /// </summary>
        private int m_output;
        
        /// <summary>
        /// The index of an accessor containing keyframe timestamps.
        /// </summary>
        [System.Text.Json.Serialization.JsonRequired()]
        [System.Text.Json.Serialization.JsonPropertyName("input")]
        public int Input {
            get {
                return this.m_input;
            }
            set {
                if ((value < 0)) {
                    throw new System.ArgumentOutOfRangeException("Input", value, "Expected value to be greater than or equal to 0");
                }
                this.m_input = value;
            }
        }
        
        /// <summary>
        /// Interpolation algorithm.
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverterWithEnumMemberAttrSupport<InterpolationEnum>))]
        [System.Text.Json.Serialization.JsonPropertyName("interpolation")]
        public InterpolationEnum Interpolation {
            get {
                return this.m_interpolation;
            }
            set {
                this.m_interpolation = value;
            }
        }
        
        /// <summary>
        /// The index of an accessor, containing keyframe output values.
        /// </summary>
        [System.Text.Json.Serialization.JsonRequired()]
        [System.Text.Json.Serialization.JsonPropertyName("output")]
        public int Output {
            get {
                return this.m_output;
            }
            set {
                if ((value < 0)) {
                    throw new System.ArgumentOutOfRangeException("Output", value, "Expected value to be greater than or equal to 0");
                }
                this.m_output = value;
            }
        }
        
        public bool ShouldSerializeInterpolation() {
            return ((m_interpolation == InterpolationEnum.LINEAR) 
                        == false);
        }
        
        public enum InterpolationEnum {
            
            LINEAR,
            
            STEP,
            
            CUBICSPLINE,
        }
    }
}
