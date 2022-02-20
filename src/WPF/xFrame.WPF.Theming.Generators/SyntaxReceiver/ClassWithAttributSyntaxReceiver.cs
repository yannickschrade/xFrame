using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xFrame.WPF.Theming.Generators.SyntaxReceiver
{
    internal class ClassWithAttributSyntaxReceiver<TAttribute> : ISyntaxContextReceiver
        where TAttribute : Attribute
    {
        private string typeName = typeof(TAttribute).FullName;
        public List<INamedTypeSymbol> MatchingClasses { get; } = new List<INamedTypeSymbol>();

        public ClassWithAttributSyntaxReceiver()
        {
        }

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {

            if(context.Node is ClassDeclarationSyntax cds && cds.AttributeLists.Any())
            {
                var cs = context.SemanticModel.GetDeclaredSymbol(cds);
                if(cs.GetAttributes().Any(a => a.AttributeClass.ToDisplayString() == typeName))
                {
                    MatchingClasses.Add(cs);
                }
                    
            }
        }
    }
}
