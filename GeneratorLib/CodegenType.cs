using System.CodeDom;

namespace KhronosGroup.Gltf.Generator
{
    public class CodegenType
    {
        public CodeTypeReference CodeType { get; set; }

        public CodeAttributeDeclarationCollection Attributes { get; } = new CodeAttributeDeclarationCollection();

        public CodeExpression DefaultValue { get; set; }

        public CodeTypeMemberCollection AdditionalMembers { get; } = new CodeTypeMemberCollection();

        public CodeStatementCollection SetStatements { get; } = new CodeStatementCollection();
    }
}
