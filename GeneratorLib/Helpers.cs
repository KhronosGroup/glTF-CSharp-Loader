using System.CodeDom;

namespace GeneratorLib
{
    public static class Helpers
    {
        public static string GetFieldName(string name)
        {
            return "m_" + name.Substring(0, 1).ToLower() + name.Substring(1);
        }

        public static string ParsePropertyName(string rawName)
        {
            return rawName.Substring(0, 1).ToUpper() + rawName.Substring(1);
        }

        public static string ParseTitle(string rawTitle)
        {
            var words = rawTitle.ToLower().Split(' ');
            string retval = "";
            foreach (var word in words)
            {
                retval += word[0].ToString().ToUpper();
                retval += word.Substring(1);
            }
            return retval;
        }

        public static CodeMemberMethod CreateMethodThatChecksIfTheValueOfAMemberIsNotEqualToAnotherExpression(
           string name, CodeExpression expression)
        {
            return new CodeMemberMethod
            {
                ReturnType = new CodeTypeReference(typeof(bool)),
                Statements =
                {
                    new CodeMethodReturnStatement()
                    {
                        Expression = new CodeBinaryOperatorExpression()
                        {
                            Left = new CodeBinaryOperatorExpression()
                            {
                                Left = new CodeFieldReferenceExpression()
                                {
                                    FieldName = "m_" + name.Substring(0, 1).ToLower() + name.Substring(1)
                                },
                            Operator = CodeBinaryOperatorType.ValueEquality,
                            Right = expression
                            },
                            Operator = CodeBinaryOperatorType.ValueEquality,
                            Right = new CodePrimitiveExpression(false)
                        }
                    }
                },
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = "ShouldSerialize" + name
            };
        }

        public static CodeMemberMethod CreateMethodThatChecksIfTheArrayOfValueOfAMemberIsNotEqualToAnotherExpression(
           string name, CodeExpression expression)
        {
            return new CodeMemberMethod
            {
                ReturnType = new CodeTypeReference(typeof(bool)),
                Statements =
                {
                    new CodeMethodReturnStatement()
                    {
                        Expression = new CodeBinaryOperatorExpression()
                        {
                            Left = new CodeMethodInvokeExpression(
                                new CodeFieldReferenceExpression() {FieldName = "m_" + name.Substring(0, 1).ToLower() + name.Substring(1)},
                                "SequenceEqual",
                                new CodeExpression[] { expression}
                                )
                            ,
                            Operator = CodeBinaryOperatorType.ValueEquality,
                            Right = new CodePrimitiveExpression(false)
                        }
                    }
                },
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = "ShouldSerialize" + name
            };
        }
    }
}
