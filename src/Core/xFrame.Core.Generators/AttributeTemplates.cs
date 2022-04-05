namespace xFrame.Core.Generators
{
    internal static class AttributeTemplates
    {
        public const string GeneratePropertyAttribute = @"

            using System;

            namespace xFrame.Core.Generators
            {
                [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
                public class GeneratepropertyAttribute : Attribute
                {
                }
            }
        ";
    }
}
