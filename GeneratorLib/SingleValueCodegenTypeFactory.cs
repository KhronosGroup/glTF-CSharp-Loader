using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using KhronosGroup.Gltf.Generator.JsonSchema;

namespace KhronosGroup.Gltf.Generator
{
    public class SingleValueCodegenTypeFactory
    {
        public static CodegenType MakeCodegenType(string name, Schema schema)
        {
            CodegenType returnType = new CodegenType();
            EnforceRestrictionsOnSetValues(returnType, name, schema);

            if (schema.Format == "uriref")
            {
                switch (schema.UriType)
                {
                    case UriType.Application:
                    case UriType.Image:
                    case UriType.Text:
                        returnType.CodeType = new CodeTypeReference(typeof(string));
                        returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                        break;
                    case UriType.None:
                        throw new InvalidDataException("UriType must be specified in the schema");
                }

                return returnType;
            }

            if (schema.Type.Count > 1)
            {
                returnType.CodeType = new CodeTypeReference(typeof(object));
                returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                return returnType;
            }

            var typeRef = schema.Type[0];
            if (typeRef.IsReference)
            {
                throw new NotImplementedException();
            }

            if (typeRef.Name == "object")
            {
                if (schema.Enum != null || schema.HasDefaultValue())
                {
                    throw new NotImplementedException();
                }

                if (schema.Title != null)
                {
                    returnType.CodeType = new CodeTypeReference(Helpers.ParseTitle(schema.Title));
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                    return returnType;
                }
                throw new NotImplementedException();
            }

            if (typeRef.Name == "number")
            {
                if (schema.Enum != null)
                {
                    throw new NotImplementedException();
                }

                if (schema.HasDefaultValue())
                {
                    returnType.DefaultValue = new CodePrimitiveExpression((float)(double)schema.Default);
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, returnType.DefaultValue));
                }
                else if (!schema.IsRequired)
                {
                    returnType.CodeType = new CodeTypeReference(typeof(float?));
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                    return returnType;
                }

                returnType.CodeType = new CodeTypeReference(typeof(float));
                return returnType;
            }

