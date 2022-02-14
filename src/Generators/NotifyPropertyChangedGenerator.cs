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
        private INamedTypeSymbol viewModelSymbol;
        private INamedTypeSymbol notifyInterface;
        private INamedTypeSymbol callerMemberSymbol;

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is not FieldSyntaxReciever syntaxReciever) return;
            viewModelSymbol = context.Compilation.GetTypeByMetadataName("xFrame.Core.MVVM.ViewModelBase");
            notifyInterface = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");
            callerMemberSymbol = context.Compilation.GetTypeByMetadataName("System.Runtime.CompilerServices.CallerMemberNameAttribute");
            foreach (var containingClassGroup in syntaxReciever.IdentifiedFields.GroupBy(x => x.ContainingType))
            {
                bool generateInterface = false;
                var containingClass = containingClassGroup.Key;
                if (!SymbolEqualityComparer.Default.Equals(containingClass.BaseType, viewModelSymbol)
                    && !containingClass.Interfaces.Contains(notifyInterface))
                    generateInterface = true;

                    
                var namespc = containingClass.ContainingNamespace;
                var source = GenerateClass(context, containingClass, namespc, containingClassGroup.ToList(), generateInterface);
                context.AddSource($"{containingClass.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new FieldSyntaxReciever());
        }

        private string GenerateClass(GeneratorExecutionContext context, INamedTypeSymbol @class, INamespaceSymbol @namespace, List<IFieldSymbol> fields, bool generateInterface)
        {
            var builder = new StringBuilder();
            var classBuilder = new SourceBuilder(builder);
            classBuilder.AppendLine("using System;")
                .AppendIf(generateInterface, $"using {notifyInterface.ContainingNamespace};\n")
                .AppendIf(generateInterface, $"using {callerMemberSymbol.ContainingNamespace};\n");
            classBuilder.AppendLine($"namespace {@namespace.ToDisplayString()}");
            classBuilder.AppendLine("{");
            classBuilder.Tab.Append($"{@class.DeclaredAccessibility.ToString().ToLower()} partial class {@class.Name}")
                .AppendIf(generateInterface, " : INotifyPropertyChanged");
            classBuilder.Tab.AppendLine("{");
            if (generateInterface)
                AddNotifyInterfaceImplementation(classBuilder);
            // Iterate over the fields and create the properties
            foreach (var field in fields)
            {
                var fieldName = field.Name;
                var fieldType = field.Type.Name;
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

        private void AddNotifyInterfaceImplementation(SourceBuilder classBuilder)
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
