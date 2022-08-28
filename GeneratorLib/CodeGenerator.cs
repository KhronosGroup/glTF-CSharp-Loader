using KhronosGroup.Gltf.Generator.Schema;

using Microsoft.CSharp;

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

namespace GeneratorLib
{
    public class CodeGenerator
    {

        public CodeGenerator(Dictionary<string, Schema> fileSchemas)
            => FileSchemas = fileSchemas;
        public Dictionary<string, Schema> FileSchemas { get; }
        public Dictionary<string, CodeTypeDeclaration> GeneratedClasses { get; set; }

        public CodeCompileUnit RawClass(string fileName, out string className)
        {
            var root = FileSchemas[fileName];
            var schemaFile = new CodeCompileUnit();
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

            if (root.Properties != null)
            {
                foreach (var property in root.Properties)
                {
                    AddProperty(schemaClass, property.Key, property.Value);
                }
            }

            GeneratedClasses[fileName] = schemaClass;
            schemaNamespace.Types.Add(schemaClass);
            schemaFile.Namespaces.Add(schemaNamespace);
            return schemaFile;
        }

        private void AddProperty(CodeTypeDeclaration target, string rawName, Schema schema)
        {
            var propertyName = Helpers.ParsePropertyName(rawName);
            var fieldName = Helpers.GetFieldName(propertyName);
            var codegenType = CodegenTypeFactory.MakeCodegenType(rawName, schema);
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
            // make sure the output directory exists
            System.IO.Directory.CreateDirectory(outputDirectory);

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
