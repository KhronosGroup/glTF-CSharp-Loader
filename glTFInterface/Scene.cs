using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    internal class Scene
    {
        public string name { get; set; } = "default";
        public SceneNode[] nodes { get; set; }   = new SceneNode[0];
        public Extension[] extensions { get; set; } = new Extension[0];
    }
    internal class DefaultScene
    {
        public int sceneIndex { get; set; } = 0;
    }
    internal class SceneNode
    {
        public int[] nodes { get; set; } = new int[0];
    }
}