            if (typeRef.Name == "string")
            {
                if (schema.Enum != null)
                {
                    if (schema.IsOpenStringEnum)
                    {
                        return MakeOpenStringEnumType(name, schema, returnType);
                    }

                    var enumType = GenStringEnumType(name, schema);
                    returnType.Attributes.Add(
                        new CodeAttributeDeclaration("System.Text.Json.Serialization.JsonConverter", 
                        new CodeAttributeArgument(new CodeTypeOfExpression($"System.Text.Json.Serialization.JsonStringEnumConverter<{enumType.Name}>"))));
                    returnType.AdditionalMembers.Add(enumType);

                    if (schema.HasDefaultValue())
                    {
                        returnType.CodeType = new CodeTypeReference(enumType.Name);
                        for (var i = 0; i < enumType.Members.Count; i++)
                        {
                            if (enumType.Members[i].Name == schema.Default.ToString())
                            {
                                returnType.DefaultValue = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(enumType.Name), (string)schema.Default);
                                returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, returnType.DefaultValue));
                                return returnType;
                            }
                        }
                        throw new InvalidDataException("The default value is not in the enum list");
                    }
                    else if (!schema.IsRequired)
                    {
                        returnType.CodeType = new CodeTypeReference(typeof(Nullable<>));
                        returnType.CodeType.TypeArguments.Add(new CodeTypeReference(enumType.Name));
                        returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                        return returnType;
                    }

                    returnType.CodeType = new CodeTypeReference(enumType.Name);
                    return returnType;
                }

                if (schema.HasDefaultValue())
                {
                    returnType.DefaultValue = new CodePrimitiveExpression((string)schema.Default);
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, returnType.DefaultValue));
                }
                else
                {
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                }
                returnType.CodeType = new CodeTypeReference(typeof(string));
                return returnType;
            }

            if (typeRef.Name == "integer")
            {
                if (schema.Enum != null)
                {
                    var enumType = GenIntEnumType(name, schema);
                    returnType.AdditionalMembers.Add(enumType);

                    if (schema.HasDefaultValue())
                    {
                        returnType.DefaultValue = GetEnumField(enumType, (int)(long)schema.Default);
                        returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, returnType.DefaultValue));
                    }
                    else if (!schema.IsRequired)
                    {
                        returnType.CodeType = new CodeTypeReference(typeof(Nullable<>));
                        returnType.CodeType.TypeArguments.Add(new CodeTypeReference(enumType.Name));
                        returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                        return returnType;
                    }

                    returnType.CodeType = new CodeTypeReference(enumType.Name);
                    return returnType;
                }

                if (schema.Default != null)
                {
                    returnType.DefaultValue = new CodePrimitiveExpression((int)(long)schema.Default);
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, returnType.DefaultValue));
                }
                else if (!schema.IsRequired)
                {
                    returnType.CodeType = new CodeTypeReference(typeof(int?));
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                    return returnType;
                }

                returnType.CodeType = new CodeTypeReference(typeof(int));
                return returnType;
            }

            if (typeRef.Name == "boolean")
            {
                if (schema.Enum != null)
                {
                    throw new NotImplementedException();
                }

                if (schema.Default != null)
                {
                    returnType.DefaultValue = new CodePrimitiveExpression((bool)schema.Default);
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, returnType.DefaultValue));
                }
                else if (!schema.IsRequired)
                {
                    returnType.CodeType = new CodeTypeReference(typeof(bool?));
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                    return returnType;
                }
                returnType.CodeType = new CodeTypeReference(typeof(bool));
                return returnType;
            }

            // other types: array, null

            throw new NotImplementedException(typeRef.Name);
        }

        private static void EnforceRestrictionsOnSetValues(CodegenType returnType, string name, Schema schema)
        {
            if (schema.Minimum != null)
            {
                returnType.SetStatements.Add(new CodeConditionStatement
                {
                    Condition = new CodeBinaryOperatorExpression
                    {
                        Left = new CodePropertySetValueReferenceExpression(),
                        Operator = schema.ExclusiveMinimum ? CodeBinaryOperatorType.LessThanOrEqual : CodeBinaryOperatorType.LessThan,
                        Right = new CodePrimitiveExpression(schema.Minimum)
                    },
                    TrueStatements =
                    {
                        new CodeThrowExceptionStatement
                        {
                            ToThrow = new CodeObjectCreateExpression
                            {
                                CreateType = new CodeTypeReference(typeof(ArgumentOutOfRangeException)),
                                Parameters =
                                {
                                    new CodePrimitiveExpression(name),
                                    new CodePropertySetValueReferenceExpression(),
                                    new CodePrimitiveExpression(
                                        schema.ExclusiveMinimum ?
                                            $"Expected value to be greater than {schema.Minimum}" :
                                            $"Expected value to be greater than or equal to {schema.Minimum}")
                                }
                            }
                        }
                    }
                });
            }

            if (schema.Maximum != null)
            {
                returnType.SetStatements.Add(new CodeConditionStatement
                {
                    Condition = new CodeBinaryOperatorExpression
                    {
                        Left = new CodePropertySetValueReferenceExpression(),
                        Operator = schema.ExclusiveMaximum ? CodeBinaryOperatorType.GreaterThanOrEqual : CodeBinaryOperatorType.GreaterThan,
                        Right = new CodePrimitiveExpression(schema.Maximum)
                    },
                    TrueStatements =
                    {
                        new CodeThrowExceptionStatement
                        {
                            ToThrow = new CodeObjectCreateExpression
                            {
                                CreateType = new CodeTypeReference(typeof(ArgumentOutOfRangeException)),
                                Parameters =
                                {
                                    new CodePrimitiveExpression(name),
                                    new CodePropertySetValueReferenceExpression(),
                                    new CodePrimitiveExpression(
                                        schema.ExclusiveMaximum ?
                                        $"Expected value to be less than {schema.Maximum}" :
                                        $"Expected value to be less than or equal to {schema.Maximum}")
                                }
                            }
                        }
                    }
                });
            }

            if (schema.MinItems != null)
            {
                throw new NotImplementedException();
            }

            if (schema.MinLength != null)
            {
                throw new NotImplementedException();
            }

            if (schema.MaxItems != null)
            {
                throw new NotImplementedException();
            }

            if (schema.MaxLength != null)
            {
                throw new NotImplementedException();
            }
        }

        public static CodeTypeDeclaration GenStringEnumType(string name, Schema schema)
        {
            var enumName = $"{name}Enum";
            var enumType = new CodeTypeDeclaration()
            {
                IsEnum = true,
                Attributes = MemberAttributes.Public,
                Name = enumName
            };

            foreach (var value in schema.Enum)
            {
                if (((string)value).Contains('/'))
                {
                    //[EnumMember(Value = "image/jpeg")]
                    //image_jpeg,
                    string newValue = Regex.Replace(value.ToString(), "/", "_");
                    CodeMemberField field = new CodeMemberField(enumName, newValue);

                    CodeAttributeDeclaration attribute = new CodeAttributeDeclaration("System.Text.Json.Serialization.JsonStringEnumMemberName",
                        new CodeAttributeArgument(new CodePrimitiveExpression(value)));
                    field.CustomAttributes = new CodeAttributeDeclarationCollection
                    {
                        attribute
                    };
                    enumType.Members.Add(field);
                }
                else
                {
                    enumType.Members.Add(new CodeMemberField(enumName, (string)value));
                }
            }

            return enumType;
        }

        // Open string enums (schemas whose anyOf permits an arbitrary string in addition to the
        // known constants) are generated as a "smart enum" struct instead of a strict C# enum.
        // The struct exposes the known values as static readonly fields but round-trips any string,
        // so values introduced by extensions (e.g. KHR_animation_pointer's "pointer" or
        // EXT_texture_webp's "image/webp") load and save without error.
        private static CodegenType MakeOpenStringEnumType(string name, Schema schema, CodegenType returnType)
        {
            var structName = $"{name}Enum";
            returnType.AdditionalMembers.Add(GenStringEnumStruct(structName, schema));

            if (schema.HasDefaultValue())
            {
                returnType.CodeType = new CodeTypeReference(structName);
                returnType.DefaultValue = new CodeFieldReferenceExpression(
                    new CodeTypeReferenceExpression(structName), SanitizeEnumMemberName((string)schema.Default));
                returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, returnType.DefaultValue));
                return returnType;
            }

            if (!schema.IsRequired)
            {
                returnType.CodeType = new CodeTypeReference(typeof(Nullable<>));
                returnType.CodeType.TypeArguments.Add(new CodeTypeReference(structName));
                returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                return returnType;
            }

            returnType.CodeType = new CodeTypeReference(structName);
            return returnType;
        }

        private static string SanitizeEnumMemberName(string value)
        {
            return value.Contains('/') ? Regex.Replace(value, "/", "_") : value;
        }

        public static CodeTypeDeclaration GenStringEnumStruct(string structName, Schema schema)
        {
            var converterName = $"{structName}Converter";

            var structType = new CodeTypeDeclaration(structName)
            {
                IsStruct = true,
                Attributes = MemberAttributes.Public,
            };
            structType.CustomAttributes.Add(new CodeAttributeDeclaration(
                "System.Text.Json.Serialization.JsonConverter",
                new CodeAttributeArgument(new CodeTypeOfExpression(converterName))));
            structType.BaseTypes.Add(new CodeTypeReference($"System.IEquatable<{structName}>"));

            var body = new StringBuilder();
            body.AppendLine("private readonly string m_value;");
            body.AppendLine();
            body.AppendLine($"public {structName}(string value) {{");
            body.AppendLine("    this.m_value = value;");
            body.AppendLine("}");
            body.AppendLine();
            foreach (var value in schema.Enum)
            {
                var memberName = SanitizeEnumMemberName((string)value);
                body.AppendLine($"public static readonly {structName} {memberName} = new {structName}(\"{value}\");");
            }
            body.AppendLine();
            body.AppendLine("public string Value {");
            body.AppendLine("    get { return this.m_value; }");
            body.AppendLine("}");
            body.AppendLine();
            body.AppendLine("public override string ToString() {");
            body.AppendLine("    return this.m_value;");
            body.AppendLine("}");
            body.AppendLine();
            body.AppendLine($"public bool Equals({structName} other) {{");
            body.AppendLine("    return string.Equals(this.m_value, other.m_value, System.StringComparison.Ordinal);");
            body.AppendLine("}");
            body.AppendLine();
            body.AppendLine("public override bool Equals(object obj) {");
            body.AppendLine($"    return ((obj is {structName}) && this.Equals((({structName})(obj))));");
            body.AppendLine("}");
            body.AppendLine();
            body.AppendLine("public override int GetHashCode() {");
            body.AppendLine("    return ((this.m_value == null) ? 0 : this.m_value.GetHashCode());");
            body.AppendLine("}");
            body.AppendLine();
            body.AppendLine($"public static bool operator ==({structName} left, {structName} right) {{");
            body.AppendLine("    return left.Equals(right);");
            body.AppendLine("}");
            body.AppendLine();
            body.AppendLine($"public static bool operator !=({structName} left, {structName} right) {{");
            body.AppendLine("    return (left.Equals(right) == false);");
            body.AppendLine("}");
            body.AppendLine();
            body.AppendLine($"public static implicit operator string({structName} value) {{");
            body.AppendLine("    return value.m_value;");
            body.AppendLine("}");
            body.AppendLine();
            body.AppendLine($"public static implicit operator {structName}(string value) {{");
            body.AppendLine($"    return new {structName}(value);");
            body.AppendLine("}");
            body.AppendLine();
            body.AppendLine($"public class {converterName} : System.Text.Json.Serialization.JsonConverter<{structName}> {{");
            body.AppendLine($"    public override {structName} Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options) {{");
            body.AppendLine("        if (reader.TokenType != System.Text.Json.JsonTokenType.String) {");
            body.AppendLine($"            throw new System.Text.Json.JsonException(\"Expected a string value for {structName}.\");");
            body.AppendLine("        }");
            body.AppendLine($"        return new {structName}(reader.GetString());");
            body.AppendLine("    }");
            body.AppendLine($"    public override void Write(System.Text.Json.Utf8JsonWriter writer, {structName} value, System.Text.Json.JsonSerializerOptions options) {{");
            body.AppendLine("        writer.WriteStringValue(value.m_value);");
            body.AppendLine("    }");
            body.AppendLine("}");

            structType.Members.Add(new CodeSnippetTypeMember(body.ToString()));
            return structType;
        }

        public static CodeTypeDeclaration GenIntEnumType(string name, Schema schema)
        {
            var enumName = $"{name}Enum";
            var enumType = new CodeTypeDeclaration()
            {
                IsEnum = true,
                Attributes = MemberAttributes.Public,
                Name = enumName
            };

            if (schema.EnumNames == null || (schema.Enum).Count != schema.EnumNames.Count)
            {
                throw new InvalidOperationException("Enum names must be defined for each integer enum");
            }

            foreach (var index in Enumerable.Range(0, schema.EnumNames.Count))
            {
                var value = (int)schema.Enum[index];
                enumType.Members.Add(new CodeMemberField(enumName, schema.EnumNames[index])
                {
                    InitExpression = new CodePrimitiveExpression(value)
                });
            }

            return enumType;
        }

        public static CodeFieldReferenceExpression GetEnumField(CodeTypeDeclaration enumType, int value)
        {
            var defaultMember = enumType.Members.Cast<CodeMemberField>().FirstOrDefault(m => (int)((CodePrimitiveExpression)m.InitExpression).Value == value);

            if (defaultMember == null)
            {
                throw new InvalidDataException("The default value is not in the enum list");
            }

            return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(enumType.Name), defaultMember.Name);
        }
    }
}
