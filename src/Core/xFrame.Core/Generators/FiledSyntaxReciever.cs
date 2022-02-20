using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xFrame.Core.Generators
{
    internal class FieldSyntaxReciever : ISyntaxContextReceiver
    {
        public List<IFieldSymbol> IdentifiedFields { get; } = new List<IFieldSymbol>();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is FieldDeclarationSyntax fieldDeclaration && fieldDeclaration.AttributeLists.Any())
            {
                var variableDeclaration = fieldDeclaration.Declaration.Variables;
                foreach (var variable in variableDeclaration)
                {
                    var field = context.SemanticModel.GetDeclaredSymbol(variable);
                    if (field is IFieldSymbol fieldInfo && fieldInfo.GetAttributes().Any(x => x.AttributeClass.ToDisplayString() == "xFrame.Core.Attributes.GeneratepropertyAttribute"))
                    {
                        IdentifiedFields.Add(fieldInfo);
                    }
                }

            }
        }
    }
}
