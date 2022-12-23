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
        public string[] extensionsUsed { get; set; } = new string[0];

        // Type: string [1-*]
        // Names of glTF extensions required to properly load this asset.
        // Required: No
        public string[] extensionsRequired { get; set; } = new string[0];

        // Type:  integer
        // The index of the default scene.
        // Required: No
        public int scene { get; set; } = 0;

        // Type: scene[1 - *]
        // An array of scenes.
        // Required: No
        public Scene[] scenes { get; set; } = new Scene[0];

        // Type: node[1 - *]
        // An array of nodes.
        // Required: No
        public Node[] nodes { get; set; } = new Node[0];

        // Type: material[1 - *]
        // An array of materials.
        // Required: No
        public List<Material> materials { get; set; } = new List<Material>();

        // Type: mesh[1 - *]
        // An array of meshes.
        // Required: No
        public Mesh[] meshes { get; set; } = new Mesh[0];

        // Type: accessor[1 - *]
        // An array of accessors.
        // Required: No
        public Accessor[] accessors { get; set; } = new Accessor[0];

        // Type: bufferView[1 - *]
        // An array of bufferViews.
        // Required: No
        public BufferView[] bufferViews { get; set; } = new BufferView[0];

        // Type: buffer[1 - *]
        // An array of buffers.
        // Required: No
        public Buffer[] buffers { get; set; } = new Buffer[0];

        // Type: animation[1 - *]
        // An array of keyframe animations.
        // Required: No
        public Animation[]? animations { get; set; } = null;

        // Type: camera[1 - *]
        // An array of cameras.
        // Required: No
        public Camera[]? cameras { get; set; } = null;

        // Type: Image [1-*]
        // An array of images.
        // Required: No
        public Image[]? images {get;set;} = null;    

        // Type: sampler[1 - *]
        // An array of samplers.
        // Required: No
        public Sampler[]? samplers { get; set; } = null;   

        // Type: skin [1-*]
        // An array of skins.
        // Required: No
        public Skin[]? skins { get; set; } = null;

        // Type: texture[1 - *]
        // An array of textures.
        // Required: No
        public Texture[]? textures { get; set; } = null;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        //public Extension[]? extensions { get; set; } = null;
        public System.Collections.Generic.Dictionary<string, object>? extensions = null;
        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[]? extras { get; set; } = null;

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
        public string ToJSONX()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\r\n");

            // Extensions Used
            /*
             * 
	"extensionsUsed": [
		"OGC_Geo_Semantic_Replica"
	],
	"extensionsRequired": [
		"OGC_Geo_Semantic_Replica"
	],
	"asset": {
		"generator": "SGR00",
		"version": "2.0"
	},
             * 
             */
            if (extensionsUsed.Length > 0)
            {
               sb.Append("\t\"extensionsUsed\": [\r\n");
               for(int nExt = 0; nExt < extensionsUsed.Length; nExt++)
               {
                    sb.Append("\t\t\"");
                    sb.Append(extensionsUsed[nExt]);
                    sb.Append("\"");
                    if (nExt < (extensionsUsed.Length - 1))
                    {
                        sb.Append(",");
                    }
                    sb.Append("\r\n");
                }
                sb.Append("\t],\r\n");
            }

            // ExtensionsRequired
            if (extensionsRequired.Length > 0)
            {
                sb.Append("\t\"extensionsRequired\": [\r\n");
                for (int nExt = 0; nExt < extensionsRequired.Length; nExt++)
                {
                    sb.Append("\t\t\"");
                    sb.Append(extensionsRequired[nExt]);
                    sb.Append("\"");
                    if (nExt < (extensionsRequired.Length - 1))
                    {
                        sb.Append(",");
                    }
                    sb.Append("\r\n");
                }
                sb.Append("\t],\r\n");
            }

            // Asset
            if (extensionsRequired.Length > 0)
            {
                sb.Append("\t\"asset\": {\r\n");
                sb.Append("\t\t");
                sb.Append("\"generator\": \"");
                sb.Append(asset.generator);
                sb.Append("\",\r\n");
                sb.Append("\t\t\"version\": \"");
                sb.Append(asset.version);
                sb.Append("\"\r\n");
                sb.Append("\t}");
            }
            // scene
            if(scene >= 0)
            {
                sb.Append(",\r\n\t\"scene\": " + scene.ToString());
            }
            // scenes
            if(scenes.Length > 0)
            {
                string indent = "\t";
                sb.Append(",\r\n\t\"scenes\": [");
                bool isFirst = true;
                foreach (Scene scene in scenes)
                {
                    if(!isFirst)
                    {
                        sb.Append(",\r\n\t\t");
                    }
                    else
                    {
                        isFirst = false;
                        sb.Append("\r\n\t\t");
                    }
                    sb.Append(scene.ToJSON(indent + "\t"));
                }
                sb.Append("\r\n\t]");
            }
            // nodes
            if (nodes.Length > 0)
            {
                string indent = "\t";
                sb.Append(",\r\n\t\"nodes\": [");
                bool isFirst = true;
                foreach (Node node in nodes)
                {
                    if (!isFirst)
                    {
                        sb.Append(",\r\n\t\t");
                    }
                    else
                    {
                        isFirst = false;
                        sb.Append("\r\n\t\t");
                    }
                    sb.Append(node.ToJSON(indent + "\t"));
                }
                sb.Append("\r\n\t]");
            }

            // materials
            if (materials.Count > 0)
            {
                string indent = "\t";
                sb.Append(",\r\n\t\"materials\": [");
                bool isFirst = true;
                foreach (Material material in materials)
                {
                    if (!isFirst)
                    {
                        sb.Append(",\r\n\t\t");
                    }
                    else
                    {
                        isFirst = false;
                        sb.Append("\r\n\t\t");
                    }
                    sb.Append(material.ToJSON(indent + "\t"));
                }
                sb.Append("\r\n\t]");
            }

            // meshes
            if (meshes.Length > 0)
            {
                string indent = "\t";
                sb.Append(",\r\n\t\"meshes\": [");
                bool isFirst = true;
                foreach (Material material in materials)
                {
                    if (!isFirst)
                    {
                        sb.Append(",\r\n\t\t");
                    }
                    else
                    {
                        isFirst = false;
                        sb.Append("\r\n\t\t");
                    }
                    sb.Append(material.ToJSON(indent + "\t"));
                }
                sb.Append("\r\n\t]");
            }

            // accessors
            if (accessors.Length > 0)
            {
                string indent = "\t";
                sb.Append(",\r\n\t\"accessors\": [");
                bool isFirst = true;
                foreach (Accessor accessor in accessors)
                {
                    if (!isFirst)
                    {
                        sb.Append(",\r\n\t\t");
                    }
                    else
                    {
                        isFirst = false;
                        sb.Append("\r\n\t\t");
                    }
                    sb.Append(accessor.ToJSON(indent + "\t"));
                }
                sb.Append("\r\n\t]");
            }

            // bufferViews
            if (bufferViews.Length > 0)
            {
                string indent = "\t";
                sb.Append(",\r\n\t\"bufferViews\": [");
                bool isFirst = true;
                foreach (BufferView bufferView in bufferViews)
                {
                    if (!isFirst)
                    {
                        sb.Append(",\r\n\t\t");
                    }
                    else
                    {
                        isFirst = false;
                        sb.Append("\r\n\t\t");
                    }
                    sb.Append(bufferView.ToJSON(indent + "\t"));
                }
                sb.Append("\r\n\t]");
            }

            // buffers
            if (buffers.Length > 0)
            {
                string indent = "\t";
                sb.Append(",\r\n\t\"buffers\": [");
                bool isFirst = true;
                foreach (Buffer buffer in buffers)
                {
                    if (!isFirst)
                    {
                        sb.Append(",\r\n\t\t");
                    }
                    else
                    {
                        isFirst = false;
                        sb.Append("\r\n\t\t");
                    }
                    sb.Append(buffer.ToJSON(indent + "\t"));
                }
                sb.Append("\r\n\t]");
            }

            sb.Append("\r\n}\r\n");
            return sb.ToString();
        }
    }
}
