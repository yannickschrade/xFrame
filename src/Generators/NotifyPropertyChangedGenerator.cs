using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using xFrame.SourceGenerators.Helper;

namespace xFrame.SourceGenerators
{
    [Generator]
    public class NotifyPropertyChangedGenerator : ISourceGenerator
    {
        private INamedTypeSymbol notifyChangedInterface;
        private INamedTypeSymbol callerMemberSymbol;

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is not FieldSyntaxReciever syntaxReciever) return;
            notifyChangedInterface = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");
            callerMemberSymbol = context.Compilation.GetTypeByMetadataName("System.Runtime.CompilerServices.CallerMemberNameAttribute");
            foreach (var containingClassGroup in syntaxReciever.IdentifiedFields.GroupBy(x => x.ContainingType))
            {
                bool generatePropertyChangedInterface = true;
                var containingClass = containingClassGroup.Key;

                if(containingClass.AllInterfaces.Any(n => SymbolEqualityComparer.Default.Equals(n, notifyChangedInterface)))
                    generatePropertyChangedInterface = false;


                var namespc = containingClass.ContainingNamespace;
                var source = GenerateClass(context, containingClass, namespc, containingClassGroup.ToList(), generatePropertyChangedInterface);
                context.AddSource($"{containingClass.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new FieldSyntaxReciever());
        }

        private string GenerateClass(GeneratorExecutionContext context, INamedTypeSymbol @class, INamespaceSymbol @namespace, List<IFieldSymbol> fields, bool generateNotifyChangedInterface)
        {
            var builder = new StringBuilder();
            var classBuilder = new SourceBuilder(builder);
            var namespaces = new HashSet<string>();
            foreach (var field in fields)
            {
                var name = field.Type.ContainingNamespace.ToDisplayString();
                namespaces.Add(field.Type.ContainingNamespace.ToDisplayString());
            }
            classBuilder.AppendLine("using System;")
                .AppendLine($"using {callerMemberSymbol.ContainingNamespace};");
            foreach (var name in namespaces)
            {
                classBuilder.AppendLine($"using {name};");
            }

            classBuilder.AppendLine($"namespace {@namespace.ToDisplayString()}");
            classBuilder.AppendLine("{");
            classBuilder.Tab.Append($"{@class.DeclaredAccessibility.ToString().ToLower()} partial class {@class.Name}")
                .AppendIf(generateNotifyChangedInterface, " : INotifyPropertyChanged");
            classBuilder.Tab.AppendLine("{");
            if (generateNotifyChangedInterface)
                AddNotifyPropertyChangedImplementation(classBuilder);

            // Iterate over the fields and create the properties
            foreach (var field in fields)
            {
                var fieldName = field.Name;
                var fieldType = field.Type.ToDisplayString();
                classBuilder.AppendLine($"public {fieldType} {NormalizePropertyName(fieldName)}")
                    .AppendLine("{")
                    .AppendLine($"get=> {fieldName};")
                    .AppendLine($"set")
                    .AppendLine("{")
                    .AppendLine($"if({fieldName} == value) return;")
                    .AppendLine($"{fieldName} = value;")
                    .AppendLine("NotifyPropertyChanged();");
                classBuilder.AppendLine("}");
                classBuilder.AppendLine("}");
            }
            classBuilder.AppendLine("}");
            classBuilder.AppendLine("}");
            return builder.ToString();
        }

        private void AddNotifyPropertyChangedImplementation(SourceBuilder classBuilder)
        {
            classBuilder.Append(@"public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }").AppendLine();
        }

        private string NormalizePropertyName(string fieldName)
        {
            return Regex.Replace(fieldName, "_[a-z]", delegate (Match m)
            {
                return m.ToString().TrimStart('_').ToUpper();
            });
        }
    }


}
