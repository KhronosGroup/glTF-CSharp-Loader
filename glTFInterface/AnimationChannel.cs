using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class AnimationChannel
    {
        // Type: integer
        // The index of a sampler in this animation used to compute the value for the target.
        // Required: Yes
        public int sampler { get; set; } = -1;

        // Type: animation.channel.target
        // The descriptor of the animated property.
        // Required: Yes
        public AnimationChannelTarget target { get; set; } = new AnimationChannelTarget();

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[] extras { get; set; } = new Extra[0];
    }
}
