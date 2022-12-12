using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class AnimationSampler
    {
        // Type: integer
        // The index of an accessor containing keyframe timestamps.
        // Required: Yes
        public int input { get; set; } = -1;

        // Type: string
        // Interpolation algorithm.
        // Required: No, default: "LINEAR"
        // "LINEAR" The animated values are linearly interpolated between keyframes.
        // When targeting a rotation, spherical linear interpolation (slerp) SHOULD be used to interpolate quaternions.
        // The number of output elements MUST equal the number of input elements.
        // "STEP" The animated values remain constant to the output of the first keyframe, until the next keyframe.
        // The number of output elements MUST equal the number of input elements.
        // "CUBICSPLINE" The animation’s interpolation is computed using a cubic spline with specified tangents.
        // The number of output elements MUST equal three times the number of input elements.
        // For each input element, the output stores three elements, an in-tangent, a spline vertex, and an out-tangent.
        // There MUST be at least two keyframes when using this interpolation.
        public string interpolation { get; set; } = "LINEAR";

        // Type: integer
        // The index of an accessor, containing keyframe output values.
        // Required: Yes
        public int output { get; set; } = -1;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public Extension[] extensions { get; set; } = new Extension[0];

        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[] extras { get; set; } = new Extra[0];
    }
}
