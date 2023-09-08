using KhronosGroup.Gltf.Generator.JsonSchema;

using Microsoft.CSharp;

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KhronosGroup.Gltf.Generator
{
    public class CodeGenerator
    {
        public Dictionary<string, Schema> FileSchemas { get; }
        public Dictionary<string, CodeTypeDeclaration> GeneratedClasses { get; } = new Dictionary<string, CodeTypeDeclaration>();
        
        public CodeGenerator(Dictionary<string, Schema> fileSchemas)
        {
            FileSchemas = fileSchemas;
        }
        
        private static bool IsGeneratableType(Schema schema)
        {
            return schema.Type != null && schema.Type[0].Name == "object";
        }
        
        private IEnumerable<KeyValuePair<string, Schema>> GetImplementedProperties(Schema schema)
        {
            if (schema.Properties == null) yield break;
            foreach (var propertyPair in schema.Properties)
            {
                yield return propertyPair;
            }
        }

        private CodeCompileUnit BuildClass(string fileName, out string className)
        {
            var root = FileSchemas[fileName];
            var schemaNamespace = new CodeNamespace("glTFLoader.Schema");
            schemaNamespace.Imports.Add(new CodeNamespaceImport("System.Linq"));
            schemaNamespace.Imports.Add(new CodeNamespaceImport("System.Runtime.Serialization"));

            className = Helpers.ParseTitle(root.Title);

            var schemaClass = new CodeTypeDeclaration(className)
            {
                Attributes = MemberAttributes.Public
            };

            if (root.AllOf != null)
            {
                foreach (var typeRef in root.AllOf)
                {
                    if (typeRef.IsReference)
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            
            foreach (var property in GetImplementedProperties(root))
            {
                AddProperty(schemaClass, property.Key, property.Value);
            }

            GeneratedClasses[fileName] = schemaClass;
            schemaNamespace.Types.Add(schemaClass);
            var schemaFile = new CodeCompileUnit();
            schemaFile.Namespaces.Add(schemaNamespace);
            return schemaFile;
        }

        private void AddProperty(CodeTypeDeclaration target, string rawName, Schema valueSchema)
        {
            var propertyName = Helpers.ParsePropertyName(rawName);
            var fieldName = Helpers.GetFieldName(propertyName);
            var codegenType = CodegenTypeFactory.MakeCodegenType(rawName, valueSchema);
            target.Members.AddRange(codegenType.AdditionalMembers);

            var propertyBackingVariable = new CodeMemberField
            {
                Type = codegenType.CodeType,
                Name = fieldName,
                Comments = { new CodeCommentStatement("<summary>", true), new CodeCommentStatement($"Backing field for {propertyName}.", true), new CodeCommentStatement("</summary>", true) },
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
                Name = propertyName,
                // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                HasGet = true,
                GetStatements = { new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)) },
                HasSet = true,
                Comments = { new CodeCommentStatement("<summary>", true), new CodeCommentStatement(valueSchema.Description, true), new CodeCommentStatement("</summary>", true) },
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

        private CodeCompileUnit BuildJsonTypeModifier()
        {
            var ns = new CodeNamespace("glTFLoader");
            ns.Imports.Add(new CodeNamespaceImport("glTFLoader.Schema"));
            
            var method = new CodeMemberMethod
            {
                // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = "TypeInfoModifier",
                Parameters = { new CodeParameterDeclarationExpression("System.Text.Json.Serialization.Metadata.JsonTypeInfo", "info") },
                ReturnType = new CodeTypeReference("partial void")
            };
            method.Statements.AddRange(BuildJsonTypeModifierStatements().ToArray());
            
            ns.Types.Add(new CodeTypeDeclaration
            {
                Name = "GltfJsonContext",
                IsPartial = true,
                Members = { method }
            });
            
            var schemaFile = new CodeCompileUnit();
            schemaFile.Namespaces.Add(ns);
            return schemaFile;
        }
        
        private IEnumerable<CodeStatement> BuildJsonTypeModifierStatements()
        {
            foreach (var pair in FileSchemas)
            {
                var schema = pair.Value;
                if (!IsGeneratableType(schema)) continue;
                
                var typeName = Helpers.ParseTitle(schema.Title);
                
                var caseStatements = new CodeStatementCollection();
                foreach (var property in GetImplementedProperties(schema))
                {
                    var shouldSerializeMethodName = $"ShouldSerialize{Helpers.ParsePropertyName(property.Key)}";
                    if (GeneratedClasses[pair.Key].Members.OfType<CodeMemberMethod>().All(x => x.Name != shouldSerializeMethodName)) continue;
                    caseStatements.Add(new CodeSnippetStatement($"        \"{property.Key}\" => static (inst, _) => (({typeName})inst).ShouldSerialize{Helpers.ParsePropertyName(property.Key)}(),"));
                }
                if (caseStatements.Count == 0) continue;
                
                var branchStatements = new CodeStatementCollection
                {
                    new CodeSnippetStatement("foreach (var propertyInfo in info.Properties)"),
                    new CodeSnippetStatement("{"),
                    new CodeSnippetStatement("    propertyInfo.ShouldSerialize ??= propertyInfo.Name switch"),
                    new CodeSnippetStatement("    {"),
                };
                branchStatements.AddRange(caseStatements);
                branchStatements.AddRange(new CodeStatement[]
                {
                    new CodeSnippetStatement("        _ => null"), // default
                    new CodeSnippetStatement("    };"),
                    new CodeSnippetStatement("}")
                });
                
                foreach (CodeStatement statement in branchStatements)
                {
                    if (statement is CodeSnippetStatement snippetStatement)
                    {
                        // oopsie... need to add our own indentation
                        snippetStatement.Value = $"                {snippetStatement.Value}";
                    }
                }
                
                var condition = new CodeConditionStatement
                {
                    Condition = new CodeSnippetExpression($"typeof({typeName}).IsAssignableFrom(info.Type)")
                };
                condition.TrueStatements.AddRange(branchStatements);
                yield return condition;
            }
        } 

        public void CSharpCodeGen(string outputDirectory)
        {
            // make sure the output directory exists
            Directory.CreateDirectory(outputDirectory);
            
            foreach (var schema in FileSchemas)
            {
                if (!IsGeneratableType(schema.Value)) continue;
                WriteClass(schema.Key, outputDirectory);
            }
            
            WriteClass(BuildJsonTypeModifier(), "GltfJsonContext.g.cs", Path.GetDirectoryName(outputDirectory));
        }

        private void WriteClass(string fileName, string outputDirectory)
        {
            var schemaFile = BuildClass(fileName, out var className);
            WriteClass(schemaFile, className, outputDirectory);
        }
        
        private static void WriteClass(CodeCompileUnit cu, string fileName, string outputDirectory)
        {
            var sourceFile = Path.Combine(outputDirectory, fileName + ".cs");

            var writer = new IndentedTextWriter(new StreamWriter(sourceFile, false), "    ");
            var provider = new CSharpCodeProvider();
            provider.GenerateCodeFromCompileUnit(cu, writer, new CodeGeneratorOptions());
            writer.Close();
        }
    }
}
