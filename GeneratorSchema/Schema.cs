using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KhronosGroup.Gltf.Generator.JsonSchema
{
    public class SchemaEnumerator : IEnumerable<Schema>
    {
        private readonly Schema m_schema;

        public SchemaEnumerator(Schema schema)
        {
            m_schema = schema;
        }

        public IEnumerator<Schema> GetEnumerator()
        {
            return (m_schema.Properties != null ? m_schema.Properties.Values : Enumerable.Empty<Schema>())
                .Concat(m_schema.PatternProperties != null ? m_schema.PatternProperties.Values : Enumerable.Empty<Schema>())
                .Concat(m_schema.AdditionalItems != null ? new[] { m_schema.AdditionalItems } : Enumerable.Empty<Schema>())
                .Concat(m_schema.AdditionalProperties != null ? new[] { m_schema.AdditionalProperties } : Enumerable.Empty<Schema>())
                .Concat(m_schema.Items != null ? new[] { m_schema.Items } : Enumerable.Empty<Schema>())
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TypeReferenceEnumerator : IEnumerable<TypeReference>
    {
        private readonly Schema m_schema;

        public TypeReferenceEnumerator(Schema schema)
        {
            m_schema = schema;
        }

        public IEnumerator<TypeReference> GetEnumerator()
        {
            return (m_schema.Type ?? Enumerable.Empty<TypeReference>())
                .Concat(m_schema.AllOf ?? Enumerable.Empty<TypeReference>())
                .Concat(!string.IsNullOrWhiteSpace(m_schema.ReferenceType) ? new[] { new TypeReference() { IsReference = true, Name = m_schema.ReferenceType } } : Enumerable.Empty<TypeReference>())
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // Based on http://json-schema.org/latest/json-schema-validation.html#rfc.section.5 and http://json-schema.org/draft-04/schema
    public class Schema
    {
        public override string ToString() => ReferenceType ?? Title ?? Description;
        public Schema AdditionalItems { get; set; }

        // TODO implement this for glTF 2.0
        // Example: Dependencies: a: [ b ], c: [ b ]
        public Dictionary<string, IList<string>> Dependencies { get; set; }

        public object Default { get; set; }

        public string Description { get; set; }

        // TODO implement this for glTF 2.0
        [JsonProperty("gltf_detailedDescription")]
        public string DetailedDescription { get; set; }

        public Schema AdditionalProperties { get; set; }

        // TODO implement this for glTF 2.0
        // Used by Schema.Disallowed
        // Example: Not : AnyOf : [ required: [a, b], required: [a, c], required: [a, d] ]
        // TypeReferenceConverter
        public Schema Not { get; set; }

        // TODO implement this for glTF 2.0
        public uint? MultipleOf { get; set; }

        [JsonConverter(typeof(ArrayOfTypeReferencesConverter))]
        public IList<TypeReference> AllOf { get; set; }

        // Handled by CodeGenerator.EvaluteEnums
        public IList<IDictionary<string, object>> AnyOf { get; set; }

        // TODO implement this for glTF 2.0
        // Example: OneOf : [ required: a, required: b ]
        // TypeReferenceConverter
        public IList<IDictionary<string, IList<string>>> OneOf { get; set; }

        public IList<object> Enum { get; set; }

        [JsonProperty("gltf_enumNames")]
        public IList<string> EnumNames { get; set; }

        [JsonProperty("exclusiveMaximum")]
        public object RawExclusiveMaximum { get; set; }

        [JsonProperty("exclusiveMinimum")]
        public object RawExclusiveMinimum { get; set; }

        [JsonProperty("exclusiveMaximum_bool")]
        public bool ExclusiveMaximum { get; set; } = false;

        [JsonProperty("exclusiveMinimum_bool")]
        public bool ExclusiveMinimum { get; set; } = false;

        public string Format { get; set; }

        // Technically could be an array, but glTF only uses it for a schema
        public Schema Items { get; set; }

        public uint? MaxItems { get; set; }

        // Not used in glTF 2.0
        public uint? MaxLength { get; set; }

        // Not used in glTF 2.0
        public uint? MaxProperties { get; set; }

        public object Maximum { get; set; }

        public uint? MinItems { get; set; }

        // Not used in glTF 2.0
        public uint? MinLength { get; set; }

        // TODO implement this for glTF 2.0
        // Example: minProperties: 1
        public uint? MinProperties { get; set; }

        public object Minimum { get; set; }

        public Dictionary<string, Schema> PatternProperties { get; set; }

        // TODO implement this for glTF 2.0
        public string Pattern { get; set; }

        public Dictionary<string, Schema> Properties { get; set; }

        [JsonProperty("__ref__")]
        public string ReferenceType { get; set; }

        // Handled by CodeGenerator.SetRequired
        public IList<string> Required { get; set; }

        public string Title { get; set; }

        [JsonConverter(typeof(ArrayOfTypeReferencesConverter))]
        public IList<TypeReference> Type { get; set; }

        // TODO implement this for glTF 2.0
        // Example: UniqueItems: true
        public bool UniqueItems { get; set; } = false;

        [JsonProperty("gltf_uriType")]
        public UriType UriType { get; set; } = UriType.None;

        // TODO implement this for glTF 2.0
        [JsonProperty("gltf_webgl")]
        public string WebGl { get; set; }

        private static readonly Schema empty = new Schema();
        public bool IsEmpty()
        {
            return this.GetType().GetProperties().All(property => Object.Equals(property.GetValue(this), property.GetValue(empty)));
        }

        /// <summary>
        /// Json schema properties that contain an "anyOf" object are used as
        /// enumerations in glTF 2.0.
        ///
        /// This method searches the array of dictionaries in the "anyOf"
        /// object for the dictionary with the key "type" and extracts the type
        /// string from its value.
        /// </summary>
        public void SetTypeFromAnyOf()
        {
            foreach (var dict in this.AnyOf)
            {
                if (dict.ContainsKey("type"))
                {
                    if (this.Type == null)
                    {
                        this.Type = new List<TypeReference>();
                    }
                    this.Type.Add(new TypeReference { IsReference = false, Name = dict["type"].ToString() });
                    break;
                }
            }
        }

        /// <summary>
        /// Json schema properties that contain an "anyOf" object are used as
        /// enumerations in glTF 2.0.
        ///
        /// This method requires that the type of the enumeration has been set.
        ///
        /// For each dictionary in the "anyOf" array, this method extracts the
        /// list of enumerations and appends the first element of the "enum"
        /// array to the Schema enum list.
        ///
        /// Additionally, when the enumeration is of type integer, the
        /// "description" object value is appended to the Schema enum name list.
        /// </summary>
        public void SetValuesFromAnyOf()
        {
            if (this.Enum == null)
            {
                this.Enum = new List<object>();
            }
            if (this.Type?[0].Name == "integer" && this.EnumNames == null)
            {
                this.EnumNames = new List<string>();
            }

            foreach (var dict in this.AnyOf)
            {
                if (dict.ContainsKey("const"))
                {
                    // Newer glTF 2.0 schema (as of September 2021) uses "const" for enums.
                    var enumValue = dict["const"];
                    if (this.Type?[0].Name == "integer")
                    {
                        this.Enum.Add(Convert.ToInt32(enumValue));
                        this.EnumNames.Add(dict["description"].ToString());
                    }
                    else if (this.Type?[0].Name == "string")
                    {
                        this.Enum.Add(Convert.ToString(enumValue));
                    }
                    else
                    {
                        throw new NotImplementedException("Enum of " + this.Type?[0].Name);
                    }
                }
                else if (dict.ContainsKey("enum"))
                {
                    // Older glTF schemas use the enum keyword.
                    JArray enumList = dict["enum"] as JArray;
                    if (this.Type?[0].Name == "integer")
                    {
                        this.Enum.Add(enumList[0].ToObject<int>());
                        this.EnumNames.Add(dict["description"].ToString());
                    }
                    else if (this.Type?[0].Name == "string")
                    {
                        this.Enum.Add(enumList[0].ToObject<string>());
                    }
                    else
                    {
                        throw new NotImplementedException("Enum of " + this.Type?[0].Name);
                    }
                }
            }
        }

        public bool IsRequired { get; set; } = false;

        public bool HasDefaultValue()
        {
            return this.Default != null &&
                   (
                       (this.Default is JObject && ((JObject)this.Default).Count > 0) ||
                       (this.Default is JArray && ((JArray)this.Default).Count > 0) ||
                       (this.Default is int) ||
                       (this.Default is long) ||
                       (this.Default is float) ||
                       (this.Default is double) ||
                       (this.Default is string) ||
                       (this.Default is bool)
                   );
        }
    }

    public class TypeReference
    {
        public bool IsReference { get; set; } = false;

        public string Name { get; set; }
        public override string ToString() => Name;
    }

    public enum UriType
    {
        None,
        Application,
        Text,
        Image
    }
}
