using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeneratorLib;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace GeneratorUnitTests
{
    [TestFixture]
    public class GetCodegenTypeTests
    {
        [Test]
        public void SchemaReferenceTypeNotNullException()
        {
            Schema schemaHasReferenceType = new Schema()
            {
                ReferenceType = "schema.json"
            };
            Assert.Throws<InvalidOperationException>(() => CodegenTypeFactory.MakeCodegenType("name", schemaHasReferenceType));
        }

        [Test]
        public void SchemaIsNotValidDictionaryException()
        {
            Schema schema = new Schema
            {
                AdditionalProperties = new Schema(),
                Type = null
            };
            Assert.Throws<InvalidOperationException>(() => CodegenTypeFactory.MakeCodegenType("NoTypeDictionary", schema));
            var typeRef = new TypeReference();
            schema.Type = new[] { typeRef };
            Assert.Throws<InvalidOperationException>(() => CodegenTypeFactory.MakeCodegenType("NoTypeDictionary", schema));
            typeRef.Name = "string";
            Assert.Throws<InvalidOperationException>(() => CodegenTypeFactory.MakeCodegenType("NoTypeDictionary", schema));
        }

        [Test]
        public void DictionaryValueTypeMultiTypeTest()
        {
            Schema schema = new Schema();
            var typeRef = new TypeReference()
            {
                Name = "object"
            };
            schema.Type = new[] { typeRef };
            schema.AdditionalProperties = new Schema()
            {
                Type = new[] { new TypeReference(), new TypeReference(), new TypeReference() }
            };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "MultiDictionaryValueTypeType", out CodeAttributeDeclarationCollection attributes,
                out CodeExpression defaultValue);
            Assert.IsTrue(result.BaseType.Contains("Dictionary"));
            Assert.AreEqual(typeof(string).ToString(), result.TypeArguments[0].BaseType);
            Assert.AreEqual(typeof(object).ToString(), result.TypeArguments[1].BaseType);
        }

        [Test]
        public void DictionaryValueTypeHasDefaultException()
        {
            Schema schema = new Schema();
            var typeRef = new TypeReference()
            {
                Name = "object"
            };
            schema.Type = new[] { typeRef };
            schema.AdditionalProperties = new Schema()
            {
                Type = new[] { typeRef }
            };
            schema.Default = JObject.Parse(@"{""default"":""defalut""}");
            Assert.Throws<NotImplementedException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "HasDefaultDictionaryValue", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }

        [Test]
        public void DictionaryValueTypeIsObjectTitleIsNull()
        {
            Schema schema = new Schema();
            var typeRef = new TypeReference()
            {
                Name = "object"
            };
            schema.Type = new[] { typeRef };
            schema.AdditionalProperties = new Schema()
            {
                Type = new[] { typeRef }
            };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "DictionaryValueTypeIsObject", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.IsTrue(result.BaseType.Contains("Dictionary"));
            Assert.AreEqual(typeof(string).ToString(), result.TypeArguments[0].BaseType);
            Assert.AreEqual(typeof(object).ToString(), result.TypeArguments[1].BaseType);
        }

        [Test]
        public void DictionaryValueTypeIsObjectIsAnotherClass()
        {
            Schema schema = new Schema();
            var typeRef = new TypeReference()
            {
                Name = "object"
            };
            schema.Type = new[] { typeRef };
            schema.AdditionalProperties = new Schema()
            {
                Title = "Asset",
                Type = new[] { typeRef }
            };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "DictionaryValueTypeIsObject", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual("System.Collections.Generic.Dictionary<string, Asset>", result.BaseType);
        }

        [Test]
        public void DictionaryOfStringsTest()
        {
            Schema schema = new Schema();
            var typeRef = new TypeReference()
            {
                Name = "object"
            };
            schema.Type = new[] { typeRef };
            schema.AdditionalProperties = new Schema();
            var valueTypeRef = new TypeReference();
            schema.AdditionalProperties.Type = new[] { valueTypeRef };
            valueTypeRef.Name = "string";
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "DictionaryValueTypeIsString", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.IsTrue(result.BaseType.Contains("Dictionary"));
            Assert.AreEqual(typeof(string).ToString(), result.TypeArguments[0].BaseType);
            Assert.AreEqual(typeof(string).ToString(), result.TypeArguments[1].BaseType);
        }

        [Test]
        public void SchemaHasNoTypeException()
        {
            Schema schemaNoType = new Schema()
            {
                AdditionalProperties = null,
                Type = null
            };
            Assert.Throws<InvalidOperationException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schemaNoType, "NoType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }

        [Test]
        public void SingleObjectMultiTypeReturnObject()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "object"
            };
            var typeRef2 = new TypeReference()
            {
                Name = "string"
            };
            schema.Type = new TypeReference[] { typeRef1, typeRef2 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectMultiTypeReturnObject", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual(typeof(object).ToString(), result.BaseType);
        }

        [Test]
        public void SingleObjectAnyTypeReturnObject()
        {
            // "any" isn't a valid type in json schema draft 4
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "any"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            Assert.Throws<NotImplementedException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectAnyTypeReturnObject", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }

        [Test]
        public void SingleObjectIsReferenceTypeException()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                IsReference = true
            };
            schema.Type = new TypeReference[] { typeRef1 };
            Assert.Throws<NotImplementedException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "IsReferenceType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }

        [Test]
        public void SingleObjectCustomerTypeTest()
        {
            Schema schema = new Schema()
            {
                Title = "Animation"
            };
            var typeRef1 = new TypeReference()
            {
                Name = "object"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectCustomerType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual("Animation", result.BaseType);
        }

        [Test]
        public void SingleObjectCustomerTypeNoTitleException()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "object"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            Assert.Throws<NotImplementedException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectCustomerTypeNoTitle", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }

        [Test]
        public void SingleObjectFloatTypeNoDefaultTest()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "number"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectFloatType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual(typeof(float).ToString(), result.TypeArguments[0].BaseType);
        }

        [Test]
        public void SingleObjectFloatTypeHasDefaultTest()
        {
            Schema schema = new Schema()
            {
                Default = 0.1
            };
            var typeRef1 = new TypeReference()
            {
                Name = "number"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectFloatType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual(typeof(float).ToString(), result.BaseType);
            Assert.AreEqual((float)(double)schema.Default, (float)((CodePrimitiveExpression)defaultValue).Value);
        }

        [Test]
        public void SingleObjectStringTypeNoDefaultNoEnumTest()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "string"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectStringType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual(typeof(string).ToString(), result.BaseType);
        }

        [Test]
        public void SingleObjectStringTypeHasDefaultNoEnumTest()
        {
            Schema schema = new Schema()
            {
                Default = "Empty"
            };
            var typeRef1 = new TypeReference()
            {
                Name = "string"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectStringType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual(typeof(string).ToString(), result.BaseType);
            Assert.AreEqual(schema.Default, ((CodePrimitiveExpression)defaultValue).Value);
        }

        [Test]
        public void SingleObjectStringTypeNoDefaultHasEnumTest()
        {
            Schema schema = new Schema();
            var expectedResult = new string[] { "One", "Two", "Three" };
            schema.Enum = new List<object>(expectedResult);
            CodeTypeDeclaration target = new CodeTypeDeclaration();
            var typeRef1 = new TypeReference()
            {
                Name = "string"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(target, schema, "SingleObjectStringType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual("System.Nullable`1", result.BaseType);
            Assert.AreEqual("SingleObjectStringTypeEnum", result.TypeArguments[0].BaseType);
            var members = new CodeTypeMember[3];
            ((CodeTypeDeclaration)target.Members[0]).Members.CopyTo(members, 0);
            CollectionAssert.AreEquivalent(expectedResult, members.Select((m) => m.Name));
        }

        [Test]
        public void SingleObjectStringTypeHasDefaultHasEnumTest()
        {
            Schema schema = new Schema()
            {
                Default = "One"
            };
            var expectedResult = new string[] { "One", "Two", "Three" };
            schema.Enum = new List<object>(expectedResult);
            CodeTypeDeclaration target = new CodeTypeDeclaration();
            var typeRef1 = new TypeReference()
            {
                Name = "string"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(target, schema, "SingleObjectStringType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual("SingleObjectStringTypeEnum", result.BaseType);
            var members = new CodeTypeMember[3];
            ((CodeTypeDeclaration)target.Members[0]).Members.CopyTo(members, 0);
            CollectionAssert.AreEquivalent(expectedResult, members.Select((m) => m.Name));
            Assert.AreEqual(schema.Default, ((CodeFieldReferenceExpression)defaultValue).FieldName);
        }

        [Test]
        public void SingleObjectStringTypeInvalidDefaultHasEnumException()
        {
            Schema schema = new Schema()
            {
                Default = "Four"
            };
            var expectedResult = new string[] { "One", "Two", "Three" };
            schema.Enum = new List<object>(expectedResult);
            CodeTypeDeclaration target = new CodeTypeDeclaration();
            var typeRef1 = new TypeReference()
            {
                Name = "string"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            Assert.Throws<InvalidDataException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "InvalidDefaultValue", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }

        [Test]
        public void SingleObjectIntegerTypeNoDefaultTest()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "integer"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectIntegerType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual(typeof(int).ToString(), result.TypeArguments[0].BaseType);
        }

        [Test]
        public void SingleObjectIntegerTypeHasDefaultTest()
        {
            Schema schema = new Schema()
            {
                Default = 1L
            };
            var typeRef1 = new TypeReference()
            {
                Name = "integer"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectIntegerType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual(typeof(int).ToString(), result.BaseType);
            Assert.AreEqual((int)(long)schema.Default, ((CodePrimitiveExpression)defaultValue).Value);
        }
        
        [Test]
        public void SingleObjectIntegerTypeNoDefaultHasEnumTest()
        {
            Schema schema = new Schema();
            var enumValues = new int[] { 1, 2, 3 };
            schema.Enum = new List<object>(enumValues.Cast<object>().ToList());
            var expectedResult = new string[] { "one", "two", "three" };
            schema.EnumNames = new List<string>(expectedResult);
            CodeTypeDeclaration target = new CodeTypeDeclaration();
            var typeRef1 = new TypeReference()
            {
                Name = "integer"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(target, schema, "SingleObjectIntegerType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual("System.Nullable`1", result.BaseType);
            Assert.AreEqual("SingleObjectIntegerTypeEnum", result.TypeArguments[0].BaseType);
            var members = new CodeTypeMember[3];
            ((CodeTypeDeclaration)target.Members[0]).Members.CopyTo(members, 0);
            CollectionAssert.AreEquivalent(expectedResult, members.Select((m) => m.Name));
        }

        [Test]
        public void SingleObjectIntegerTypeHasDefaultHasEnumTest()
        {
            Schema schema = new Schema()
            {
                Default = 5L
            };
            var enumValues = new int[] { 4, 5, 6 };
            schema.Enum = new List<object>(enumValues.Cast<object>().ToList());
            var expectedResult = new string[] { "four", "five", "six" };
            schema.EnumNames = new List<string>(expectedResult);
            CodeTypeDeclaration target = new CodeTypeDeclaration();
            var typeRef1 = new TypeReference()
            {
                Name = "integer"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(target, schema, "SingleObjectIntegerType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual("SingleObjectIntegerTypeEnum", result.BaseType);
            var members = new CodeTypeMember[3];
            ((CodeTypeDeclaration)target.Members[0]).Members.CopyTo(members, 0);
            CollectionAssert.AreEquivalent(expectedResult, members.Select((m) => m.Name));
            Assert.AreEqual("five", ((CodeFieldReferenceExpression)defaultValue).FieldName);
        }

        [Test]
        public void SingleObjectIntegerTypeInvalidDefaultHasEnumException()
        {
            Schema schema = new Schema()
            {
                Default = 4L
            };
            var expectedResult = new int[] { 7, 8, 9 };
            schema.Enum = new List<object>(expectedResult.Cast<object>().ToList());
            var enumNames = new string[] { "seven", "eight", "nine" };
            schema.EnumNames = new List<string>(enumNames);
            CodeTypeDeclaration target = new CodeTypeDeclaration();
            var typeRef1 = new TypeReference()
            {
                Name = "integer"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            Assert.Throws<InvalidDataException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "InvalidDefaultValue", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }
        
        [Test]
        public void SingleObjectBoolTypeNoDefaultTest()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "boolean"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectBoolType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual(typeof(bool).ToString(), result.TypeArguments[0].BaseType);
        }

        [Test]
        public void SingleObjectBoolTypeHasDefaultTest()
        {
            Schema schema = new Schema()
            {
                Default = false
            };
            var typeRef1 = new TypeReference()
            {
                Name = "boolean"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "SingleObjectBoolType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.AreEqual(typeof(bool).ToString(), result.BaseType);
            Assert.AreEqual(schema.Default, ((CodePrimitiveExpression)defaultValue).Value);
        }

        [Test]
        public void ArrayItemIsNullException()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            Assert.Throws<InvalidOperationException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "ArrayItemIsNullException", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }

        [Test]
        public void ArrayItemTypeIsNullException()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            Assert.Throws<InvalidOperationException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "ArrayItemTypeIsNullException", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }

        [Test]
        public void ArrayItemsIsAnArrayTest()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "integer"
            };
            schema.Items.Type = new TypeReference[] { typeRef2, typeRef2, typeRef2 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "ArrayItemIsArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.NotNull(result.ArrayElementType);
            Assert.AreEqual(typeof(object).ToString(), result.ArrayElementType.BaseType);
        }

        [Test]
        public void ArrayItemsIsBooleanNoDefaultTest()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "boolean"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "BooleanArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.NotNull(result.ArrayElementType);
            Assert.AreEqual(typeof(bool).ToString(), result.ArrayElementType.BaseType);
        }

        [Test]
        public void ArrayItemsIsBooleanHasDefaultTest()
        {
            var expectedValues = new[] { true, true, false };
            Schema schema = new Schema
            {
                Default = new JArray(expectedValues),
                Type = new[] { new TypeReference { Name = "array" } },
                Items = new Schema
                {
                    Type = new[] { new TypeReference { Name = "boolean" } }
                }
            };

            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "BooleanArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.NotNull(result.ArrayElementType);
            Assert.AreEqual(typeof(bool).ToString(), result.ArrayElementType.BaseType);
            var resultValues = (((CodeArrayCreateExpression)defaultValue).Initializers.Cast<CodePrimitiveExpression>()).Select(a => (bool)(a.Value));
            CollectionAssert.AreEqual(expectedValues, resultValues);
        }

        [Test]
        public void ArrayItemsIsStringNoDefaultTest()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "string"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "StringArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.NotNull(result.ArrayElementType);
            Assert.AreEqual(typeof(string).ToString(), result.ArrayElementType.BaseType);
        }

        [Test]
        public void ArrayItemsIsStringHasDefaultTest()
        {
            Schema schema = new Schema()
            {
                Default = new JArray(new string[] { "One", "Two", "Three" })
            };
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "string"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "StringArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.NotNull(result.ArrayElementType);
            Assert.AreEqual(typeof(string).ToString(), result.ArrayElementType.BaseType);
            Assert.AreEqual("One", ((CodePrimitiveExpression)(((CodeArrayCreateExpression)defaultValue).Initializers[0])).Value);
            Assert.AreEqual("Two", ((CodePrimitiveExpression)(((CodeArrayCreateExpression)defaultValue).Initializers[1])).Value);
            Assert.AreEqual("Three", ((CodePrimitiveExpression)(((CodeArrayCreateExpression)defaultValue).Initializers[2])).Value);
        }

        [Test]
        public void ArrayItemsIsIntegerNoDefaultTest()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "integer"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "IntegerArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.NotNull(result.ArrayElementType);
            Assert.AreEqual(typeof(int).ToString(), result.ArrayElementType.BaseType);
        }

        [Test]
        public void ArrayItemsIsIntegerHasDefaultTest()
        {
            Schema schema = new Schema()
            {
                Default = new JArray(new int[] { 1, 3, 5 })
            };
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "integer"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "IntegerArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.NotNull(result.ArrayElementType);
            Assert.AreEqual(typeof(int).ToString(), result.ArrayElementType.BaseType);
            Assert.AreEqual(1, ((CodePrimitiveExpression)(((CodeArrayCreateExpression)defaultValue).Initializers[0])).Value);
            Assert.AreEqual(3, ((CodePrimitiveExpression)(((CodeArrayCreateExpression)defaultValue).Initializers[1])).Value);
            Assert.AreEqual(5, ((CodePrimitiveExpression)(((CodeArrayCreateExpression)defaultValue).Initializers[2])).Value);
        }

        [Test]
        public void ArrayItemsIsFloatNoDefaultTest()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "number"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "FloatArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.NotNull(result.ArrayElementType);
            Assert.AreEqual(typeof(float).ToString(), result.ArrayElementType.BaseType);
        }

        [Test]
        public void ArrayItemsIsFloatHasDefaultTest()
        {
            Schema schema = new Schema()
            {
                Default = new JArray(new float[] { 1.1f, 3.3f, 5.5f })
            };
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "number"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "FloatArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.NotNull(result.ArrayElementType);
            Assert.AreEqual(typeof(float).ToString(), result.ArrayElementType.BaseType);
            Assert.AreEqual(1.1f, ((CodePrimitiveExpression)(((CodeArrayCreateExpression)defaultValue).Initializers[0])).Value);
            Assert.AreEqual(3.3f, ((CodePrimitiveExpression)(((CodeArrayCreateExpression)defaultValue).Initializers[1])).Value);
            Assert.AreEqual(5.5f, ((CodePrimitiveExpression)(((CodeArrayCreateExpression)defaultValue).Initializers[2])).Value);
        }

        [Test]
        public void ArrayItemsIsObjectNoDefaultTest()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "object"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "ObjectArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.NotNull(result.ArrayElementType);
            Assert.AreEqual(typeof(object).ToString(), result.ArrayElementType.BaseType);
        }

        [Test]
        public void ArrayItemsIsObjectHasDefaultTest()
        {
            Schema schema = new Schema()
            {
                Default = new JArray(new object[] { "Hello", 3.3f, 5 })
            };
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "object"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            Assert.Throws<NotImplementedException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "ObjectArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }

        [Test]
        public void ArrayItemsIsDictionaryTest()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "object"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            schema.Items.AdditionalProperties = new Schema();
            var typeRef3 = new TypeReference()
            {
                Name = "integer"
            };
            schema.Items.AdditionalProperties.Type = new TypeReference[] { typeRef3 };

            var result = CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "ArrayItemsIsDictionary", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue);
            Assert.NotNull(result.ArrayElementType);

            Assert.IsTrue(result.BaseType.Contains("Dictionary"));
            Assert.AreEqual(typeof(string).ToString(), result.TypeArguments[0].BaseType);
            Assert.AreEqual(typeof(int).ToString(), result.TypeArguments[1].BaseType);

            Assert.IsTrue(result.ArrayElementType.BaseType.Contains("Dictionary"));
            Assert.AreEqual(typeof(string).ToString(), result.ArrayElementType.TypeArguments[0].BaseType);
            Assert.AreEqual(typeof(int).ToString(), result.ArrayElementType.TypeArguments[1].BaseType);
        }

        [Test]
        public void ArrayItemsTypeNotImplementedTypeException()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "array"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "random"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            Assert.Throws<NotImplementedException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "NotImplementedArray", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }

        [Test]
        public void UnhandledSchemaTypeException()
        {
            Schema schema = new Schema();
            var typeRef1 = new TypeReference()
            {
                Name = "random"
            };
            schema.Type = new TypeReference[] { typeRef1 };
            schema.Items = new Schema();
            var typeRef2 = new TypeReference()
            {
                Name = "integer"
            };
            schema.Items.Type = new TypeReference[] { typeRef2 };
            Assert.Throws<NotImplementedException>(() => CodeGenerator.GetCodegenType(new CodeTypeDeclaration(), schema, "UnhandledSchemaType", out CodeAttributeDeclarationCollection attributes, out CodeExpression defaultValue));
        }
    }
}
