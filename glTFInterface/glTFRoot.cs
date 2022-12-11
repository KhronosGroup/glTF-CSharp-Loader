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
        int scene { get; set; } = 0;

        // Type: scene[1 - *]
        // An array of scenes.
        // Required: No
        Scene[] scenes { get; set; } = new Scene[0];

        // Type: node[1 - *]
        // An array of nodes.
        // Required: No
        Node[] nodes { get; set; } = new Node[0];

        // Type: material[1 - *]
        // An array of materials.
        // Required: No
        Material[] materials { get; set; } = new Material[0];

        // Type: mesh[1 - *]
        // An array of meshes.
        // Required: No
        Mesh[] meshes { get; set; } = new Mesh[0];

        // Type: accessor[1 - *]
        // An array of accessors.
        // Required: No
        Accessor[] accessors { get; set; } = new Accessor[0];

        // Type: bufferView[1 - *]
        // An array of bufferViews.
        // Required: No
        BufferView[] bufferViews { get; set; } = new BufferView[0];

        // Type: buffer[1 - *]
        // An array of buffers.
        // Required: No
        Buffer[] buffers { get; set; } = new Buffer[0];

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
    }
}
