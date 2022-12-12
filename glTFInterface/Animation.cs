using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Animation
    {
        // Type: animation.channel[1 - *]
        // An array of animation channels.An animation channel combines an animation sampler with a target property being animated. Different channels of the same animation MUST NOT have the same targets.
        // Required: Yes
        public AnimationChannel[] channels { get; set; } = new AnimationChannel[0];

        // Type: animation.sampler[1 - *]
        // An array of animation samplers.An animation sampler combines timestamps with a sequence of output values and defines an interpolation algorithm.
        // Required: Yes
        public AnimationSampler[] samplers { get; set; } = new AnimationSampler[0];

        // Type: string
        // The user-defined name of this object.
        // Required: No
        public string name { get; set; } = "not set";

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
