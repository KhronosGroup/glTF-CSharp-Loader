using System.Text;

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
        public Material[] materials { get; set; } = new Material[0];

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
        public Animation[] animations { get; set; } = new Animation[0];

        // Type: camera[1 - *]
        // An array of cameras.
        // Required: No
        public Camera[] cameras { get; set; } = new Camera[0];

        // Type: Image [1-*]
        // An array of images.
        // Required: No
        public Image[] images {get;set;} = new Image[0];    

        // Type: sampler[1 - *]
        // An array of samplers.
        // Required: No
        public Sampler[] samplers { get; set; } = new Sampler[0];   

        // Type: skin [1-*]
        // An array of skins.
        // Required: No
        public Skin[] skins { get; set; } = new Skin[0];

        // Type: texture[1 - *]
        // An array of textures.
        // Required: No
        public Texture[] textures { get; set; } = new Texture[0];   

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public Extension[] extensions { get; set; } = new Extension[0];

        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra[] extras { get; set; } = new Extra[0];

        /*
        *************************************************************************
        */
        public string ToJSON()
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
                    if(nExt < (extensions.Length - 1))
                    {
                        sb.Append(",");
                    }
                    sb.Append("\"\r\n");
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
                    if (nExt < (extensionsRequired.Length - 1))
                    {
                        sb.Append(",");
                    }
                    sb.Append("\"\r\n");
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
                    sb.Append(scene.ToJSON());
                }
                sb.Append("\r\n\t]");
            }
            sb.Append("\r\n}\r\n");
            return sb.ToString();
        }
    }
}
