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
        private INamedTypeSymbol notifyChangedInterface;
        private INamedTypeSymbol notifyChangingInterface;
        private INamedTypeSymbol callerMemberSymbol;

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is not FieldSyntaxReciever syntaxReciever) return;
            viewModelSymbol = context.Compilation.GetTypeByMetadataName("xFrame.Core.MVVM.ViewModelBase");
            notifyChangedInterface = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");
            notifyChangingInterface = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanging");
            callerMemberSymbol = context.Compilation.GetTypeByMetadataName("System.Runtime.CompilerServices.CallerMemberNameAttribute");
            foreach (var containingClassGroup in syntaxReciever.IdentifiedFields.GroupBy(x => x.ContainingType))
            {
                bool generatePropertyChangedInterface = false;
                bool generatePropertyChangingInterface = false;
                var containingClass = containingClassGroup.Key;
                if (!SymbolEqualityComparer.Default.Equals(containingClass.BaseType, viewModelSymbol)
                    && !containingClass.Interfaces.Contains(notifyChangedInterface))
                    generatePropertyChangedInterface = true;

                if (!SymbolEqualityComparer.Default.Equals(containingClass.BaseType, viewModelSymbol)
                    && !containingClass.Interfaces.Contains(notifyChangingInterface))
                    generatePropertyChangingInterface = true;


                var namespc = containingClass.ContainingNamespace;
                var source = GenerateClass(context, containingClass, namespc, containingClassGroup.ToList(), generatePropertyChangedInterface, generatePropertyChangingInterface);
                context.AddSource($"{containingClass.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new FieldSyntaxReciever());
        }

        private string GenerateClass(GeneratorExecutionContext context, INamedTypeSymbol @class, INamespaceSymbol @namespace, List<IFieldSymbol> fields, bool generateNotifyChangedInterface, bool generateNotifyChangingInterface)
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
                .AppendLine($"using {notifyChangingInterface.ContainingNamespace};\n")
                .AppendLine($"using {callerMemberSymbol.ContainingNamespace};\n");
            foreach (var name in namespaces)
            {
                classBuilder.AppendLine($"using {name};");
            }

            classBuilder.AppendLine($"namespace {@namespace.ToDisplayString()}");
            classBuilder.AppendLine("{");
            classBuilder.Tab.Append($"{@class.DeclaredAccessibility.ToString().ToLower()} partial class {@class.Name}")
                .AppendIf(generateNotifyChangedInterface, " : INotifyPropertyChanged")
                .AppendIf(generateNotifyChangedInterface && generateNotifyChangingInterface, ", ")
                .AppendIf(generateNotifyChangingInterface, "INotifyPropertyChanging");
            classBuilder.Tab.AppendLine("{");
            if (generateNotifyChangedInterface)
                AddNotifyPropertyChangedImplementation(classBuilder);

            if(generateNotifyChangingInterface)
                AddNotifyPropertieChangingImplemntation(classBuilder);

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
                    .AppendLine($"NotifyPropertyChanging();")
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

        private void AddNotifyPropertieChangingImplemntation(SourceBuilder source)
        {
            source.Append(@"public event PropertyChangingEventHandler PropertyChanging;
        protected virtual void NotifyPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }");
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
