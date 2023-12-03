using System;
using System.Collections.Generic;
#if NET
using System.Diagnostics.CodeAnalysis;
#endif
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace glTFLoader
{
    // https://github.com/dotnet/runtime/issues/74385#issuecomment-1705083109
    
    public sealed class JsonStringEnumConverterWithEnumMemberAttrSupport<
#if NET
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] 
#endif
        TEnum> :JsonStringEnumConverter<TEnum> where TEnum : struct, Enum
    {
        public JsonStringEnumConverterWithEnumMemberAttrSupport() : base(namingPolicy: ResolveNamingPolicy())
        {
        }

        private static JsonNamingPolicy ResolveNamingPolicy()
        {
            var map = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => (f.Name, AttributeName: f.GetCustomAttribute<EnumMemberAttribute>()?.Value))
                .Where(pair => pair.AttributeName != null)
                .ToDictionary(x => x.Name, y => y.AttributeName);

            return map.Count > 0 ? new DictMappingNamingPolicy(map) : null;
        }

        private sealed class DictMappingNamingPolicy : JsonNamingPolicy
        {
            private readonly Dictionary<string, string> _map;
            
            public DictMappingNamingPolicy(Dictionary<string, string> map)
            {
                _map = map;
            }
            
            public override string ConvertName(string name)
                => _map.TryGetValue(name, out var newName) ? newName : name;
        }
    }
}