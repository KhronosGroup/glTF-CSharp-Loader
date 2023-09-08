using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KhronosGroup.Gltf.Generator.JsonSchema
{
    public class SchemaLoader
    {
        private readonly string m_directory;
        private readonly string m_rootSchemaName;

        public SchemaLoader(string rootSchemaFilePath)
        {
            rootSchemaFilePath = Path.GetFullPath(rootSchemaFilePath);
            m_directory = Path.GetDirectoryName(rootSchemaFilePath);
            m_rootSchemaName = Path.GetFileName(rootSchemaFilePath);
        }
        public Dictionary<string, Schema> FileSchemas { get; private set; }
        public void ParseSchemas()
        {
            FileSchemas = new SchemaParser(m_directory).ParseSchemaTree(m_rootSchemaName);
        }
        public void ExpandSchemaReferences()
        {
            ExpandSchemaReferences(FileSchemas[m_rootSchemaName]);
        }

        private void ExpandSchemaReferences(Schema schema)
        {
            foreach (var typeReference in new TypeReferenceEnumerator(schema))
            {
                if (typeReference.IsReference)
                {
                    ExpandSchemaReferences(FileSchemas[typeReference.Name]);
                }
            }

            if (schema.Properties != null)
            {
                var keys = schema.Properties.Keys.ToArray();
                foreach (var key in keys)
                {
                    if (!string.IsNullOrEmpty(schema.Properties[key].ReferenceType))
                    {
                        schema.Properties[key] = FileSchemas[schema.Properties[key].ReferenceType];
                    }

                    ExpandSchemaReferences(schema.Properties[key]);
                }
            }

            if (schema.AdditionalProperties != null)
            {
                if (!string.IsNullOrEmpty(schema.AdditionalProperties.ReferenceType))
                {
                    schema.AdditionalProperties = FileSchemas[schema.AdditionalProperties.ReferenceType];
                }

                ExpandSchemaReferences(schema.AdditionalProperties);
            }

            if (schema.Items != null)
            {
                if (!string.IsNullOrEmpty(schema.Items.ReferenceType))
                {
                    schema.Items = FileSchemas[schema.Items.ReferenceType];
                }

                ExpandSchemaReferences(schema.Items);
            }
        }
        public void EvaluateInheritance()
        {
            EvaluateInheritance(FileSchemas[m_rootSchemaName]);
        }

        private void EvaluateInheritance(Schema schema)
        {
            foreach (var subSchema in new SchemaEnumerator(schema))
            {
                EvaluateInheritance(subSchema);
            }

            foreach (var typeReference in new TypeReferenceEnumerator(schema))
            {
                if (typeReference.IsReference)
                {
                    EvaluateInheritance(FileSchemas[typeReference.Name]);
                }
            }

            if (schema.AllOf == null) return;

            foreach (var typeRef in schema.AllOf)
            {
                var baseType = FileSchemas[typeRef.Name];

                if (schema.Properties != null && baseType.Properties != null)
                {
                    foreach (var property in baseType.Properties)
                    {
                        if (schema.Properties.TryGetValue(property.Key, out Schema value))
                        {
                            if (value.IsEmpty())
                            {
                                schema.Properties[property.Key] = property.Value;
                                schema.Properties[property.Key].WasAlreadyPopulatedFromBase = true;
                            }
                            else if (!value.WasAlreadyPopulatedFromBase)
                            {
                                throw new InvalidOperationException("Attempting to overwrite non-Default schema.");
                            }
                        }
                        else
                        {
                            schema.Properties.Add(property.Key, property.Value);
                        }
                    }
                }

                foreach (var property in baseType.GetType().GetProperties())
                {
                    if (!property.CanRead || !property.CanWrite) continue;

                    if (property.GetValue(schema) == null)
                    {
                        property.SetValue(schema, property.GetValue(baseType));
                    }
                }
            }

            // needed for inheritance generation..
            //schema.AllOf = null;
        }
        public void PostProcessSchema()
        {
            SetDefaults();
            SetExclusiveMinMax();
            EvaluateEnums();
            SetRequired();
        }

        private void SetDefaults()
        {
            foreach (var schema in FileSchemas.Values)
            {
                if (schema.Type == null)
                {
                    schema.Type = new[] { new TypeReference { IsReference = false, Name = "object" } };
                }
            }
        }

        private void SetExclusiveMinMax()
        {
            foreach (var schema in FileSchemas.Values)
            {
                if (schema.Properties != null)
                {
                    foreach (var property in schema.Properties)
                    {
                        var propertySchema = property.Value;

                        // Exclusive Min
                        if (propertySchema.RawExclusiveMinimum is bool)
                        {
                            // JSON schema draft-4, the old schema
                            propertySchema.ExclusiveMinimum = Convert.ToBoolean(
                                propertySchema.RawExclusiveMinimum);
                        }
                        else if (propertySchema.RawExclusiveMinimum is double ||
                                 propertySchema.RawExclusiveMinimum is long)
                        {
                            // JSON schema draft 2020-12, the new schema
                            propertySchema.Minimum = propertySchema.RawExclusiveMinimum;
                            propertySchema.ExclusiveMinimum = true;
                        }

                        // Exclusive Max
                        if (propertySchema.RawExclusiveMaximum is bool)
                        {
                            // JSON schema draft-4, the old schema
                            propertySchema.ExclusiveMaximum = Convert.ToBoolean(
                                propertySchema.RawExclusiveMaximum);
                        }
                        else if (propertySchema.RawExclusiveMaximum is double ||
                                 propertySchema.RawExclusiveMaximum is long)
                        {
                            // JSON schema draft 2020-12, the new schema
                            propertySchema.Maximum = propertySchema.RawExclusiveMaximum;
                            propertySchema.ExclusiveMaximum = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// In glTF 2.0 an enumeration is defined by a property that contains
        /// the "anyOf" object that contains an array containing multiple
        /// "const" objects and a single "type" object.
        ///
        ///   {
        ///     "properties" : {
        ///       "mimeType" : {
        ///         "anyOf" : [
        ///           { "const" : "image/jpeg" },
        ///           { "const" : "image/png" },
        ///           { "type" : "string" }
        ///         ]
        ///       }
        ///     }
        ///   }
        ///
        /// So if the property does not have a "type" object and it has an
        /// "anyOf" object, assume it is an enum and attempt to set the
        /// appropriate schema properties.
        /// </summary>
        private void EvaluateEnums()
        {
            foreach (var schema in FileSchemas.Values)
            {
                if (schema.Properties != null)
                {
                    foreach (var property in schema.Properties)
                    {
                        if (!(property.Value.Type?.Count >= 1))
                        {
                            if (property.Value.AnyOf?.Count > 0)
                            {
                                // Set the type of the enum
                                property.Value.SetTypeFromAnyOf();

                                // Populate the values of the enum
                                property.Value.SetValuesFromAnyOf();
                            }
                        }
                    }
                }
            }
        }

        private void SetRequired()
        {
            foreach (var schema in FileSchemas.Values)
            {
                if (schema.Required != null && schema.Required.Count > 0)
                {
                    foreach (var prop in schema.Required)
                    {
                        schema.Properties[prop].IsRequired = true;
                    }
                }
            }
        }
    }
}
