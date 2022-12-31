using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace glTFInterface
{
    public class glTFRoot
    {
        public BinChunkStore binChunks = new BinChunkStore();
        public string uri = string.Empty;
        public Asset asset { get; set; } = new Asset();

        // Type: string[1 - *]
        // Names of glTF extensions used in this asset.
        // Required: No
        private List<string>? m_extensionsUsed = null;
        public List<string> extensionsUsed
        {
            get
            {
                if(m_extensionsUsed == null)
                {
                    m_extensionsUsed = new List<string>();
                }
                return m_extensionsUsed;
            }
        }

        // Type: string [1-*]
        // Names of glTF extensions required to properly load this asset.
        // Required: No
        private List<string>? m_extensionsRequired = null;
        public List<string> extensionsRequired
        {
            get
            {
                if (m_extensionsRequired == null)
                {
                    m_extensionsRequired = new List<string>();
                }
                return m_extensionsRequired;
            }
        }

        // Type:  integer
        // The index of the default scene.
        // Required: No
        public int scene { get; set; } = 0;

        // Type: scene[1 - *]
        // An array of scenes.
        // Required: No
        private List<Scene>? m_scenes = null;
        public List<Scene> scenes
        {
            get
            {
                if(m_scenes == null)
                {
                    m_scenes = new List<Scene>();
                }
                return m_scenes;
            }
        }

        // Type: node[1 - *]
        // An array of nodes.
        // Required: No
        private List<Node>? m_nodes = null;
        public List<Node> nodes
        {
            get
            {
                if(m_nodes == null)
                {
                    m_nodes = new List<Node>();
                }
                return m_nodes;
            }
        }

        // Type: material[1 - *]
        // An array of materials.
        // Required: No
        private List<glTFInterface.Material>? m_materials = null;
        public List<glTFInterface.Material> materials
        {
            get
            {
                if(this.m_materials == null)
                {
                    m_materials = new List<glTFInterface.Material>();
                }
                return m_materials;
            }
        }

        // Type: mesh[1 - *]
        // An array of meshes.
        // Required: No
        private List<Mesh>? m_meshes = null;
        public List<Mesh> meshes
        {
            get
            {
                if (m_meshes == null)
                {
                    m_meshes = new List<Mesh>();
                }
                return m_meshes;
            }
        }

        // Type: accessor[1 - *]
        // An array of accessors.
        // Required: No
        private List<Accessor>? m_accessors = null;
        public List<Accessor> accessors
        {
            get
            {
                if (m_accessors == null)
                {
                    m_accessors = new List<Accessor>();
                }
                return m_accessors;
            }
        }

        // Type: bufferView[1 - *]
        // An array of bufferViews.
        // Required: No
        private List<BufferView>? m_bufferViews = null;
        public List<BufferView> bufferViews
        {
            get
            {
                if (m_bufferViews == null)
                {
                    m_bufferViews = new List<BufferView>();
                }
                return m_bufferViews;
            }
        }

        // Type: buffer[1 - *]
        // An array of buffers.
        // Required: No
        private List<Buffer>? m_buffers = null;
        public List<Buffer> buffers
        {
            get
            {
                if (m_buffers == null)
                {
                    m_buffers = new List<Buffer>();
                }
                return m_buffers;
            }
        }

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

        private bool isLocked = false;
        public void Lock()
        {
            isLocked = true;
            foreach (Material material in materials)
            {
                material.Lock();
            }
            foreach (Mesh mesh in meshes)
            {
                mesh.Lock();
            }
            foreach (Accessor accessor in accessors)
            {
               accessor.Lock();
            }
        }
        public void Unlock()
        {
            isLocked = false;
        }

    }
}
