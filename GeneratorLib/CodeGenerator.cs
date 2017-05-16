using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CSharp;

namespace GeneratorLib
{
    public class CodeGenerator
    {
        private string m_directory;
        private string m_rootSchemaName;

        public CodeGenerator(string rootSchemaFilePath)
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

            if (schema.DictionaryValueType != null)
            {
                if (!string.IsNullOrEmpty(schema.DictionaryValueType.ReferenceType))
                {
                    schema.DictionaryValueType = FileSchemas[schema.DictionaryValueType.ReferenceType];
                }

                ExpandSchemaReferences(schema.DictionaryValueType);
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

            // var baseSchema = FileSchemas[schema.Extends.Name];
            // if (baseSchema.Type.Length == 1 && baseSchema.Type[0].Name == "object") return;

            // While technically a list, for glTF it only ever has one element
            var baseType = FileSchemas[schema.AllOf[0].Name];

            if (schema.Properties != null && baseType.Properties != null)
            {
                foreach (var property in baseType.Properties)
                {
                    if (schema.Properties.TryGetValue(property.Key, out Schema value))
                    {
                        if (value.IsEmpty())
                        {
                            schema.Properties[property.Key] = property.Value;
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

            schema.AllOf = null;
        }

        public void SetDefaults()
        {
            foreach (var schema in FileSchemas.Values)
            {
                if (schema.Type == null)
                {
                    schema.Type = new[] { new TypeReference { IsReference = false, Name = "object" } };
                }
            }
         }

        public void EvaluateEnums()
        {
            foreach (var schema in FileSchemas.Values)
            {
                if (schema.Properties != null)
                {
                    foreach (var property in schema.Properties)
                    {
                        if (!(property.Value.Type?.Count >= 1))
                        {
                            if (property.Value.AnyOf != null && property.Value.AnyOf.Count > 0)
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

        public Dictionary<string, CodeTypeDeclaration> GeneratedClasses { get; set; }

        public CodeCompileUnit RawClass(string fileName, out string className)
        {
            var root = FileSchemas[fileName];
            var schemaFile = new CodeCompileUnit();
            var schemaNamespace = new CodeNamespace("glTFLoader.Schema");
            schemaNamespace.Imports.Add(new CodeNamespaceImport("System.Linq"));

            className = Helpers.ParseTitle(root.Title);

            var schemaClass = new CodeTypeDeclaration(className)
            {
                Attributes = MemberAttributes.Public
            };

            // While technically a list, for glTF it only ever has one element
            if (root.AllOf != null && root.AllOf[0].IsReference)
            {
                schemaClass.BaseTypes.Add(Helpers.ParseTitle(FileSchemas[root.AllOf[0].Name].Title));
            }

            if (root.Properties != null)
            {
                foreach (var property in root.Properties)
                {
                    AddProperty(schemaClass, property.Key, property.Value);
                }
            }

            GeneratedClasses[fileName] = schemaClass;
            schemaNamespace.Types.Add(schemaClass);
            //new CodeAttributeDeclaration(new CodeTypeReference(new CodeTypeParameter()))
            schemaFile.Namespaces.Add(schemaNamespace);
            return schemaFile;
        }

        private void AddProperty(CodeTypeDeclaration target, string rawName, Schema schema)
        {
            var name = Helpers.ParsePropertyName(rawName);
            var fieldName = "m_" + name.Substring(0, 1).ToLower() + name.Substring(1);
            var codegenType = CodegenTypeFactory.MakeCodegenType(rawName, schema);
            target.Members.AddRange(codegenType.AdditionalMembers);
            
            var propertyBackingVariable = new CodeMemberField
            {
                Type = codegenType.CodeType,
                Name = fieldName,
                Comments = { new CodeCommentStatement("<summary>", true), new CodeCommentStatement($"Backing field for {name}.", true), new CodeCommentStatement("</summary>", true) },
                InitExpression = codegenType.DefaultValue
            };

            target.Members.Add(propertyBackingVariable);

            var setStatements = codegenType.SetStatements ?? new CodeStatementCollection();
            setStatements.Add(new CodeAssignStatement()
            {
                Left = new CodeFieldReferenceExpression
                {
                    FieldName = fieldName,
                    TargetObject = new CodeThisReferenceExpression()
                },
                Right = new CodePropertySetValueReferenceExpression()
            });

            var property = new CodeMemberProperty
            {
                Type = codegenType.CodeType,
                Name = name,
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                HasGet = true,
                GetStatements = { new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)) },
                HasSet = true,
                Comments = { new CodeCommentStatement("<summary>", true), new CodeCommentStatement(schema.Description, true), new CodeCommentStatement("</summary>", true) },
                CustomAttributes = codegenType.Attributes
            };
            property.SetStatements.AddRange(setStatements);

            target.Members.Add(property);
        }

        public static CodeTypeReference GetCodegenType(CodeTypeDeclaration target, Schema schema, string name, out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue)
        {
            var codegenType = CodegenTypeFactory.MakeCodegenType(name, schema);
            attributes = codegenType.Attributes;
            defaultValue = codegenType.DefaultValue;
            target.Members.AddRange(codegenType.AdditionalMembers);

            return codegenType.CodeType;
        }

        public void CSharpCodeGen(string outputDirectory)
        {
            GeneratedClasses = new Dictionary<string, CodeTypeDeclaration>();
            foreach (var schema in FileSchemas)
            {
                if (schema.Value.Type != null && schema.Value.Type[0].Name == "object")
                {
                    CodeGenClass(schema.Key, outputDirectory);
                }
            }
        }

        private void CodeGenClass(string fileName, string outputDirectory)
        {
            var schemaFile = RawClass(fileName, out string className);
            CSharpCodeProvider csharpcodeprovider = new CSharpCodeProvider();
            var sourceFile = Path.Combine(outputDirectory, className + "." + csharpcodeprovider.FileExtension);

            IndentedTextWriter tw1 = new IndentedTextWriter(new StreamWriter(sourceFile, false), "    ");
            csharpcodeprovider.GenerateCodeFromCompileUnit(schemaFile, tw1, new CodeGeneratorOptions());
            tw1.Close();
        }
    }
}
