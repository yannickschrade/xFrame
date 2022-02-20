using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace xFrame.WPF.Theming.Generators
{
    public class SourceStringBuilder
    {
        private readonly string SingleIndent = new string(' ', 4);

        private int _indent = 0;
        private readonly StringBuilder _stringBuilder;

        public SourceStringBuilder()
        {
            _stringBuilder = new StringBuilder();
        }

        public SourceStringBuilder IncreaseIndent()
        {
            _indent++;
            return this;
        }

        public SourceStringBuilder DecreaseIndent()
        {
            _indent--;
            return this;
        }

        public SourceStringBuilder AppendOpenCurlyBracketLine()
        {
            AppendLine("{");
            IncreaseIndent();
            return this;
        }

        public SourceStringBuilder AppendCloseCurlyBracketLine()
        {
            DecreaseIndent();
            AppendLine("}");
            return this;
        }

        public SourceStringBuilder Append(string text)
        {
            for (int i = 0; i < _indent; i++)
            {
                _stringBuilder.Append(SingleIndent);
            }

            _stringBuilder.Append(text);
            return this;
        }

        public SourceStringBuilder AppendLine()
        {
            _stringBuilder.AppendLine();
            return this;
        }

        public SourceStringBuilder AppendLine(string text)
        {
            Append(text);
            return AppendLine();
        }

        public override string ToString()
        {
            var text = _stringBuilder.ToString();
            return string.IsNullOrWhiteSpace(text)
                ? string.Empty
                : CSharpSyntaxTree.ParseText(text).GetRoot().NormalizeWhitespace().SyntaxTree.GetText().ToString();
        }
    }
}