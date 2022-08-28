using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GeneratorLib
{
    public class SchemaParser
    {
        private string m_directory;

        public SchemaParser(string rootDirectory)
        {
            m_directory = rootDirectory;
        }

        private Dictionary<string, Schema> FileSchemas { get; set; }

        public Dictionary<string, Schema> ParseSchemaTree(string rootFile)
        {
            FileSchemas = new Dictionary<string, Schema>();
            ParseSchema(rootFile);
            return FileSchemas;
        }

        private void ParseSchema(string schemaFile)
        {
            if (FileSchemas.ContainsKey(schemaFile))
            {
                return;
            }

            var schema = Deserialize(schemaFile);
            FileSchemas.Add(schemaFile, schema);
            ParseSchemasReferencedFromSchema(schema);
        }

        private void ParseSchemasReferencedFromSchema(Schema schema)
        {
            if (schema.Type != null)
            {
                foreach (var type in schema.Type)
                {
                    if (!type.IsReference) continue;

                    ParseSchema(type.Name);
                }
            }

            if (schema.Properties != null)
            {
                foreach (var property in schema.Properties)
                {
                    if (string.IsNullOrWhiteSpace(property.Value.ReferenceType))
                    {
                        ParseSchemasReferencedFromSchema(property.Value);
                        continue;
                    }

                    ParseSchema(property.Value.ReferenceType);
                }
            }

            if (schema.AllOf != null)
            {
                foreach (var typeRef in schema.AllOf)
                {
                    if (typeRef.IsReference)
                    {
                        ParseSchema(typeRef.Name);
                    }
                }
            }

            if (schema.AdditionalProperties != null)
            {
                if (!string.IsNullOrWhiteSpace(schema.AdditionalProperties.ReferenceType))
                {
                    ParseSchema(schema.AdditionalProperties.ReferenceType);
                }

                if (schema.AdditionalProperties.Type != null)
                {
                    foreach (var type in schema.AdditionalProperties.Type)
                    {
                        if (!type.IsReference) continue;

                        ParseSchema(type.Name);
                    }
                }
            }

            if (schema.Items != null)
            {
                if (!string.IsNullOrWhiteSpace(schema.Items.ReferenceType))
                {
                    ParseSchema(schema.Items.ReferenceType);
                }

                if (schema.Items.AdditionalProperties != null)
                {
                    ParseSchemasReferencedFromSchema(schema.Items);
                }
            }
        }

        private Schema Deserialize(string fileName)
        {
            return JsonConvert.DeserializeObject<Schema>(
                File.ReadAllText(Path.Combine(m_directory, fileName))
                    .Replace("\"$ref\"", "__ref__"));
        }
    }
}