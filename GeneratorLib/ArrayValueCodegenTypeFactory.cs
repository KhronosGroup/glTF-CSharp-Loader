using System;
using System.CodeDom;
using System.Linq;
using glTFLoader.Shared;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using KhronosGroup.Gltf.Generator.Schema;

namespace GeneratorLib
{
    public class ArrayValueCodegenTypeFactory
    {
        public static CodegenType MakeCodegenType(string name, Schema schema)
        {
            if (!(schema.Items?.Type?.Count > 0))
            {
                throw new InvalidOperationException("Array type must contain an item type");
            }

            if (schema.Enum != null)
            {
                throw new InvalidOperationException();
            }

            var returnType = new CodegenType();
            returnType.Attributes.Add(new CodeAttributeDeclaration("Newtonsoft.Json.JsonConverterAttribute", new[] { new CodeAttributeArgument(new CodeTypeOfExpression(typeof(ArrayConverter))) }));

            if (schema.Items.Type.Count > 1)
            {
                returnType.CodeType = new CodeTypeReference(typeof(object[]));
                returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                return returnType;
            }

            EnforceRestrictionsOnSetValues(returnType, name, schema);

            if (schema.Items.Type[0].Name == "integer")
            {
                if (schema.Items.Enum != null)
                {
                    throw new NotImplementedException();
                }

                if (schema.HasDefaultValue())
                {
                    var defaultValueArray =
                        ((JArray)schema.Default).Select(
                            x => (CodeExpression)new CodePrimitiveExpression((int)(long)x)).ToArray();
                    returnType.DefaultValue = new CodeArrayCreateExpression(typeof(int), defaultValueArray);
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheArrayOfValueOfAMemberIsNotEqualToAnotherExpression(name, returnType.DefaultValue));
                }
                else
                {
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                }
                returnType.CodeType = new CodeTypeReference(typeof(int[]));

                return returnType;
            }

            if (schema.Items.Enum != null)
            {
                throw new NotImplementedException();
            }

            if (schema.Items.Type[0].Name == "number")
            {
                if (schema.HasDefaultValue())
                {
                    var defaultVauleArray = (JArray)schema.Default;
                    returnType.DefaultValue = new CodeArrayCreateExpression(typeof(float),
                        defaultVauleArray.Select(x => (CodeExpression)new CodePrimitiveExpression((float)x)).ToArray());
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheArrayOfValueOfAMemberIsNotEqualToAnotherExpression(name, returnType.DefaultValue));
                }
                else
                {
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                }

                returnType.CodeType = new CodeTypeReference(typeof(float[]));

                return returnType;
            }

            if (schema.Items.Minimum != null || schema.Items.Maximum != null)
            {
                throw new NotImplementedException();
            }

