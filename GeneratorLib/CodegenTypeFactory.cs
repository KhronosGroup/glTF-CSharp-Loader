using System;
using System.CodeDom;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeneratorLib
{
    public static class CodegenTypeFactory
    {
        public static CodegenType MakeCodegenType(string name, Schema schema)
        {
            var codegenType = InternalMakeCodegenType(Helpers.ParsePropertyName(name), schema);

            if (schema.Required?.Contains(name) ?? false)
            {
                codegenType.Attributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(JsonRequiredAttribute))));
            }

            codegenType.Attributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(JsonPropertyAttribute)), new []{ new CodeAttributeArgument(new CodePrimitiveExpression(name)) }));

            return codegenType;
        }

        private static CodegenType InternalMakeCodegenType(string name, Schema schema)
        {

            if (schema.Disallowed != null || schema.Pattern != null)
            {
                //throw new NotImplementedException();  // TODO implement this for glTF 2.0
            }

            if (schema.ReferenceType != null)
            {
                throw new InvalidOperationException("We don't support de-referencing here.");
            }

            // force extras to pretend to be an object
            if (schema.Title?.Equals("extras") ?? false)
            {
                TypeReference typeRef = new TypeReference();
                typeRef.IsReference = false;
                typeRef.Name = "object";
                schema.Type = new[] { typeRef };
            }

            if (!(schema.Type?.Length >= 1))
            {
                object newType = schema.AnyOfContainsType();
                if (newType != null)
                {
                    TypeReference typeRef = new TypeReference();
                    typeRef.IsReference = false;
                    typeRef.Name = newType.ToString();
                    schema.Type = new[] { typeRef };
                }
                else
                {
                    throw new InvalidOperationException("This Schema does not represent a type");
                }
            }

            if (schema.DictionaryValueType == null)
            {
                if (schema.Type.Length == 1 && !schema.Type[0].IsReference && schema.Type[0].Name == "array")
                {
                    return ArrayValueCodegenTypeFactory.MakeCodegenType(name, schema);
                }

                return SingleValueCodegenTypeFactory.MakeCodegenType(name, schema);
            }

            if (schema.Type.Length == 1 && schema.Type[0].Name == "object")
            {
                return MakeDictionaryType(name, schema);
            }

            throw new InvalidOperationException();
        }

        private static CodegenType MakeDictionaryType(string name, Schema schema)
        {
            var returnType = new CodegenType();

            if (schema.DictionaryValueType.Type.Length > 1)
            {
                returnType.CodeType = new CodeTypeReference(typeof(Dictionary<string, object>));
                returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                return returnType;
            }

            if (schema.HasDefaultValue())
            {
                throw new NotImplementedException("Defaults for dictionaries are not yet supported");
            }

            if (schema.DictionaryValueType.Type[0].Name == "object")
            {

                if (schema.DictionaryValueType.Title != null)
                {
                    returnType.CodeType = new CodeTypeReference($"System.Collections.Generic.Dictionary<string, {Helpers.ParseTitle(schema.DictionaryValueType.Title)}>");
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                    return returnType;
                }
                returnType.CodeType = new CodeTypeReference(typeof(Dictionary<string, object>));
                returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                return returnType;
            }

            if (schema.DictionaryValueType.Type[0].Name == "string")
            {
                returnType.CodeType = new CodeTypeReference(typeof(Dictionary<string, string>));
                returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                return returnType;
            }

            if (schema.DictionaryValueType.Type[0].Name == "integer")
            {
                returnType.CodeType = new CodeTypeReference(typeof(Dictionary<string, int>));
                returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                return returnType;
            }

            throw new NotImplementedException($"Dictionary<string,{schema.DictionaryValueType.Type[0].Name}> not yet implemented.");
        }
    }
}
