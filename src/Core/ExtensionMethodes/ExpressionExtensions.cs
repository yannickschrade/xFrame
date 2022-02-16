using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace xFrame.Core.ExtensionMethodes
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyName<T,TProperty>(this Expression<Func<T, TProperty>> expression)
        {
            var propInfo = expression.GetPropertyInfo();

            return propInfo.Name;
        }

        public static PropertyInfo GetPropertyInfo<T,TProperty>(this Expression<Func<T,TProperty>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException($"Expression refers to a method");

            var propInfo = member.Member as PropertyInfo;

            if (propInfo == null)
                throw new ArgumentException("Expression refers to a field");

            return propInfo;
        }
    }
}
