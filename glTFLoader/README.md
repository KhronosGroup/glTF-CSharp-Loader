# glTF2Loader

A C# reference loader for [glTF 2.0](https://www.khronos.org/gltf/), the runtime 3D asset format from the Khronos Group.

## Installation

```
dotnet add package glTF2Loader
```

## Usage

```csharp
using glTFLoader;

// Load a .gltf or .glb model.
var model = Interface.LoadModel("model.gltf");

// Access the deserialized glTF properties.
foreach (var mesh in model.Meshes)
{
    // ...
}
```

The loader also supports saving models, packing/unpacking `.glb` binary files, and reading buffers and images. See the `glTFLoaderUnitTests` project in the repository for more examples.

## Changelog

See [CHANGELOG.md](https://github.com/KhronosGroup/glTF-CSharp-Loader/blob/main/CHANGELOG.md) for the release history.

## License

See [LICENSE.md](https://github.com/KhronosGroup/glTF-CSharp-Loader/blob/main/LICENSE.md).
