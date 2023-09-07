using System.Text.Json;
using System.Text.Json.Serialization;
using glTFLoader.Schema;

namespace glTFLoader
{
    [JsonSerializable(typeof(Gltf))]
    [JsonSerializable(typeof(JsonElement))] // for extensions/extra
    // nested enums with the same name must be added here with discriminators
    // (or it will fail at runtime init with cryptic error)
    [JsonSerializable(typeof(Accessor.ComponentTypeEnum), TypeInfoPropertyName = "Accessor_ComponentTypeEnum")]
    [JsonSerializable(typeof(AccessorSparseIndices.ComponentTypeEnum), TypeInfoPropertyName = "AccessorSparseIndices_ComponentTypeEnum")]
    [JsonSerializable(typeof(Accessor.TypeEnum), TypeInfoPropertyName = "Accessor_TypeEnum")]
    [JsonSerializable(typeof(Camera.TypeEnum), TypeInfoPropertyName = "Camera_TypeEnum")]
    [JsonSourceGenerationOptions(UseStringEnumConverter = true)]
    public partial class GltfJsonContext : JsonSerializerContext
    {
        public static readonly GltfJsonContext Indented = new GltfJsonContext(new JsonSerializerOptions
        {
            WriteIndented = true
        });
        
    }
}