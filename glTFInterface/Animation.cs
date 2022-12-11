using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    /*
     * 
channels

animation.channel [1-*]

An array of animation channels. An animation channel combines an animation sampler with a target property being animated. Different channels of the same animation MUST NOT have the same targets.

 Yes

samplers

animation.sampler [1-*]

An array of animation samplers. An animation sampler combines timestamps with a sequence of output values and defines an interpolation algorithm.

 Yes

name

string

The user-defined name of this object.

No

extensions

extension

JSON object with extension-specific objects.

No

extras

extras

Application-specific data.

No     * 
     */
    public class Animation
    {
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