            if (schema.Items.Type[0].Name == "boolean")
            {
                if (schema.HasDefaultValue())
                {
                    var defaultVauleArray = (JArray)schema.Default;
                    returnType.DefaultValue = new CodeArrayCreateExpression(typeof(bool),
                        defaultVauleArray.Select(x => (CodeExpression)new CodePrimitiveExpression((bool)x)).ToArray());
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheArrayOfValueOfAMemberIsNotEqualToAnotherExpression(name, returnType.DefaultValue));
                }
                else
                {
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                }
                returnType.CodeType = new CodeTypeReference(typeof(bool[]));
                return returnType;
            }

            if (schema.Items.Type[0].Name == "string")
            {
                if (schema.HasDefaultValue())
                {
                    var defaultVauleArray = (JArray)schema.Default;
                    returnType.DefaultValue = new CodeArrayCreateExpression(typeof(string),
                        defaultVauleArray.Select(x => (CodeExpression)new CodePrimitiveExpression((string)x))
                            .ToArray());
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheArrayOfValueOfAMemberIsNotEqualToAnotherExpression(name, returnType.DefaultValue));
                }
                else
                {
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                }
                returnType.CodeType = new CodeTypeReference(typeof(string[]));

                return returnType;
            }

            if (schema.Items.Type[0].Name == "object")
            {
                if (schema.HasDefaultValue())
                {
                    throw new NotImplementedException("Array of Objects has default value");
                }

                if (schema.Items.Title != null)
                {
                    returnType.CodeType = new CodeTypeReference(Helpers.ParseTitle(schema.Items.Title) + "[]");
                    returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                    returnType.Attributes.Clear();

                    if (schema.Items.MinLength != null || schema.Items.MaxLength != null)
                    {
                        throw new NotImplementedException();
                    }

                    return returnType;
                }

                if (schema.Items.AdditionalProperties != null)
                {
                    if (schema.Items.AdditionalProperties.Type[0].Name == "integer")
                    {
                        returnType.CodeType = new CodeTypeReference(typeof(Dictionary<string, int>[]));
                        returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                        returnType.Attributes.Clear();

                        return returnType;
                    }

                    throw new NotImplementedException($"Dictionary<string,{schema.Items.AdditionalProperties.Type[0].Name}> not yet implemented.");
                }

                returnType.CodeType = new CodeTypeReference(typeof(object[]));
                returnType.AdditionalMembers.Add(Helpers.CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(name, new CodePrimitiveExpression(null)));
                return returnType;
            }

            throw new NotImplementedException("Array of " + schema.Items.Type[0].Name);
        }

        private static void EnforceRestrictionsOnSetValues(CodegenType returnType, string name, Schema schema)
        {
            if (!schema.HasDefaultValue())
            {
                var fieldName = Helpers.GetFieldName(name);
                returnType.SetStatements.Add(new CodeConditionStatement
                {
                    Condition = new CodeBinaryOperatorExpression
                    {
                        Left = new CodePropertySetValueReferenceExpression(),
                        Operator = CodeBinaryOperatorType.ValueEquality,
                        Right = new CodePrimitiveExpression(null)
                    },
                    TrueStatements =
                    {
                        new CodeAssignStatement()
                        {
                            Left = new CodeFieldReferenceExpression
                            {
                                FieldName = fieldName,
                                TargetObject = new CodeThisReferenceExpression()
                            },
                            Right = new CodePropertySetValueReferenceExpression()
                        },
                        new CodeMethodReturnStatement()
                    }
                });
            }

            if (schema.MinItems != null)
            {
                returnType.SetStatements.Add(new CodeConditionStatement
                {
                    Condition = new CodeBinaryOperatorExpression
                    {
                        Left = new CodePropertyReferenceExpression(new CodePropertySetValueReferenceExpression(), "Length"),
                        Operator = CodeBinaryOperatorType.LessThan,
                        Right = new CodePrimitiveExpression(schema.MinItems)
                    },
                    TrueStatements =
                    {
                        new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(ArgumentException), new CodeExpression[] { new CodePrimitiveExpression("Array not long enough") } ))
                    }
                });
            }

            if (schema.MaxItems != null)
            {
                returnType.SetStatements.Add(new CodeConditionStatement
                {
                    Condition = new CodeBinaryOperatorExpression
                    {
                        Left = new CodePropertyReferenceExpression(new CodePropertySetValueReferenceExpression(), "Length"),
                        Operator = CodeBinaryOperatorType.GreaterThan,
                        Right = new CodePrimitiveExpression(schema.MaxItems)
                    },
                    TrueStatements =
                    {
                        new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(ArgumentException), new CodeExpression[] { new CodePrimitiveExpression("Array too long") } ))
                    }
                });
            }

            if (schema.Items.Minimum != null || schema.Items.Maximum != null || schema.Items.MinLength != null || schema.Items.MaxLength != null)
            {
                returnType.SetStatements.Add(new CodeVariableDeclarationStatement(typeof(int), "index", new CodePrimitiveExpression(0)));
            }

            if (schema.Items.Minimum != null)
            {
                returnType.SetStatements.Add(LoopThroughArray(new CodePropertySetValueReferenceExpression(), new CodeStatementCollection
                {
                    new CodeConditionStatement
                    {
                        Condition = new CodeBinaryOperatorExpression
                        {
                            Left = new CodeArrayIndexerExpression
                            {
                                TargetObject = new CodePropertySetValueReferenceExpression(),
                                Indices = { new CodeVariableReferenceExpression("index") },
                            },
                            Operator = schema.Items.ExclusiveMinimum ? CodeBinaryOperatorType.LessThanOrEqual : CodeBinaryOperatorType.LessThan,
                            Right = new CodePrimitiveExpression(schema.Items.Minimum)
                        },
                        TrueStatements = { new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(ArgumentOutOfRangeException))) }
                    }
                }));
            }

            if (schema.Items.Maximum != null)
            {
                returnType.SetStatements.Add(LoopThroughArray(new CodePropertySetValueReferenceExpression(), new CodeStatementCollection
                {
                    new CodeConditionStatement
                    {
                        Condition = new CodeBinaryOperatorExpression
                        {
                            Left = new CodeArrayIndexerExpression
                            {
                                TargetObject = new CodePropertySetValueReferenceExpression(),
                                Indices = { new CodeVariableReferenceExpression("index") },
                            },
                            Operator = schema.Items.ExclusiveMaximum ? CodeBinaryOperatorType.GreaterThanOrEqual : CodeBinaryOperatorType.GreaterThan,
                            Right = new CodePrimitiveExpression(schema.Items.Maximum)
                        },
                        TrueStatements = { new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(ArgumentOutOfRangeException))) }
                    }
                }));
            }

            if (schema.Items.MinLength != null)
            {
                throw new NotImplementedException();
            }

            if (schema.Items.MaxLength != null)
            {
                throw new NotImplementedException();
            }
        }

        private static CodeStatement LoopThroughArray(CodeExpression array, CodeStatementCollection statements)
        {
            var returnValue = new CodeIterationStatement
            {
                InitStatement =
                    new CodeAssignStatement(new CodeVariableReferenceExpression("index"), new CodePrimitiveExpression(0)),
                TestExpression = new CodeBinaryOperatorExpression
                {
                    Left = new CodeVariableReferenceExpression("index"),
                    Operator = CodeBinaryOperatorType.LessThan,
                    Right = new CodePropertyReferenceExpression
                    {
                        PropertyName = "Length",
                        TargetObject = array
                    }
                },
                IncrementStatement = new CodeAssignStatement
                {
                    Left = new CodeVariableReferenceExpression("index"),
                    Right = new CodeBinaryOperatorExpression
                    {
                        Left = new CodeVariableReferenceExpression("index"),
                        Operator = CodeBinaryOperatorType.Add,
                        Right = new CodePrimitiveExpression(1)
                    }
                }
            };
            returnValue.Statements.AddRange(statements);
            return returnValue;
        }
    }
}
