using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using xFrame.WPF.Theming.Generators.Attributes;
using xFrame.WPF.Theming.Generators.SyntaxReceiver;

namespace xFrame.WPF.Theming.Generators
{
    [Generator]
    public class ThemeTemplateGenerator : ISourceGenerator
    {
        private readonly string defaultValueAttributeName = typeof(DefaultValueAttribute).FullName;
        private readonly string lightenColorAttributeName = typeof(LightenColorAttribute).FullName;
        private readonly string darkenColorAttributeName = typeof(DarkenColorAttribute).FullName;
        private readonly List<(string, string)> _properties = new List<(string, string)>();
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is ClassWithAttributSyntaxReceiver<GenerateThemeTemplateAttribute> reciver))
                return;

            foreach (var @classSymbole in reciver.MatchingClasses)
            {
                _properties.Clear();
                var fields = classSymbole.GetMembers().Where(m => m is IFieldSymbol).Select(m => (IFieldSymbol)m).ToList();
                var mem = classSymbole.GetMembers();
                var templateSourceCode = new SourceStringBuilder();
                var keysSourceCode = new SourceStringBuilder();
                GenerateThemeTemplateClassHeader(templateSourceCode, classSymbole);
                GenerateThemeKeyClassHeader(keysSourceCode, classSymbole);

                foreach (var field in fields)
                {
                    GenerateProperty(templateSourceCode, field);
                }

                GenerateKeys(keysSourceCode);
                GenerateCtor(classSymbole, templateSourceCode);

                templateSourceCode.AppendCloseCurlyBracketLine();
                templateSourceCode.AppendCloseCurlyBracketLine();
                keysSourceCode.AppendCloseCurlyBracketLine();
                keysSourceCode.AppendCloseCurlyBracketLine();

                var templateSource = SourceText.From(templateSourceCode.ToString(), Encoding.UTF8);
                var keysSource = SourceText.From(keysSourceCode.ToString(), Encoding.UTF8);
                context.AddSource($"{classSymbole.Name}.g.cs", templateSourceCode.ToString());
                context.AddSource($"{GetTemplateClassName(classSymbole.Name)}Keys.g.cs", keysSource.ToString());
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{classSymbole.Name}.g.cs", templateSourceCode.ToString());
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{GetTemplateClassName(classSymbole.Name)}Keys.g.cs", keysSource.ToString());
            }
        }

        private void GenerateCtor(INamedTypeSymbol @class ,SourceStringBuilder source)
        {
            source.AppendLine()
                .AppendLine($"public {@class.Name}()")
                .AppendOpenCurlyBracketLine();
            
            foreach (var (type, name)  in _properties)
            {
                source.AppendLine($"Add(\"{name}\", {name});");
                
                if(type == "System.Windows.Media.Color")
                {
                    source.AppendLine($"Add(\"{name}Brush\", new SolidColorBrush({name}));");
                }
            }

            source.AppendCloseCurlyBracketLine();
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ClassWithAttributSyntaxReceiver<GenerateThemeTemplateAttribute>());
        }

        private void GenerateThemeTemplateClassHeader(SourceStringBuilder sourceCode, INamedTypeSymbol classSymbole)
        {
            sourceCode.AppendLine("using System.Windows.Media;")
                .AppendLine($"using xFrame.WPF.Theming;")
                .AppendLine($"using xFrame.WPF.Theming.ExtensionMethodes;")
                .AppendLine()
                .AppendLine($"namespace {classSymbole.ContainingNamespace}")
                .AppendOpenCurlyBracketLine()
                .AppendLine($"public abstract partial class {classSymbole.Name} : Theme")
                .AppendOpenCurlyBracketLine();
        }

        private void GenerateThemeKeyClassHeader(SourceStringBuilder keysSourceCode, INamedTypeSymbol classSymbole)
        {
            keysSourceCode.AppendLine("using System;")
                .AppendLine()
                .AppendLine($"namespace {classSymbole.ContainingNamespace}")
                .AppendOpenCurlyBracketLine()
                .AppendLine($"public static class {GetTemplateClassName(classSymbole.Name)}Keys")
                .AppendOpenCurlyBracketLine();
        }

        private void GenerateKeys(SourceStringBuilder sourceCode)
        {
            foreach (var (type, name) in _properties)
            {
                sourceCode.AppendLine($"public const string {name} = \"{name}\";");
                if (type != "System.Windows.Media.Color")
                    continue;

                sourceCode.AppendLine($"public const string {name}Brush = \"{name}Brush\";");
            }
            
        }

        private void GenerateProperty(SourceStringBuilder sourceCode, IFieldSymbol field)
        {

            var propertyName = NormalizePropertyName(field.Name);
            var fieldType = field.Type.ToString();
            var attributes = field.GetAttributes();
            var defaultValueAttribute = attributes.FirstOrDefault(a => a.AttributeClass.ToDisplayString() == defaultValueAttributeName);
            if (defaultValueAttribute != null)
            {

                var defaultvalue = defaultValueAttribute?.ConstructorArguments.FirstOrDefault().Value;
                sourceCode.AppendLine($"public virtual {fieldType} {propertyName} => {defaultvalue};");
            }
            else
            {
                sourceCode.AppendLine($"public abstract {fieldType} {propertyName} {{get;}}");
            }

            _properties.Add((fieldType, propertyName));

            if (field.Type.ToDisplayString() != "System.Windows.Media.Color")
                return;

            var lightenColorAttribute = attributes.FirstOrDefault(a => a.AttributeClass.ToDisplayString() == lightenColorAttributeName);
            var darkenColorAttribute = attributes.FirstOrDefault(a => a.AttributeClass.ToDisplayString() == darkenColorAttributeName);

            if (lightenColorAttribute != null)
            {
                var lightenValues = lightenColorAttribute.ConstructorArguments.FirstOrDefault().Values;
                foreach (var typeconstant in lightenValues)
                {
                    var value = (int)typeconstant.Value;
                    sourceCode.AppendLine($"public virtual {fieldType} {propertyName}Lighten{value:D2} => {propertyName}.Lighten({value});")
                        .AppendLine();
                    _properties.Add((fieldType, $"{propertyName}Lighten{value:D2}"));
                }
            }

            if (darkenColorAttribute != null)
            {
                var darkenValues = darkenColorAttribute.ConstructorArguments.FirstOrDefault().Values;
                foreach (var typeConstant in darkenValues)
                {
                    var value = (int)typeConstant.Value;
                    sourceCode.AppendLine($"public virtual {fieldType} {propertyName}Darken{value:D2} => {propertyName}.Darken({value});")
                        .AppendLine();
                    _properties.Add((fieldType, $"{propertyName}Darken{value:D2}"));
                }
            }

        }

        private string GetTemplateClassName(string name)
        {
            return name.Replace("Template", "");
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
