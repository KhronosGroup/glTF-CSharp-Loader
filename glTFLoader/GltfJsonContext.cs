using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using glTFLoader.Schema;

namespace glTFLoader
{
    public partial class GltfJsonContext : IJsonTypeInfoResolver
    {
        public static partial void TypeInfoModifier(JsonTypeInfo info); // (defined in generated part)
        public static readonly IJsonTypeInfoResolver TypeInfoResolver = BaseGltfJsonContext.Default.WithAddedModifier(TypeInfoModifier);
        
        public static readonly GltfJsonContext Default = new GltfJsonContext(new JsonSerializerOptions());
        public static readonly GltfJsonContext Indented = new GltfJsonContext(new JsonSerializerOptions
        {
            WriteIndented = true
        });

        private readonly JsonSerializerOptions _options;
        private JsonTypeInfo<Gltf> _gltf;
        public JsonTypeInfo<Gltf> Gltf => _gltf ??= (JsonTypeInfo<Gltf>)_options.GetTypeInfo(typeof(Gltf));

        public GltfJsonContext(JsonSerializerOptions options)
        {
            _options = options;
            _options.TypeInfoResolver ??= TypeInfoResolver;
            _options.MakeReadOnly();
        }

        public JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            return options.GetTypeInfo(type);
        }
    }
}