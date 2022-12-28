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
        private List<AnimationChannel> m_channels = new List<AnimationChannel>();
        public List<AnimationChannel>? channels
        {
            get
            {
                if (m_channels.Count < 1)
                {
                    return null;
                }
                return m_channels;
            }  
        }

        // Type: animation.sampler[1 - *]
        // An array of animation samplers.An animation sampler combines timestamps with a sequence of output values and defines an interpolation algorithm.
        // Required: Yes
        private List<AnimationSampler> m_samplers = new List<AnimationSampler>();
        public List<AnimationSampler>? samplers
        {
            get
            {
                if(m_samplers.Count < 1)
                {
                    return null;  
                }
                return m_samplers;
            }
        }

        // Type: string
        // The user-defined name of this object.
        // Required: No
        public string name { get; set; } = String.Empty;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra? extras { get; set; } = null;

        private bool isLocked = false;
        public void Lock()
        {
            if (channels != null)
            {
                foreach (AnimationChannel channel in channels)
                {
                    channel.Lock();
                }
            }
            if (samplers != null)
            {
                foreach (AnimationSampler sampler in samplers)
                {
                    sampler.Lock();
                }
            }
            isLocked = true;
        }
        public void Unlock()
        {
            if (channels != null)
            {
                foreach (AnimationChannel channel in channels)
                {
                    channel.Unlock();
                }
            }
            if (samplers != null)
            {
                foreach (AnimationSampler sampler in samplers)
                {
                    sampler.Unlock();
                }
            }
            isLocked = false;
        }
    }
}
