namespace glTFInterface
{
    public class glTFRoot
    {
        Asset asset { get; set; } = new Asset();
        // Type: string[1 - *]
        // Names of glTF extensions used in this asset.
        // Required: No
        ExtensionsUsed extensionsUsed { get; set; } = new ExtensionsUsed();
        // Type: string [1-*]
        // Names of glTF extensions required to properly load this asset.
        // Required: No
        ExtensionsRequired extensionsRequired { get; set; } = new ExtensionsRequired();
        //
        DefaultScene scene { get; set; } = new DefaultScene();
        //
        Scene[] scenes { get; set; } = new Scene[0];
        //
        Node[] nodes { get; set; } = new Node[0];
        //
        Material[] materials { get; set; } = new Material[0];
        //
        Mesh[] meshes { get; set; } = new Mesh[0];
        //
        Accessor[]  accessors { get; set; } = new Accessor[0];
        //
        BufferView[] bufferViews { get; set; } = new BufferView[0];
        //
        Buffer[] buffers { get; set; } = new Buffer[0];

        /*
         * 
extensionsUsed

string [1-*]

Names of glTF extensions used in this asset.

No

extensionsRequired

string [1-*]

Names of glTF extensions required to properly load this asset.

No

accessors

accessor [1-*]

An array of accessors.

No

animations

animation [1-*]

An array of keyframe animations.

No

asset

asset

Metadata about the glTF asset.

 Yes

buffers

buffer [1-*]

An array of buffers.

No

bufferViews

bufferView [1-*]

An array of bufferViews.

No

cameras

camera [1-*]

An array of cameras.

No

images

image [1-*]

An array of images.

No

materials

material [1-*]

An array of materials.

No

meshes

mesh [1-*]

An array of meshes.

No

nodes

node [1-*]

An array of nodes.

No

samplers

sampler [1-*]

An array of samplers.

No

scene

integer

The index of the default scene.

No

scenes

scene [1-*]

An array of scenes.

No

skins

skin [1-*]

An array of skins.

No

textures

texture [1-*]

An array of textures.

No

extensions

extension

JSON object with extension-specific objects.

No

extras

extras

Application-specific data.

No         * 
         */
    }
}
