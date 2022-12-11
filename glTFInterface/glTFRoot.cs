namespace glTFInterface
{
    public class glTFContainer
    {
        Asset asset { get; set; } = new Asset();
        ExtensionsUsed extensionsUsed { get; set; } = new ExtensionsUsed();
        ExtensionsRequired extensionsRequired { get; set; } = new ExtensionsRequired();
        DefaultScene scene { get; set; } = new DefaultScene();
        Scene[] scenes { get; set; } = new Scene[0];
        Node[] nodes { get; set; } = new Node[0];
        Material[] materials { get; set; } = new Material[0];
        Mesh[] meshes { get; set; } = new Mesh[0];
        Accessor[]  accessors { get; set; } = new Accessor[0];
        BufferView[] bufferViews { get; set; } = new BufferView[0];
        Buffer[] buffers { get; set; } = new Buffer[0];
    }
}
