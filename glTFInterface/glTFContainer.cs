namespace glTFInterface
{
    public class glTFSet
    {
        Asset asset { get; set; } = new Asset();
        ExtensionsUsed extensionsUsed { get; set; } = new ExtensionsUsed();
        ExtensionsRequired extensionsRequired { get; set; } = new ExtensionsRequired();
        DefaultScene scene { get; set; } = new DefaultScene();
        Scene[] scenes { get; set; } = new Scene[1];
        Node[] nodes { get; set; } = new Node[1];
        Material[] materials { get; set; } = new Material[1];
        Mesh[] meshes { get; set; } = new Mesh[1];
        Accessor[]  accessors { get; set; } = new Accessor[1];
        BufferView[] bufferViews { get; set; } = new BufferView[1];
        Buffer[] buffers { get; set; } = new Buffer[1];

    }
}
