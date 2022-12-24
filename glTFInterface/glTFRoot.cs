using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace glTFInterface
{
    public class glTFRoot
    {
        public Asset asset { get; set; } = new Asset();

        // Type: string[1 - *]
        // Names of glTF extensions used in this asset.
        // Required: No
        public List<string> extensionsUsed { get; set; } = new List<string>();

        // Type: string [1-*]
        // Names of glTF extensions required to properly load this asset.
        // Required: No
        public List<string> extensionsRequired { get; set; } = new List<string>();

        // Type:  integer
        // The index of the default scene.
        // Required: No
        public int scene { get; set; } = 0;

        // Type: scene[1 - *]
        // An array of scenes.
        // Required: No
        public List<Scene> scenes { get; set; } = new List<Scene>();

        // Type: node[1 - *]
        // An array of nodes.
        // Required: No
        public List<Node> nodes { get; set; } = new List<Node>();

        // Type: material[1 - *]
        // An array of materials.
        // Required: No
        public List<Material>? materials { get; set; } = null;

        // Type: mesh[1 - *]
        // An array of meshes.
        // Required: No
        public List<Mesh>? meshes { get; set; } = null;

        // Type: accessor[1 - *]
        // An array of accessors.
        // Required: No
        public List<Accessor>? accessors { get; set; } = null;

        // Type: bufferView[1 - *]
        // An array of bufferViews.
        // Required: No
        public List<BufferView>? bufferViews { get; set; } = null;

        // Type: buffer[1 - *]
        // An array of buffers.
        // Required: No
        public List<Buffer>? buffers { get; set; } = null;

        // Type: animation[1 - *]
        // An array of keyframe animations.
        // Required: No
        public List<Animation>? animations { get; set; } = null;

        // Type: camera[1 - *]
        // An array of cameras.
        // Required: No
        public List<Camera>? cameras { get; set; } = null;

        // Type: Image [1-*]
        // An array of images.
        // Required: No
        public List<Image>? images {get;set;} = null;    

        // Type: sampler[1 - *]
        // An array of samplers.
        // Required: No
        public List<Sampler>? samplers { get; set; } = null;   

        // Type: skin [1-*]
        // An array of skins.
        // Required: No
        public List<Skin>? skins { get; set; } = null;

        // Type: texture[1 - *]
        // An array of textures.
        // Required: No
        public List<Texture>? textures { get; set; } = null;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        //public Extension[]? extensions { get; set; } = null;
        public System.Collections.Generic.Dictionary<string, object>? extensions = null;
        // Type: extras
        // Application-specific data.
        // Required: No
        public List<Extra>? extras { get; set; } = null;

        /*
        *************************************************************************
        */
        public string ToJSON()
        {
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize<glTFRoot>(this, options);
            return json;
        }
    }
}
