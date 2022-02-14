using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace xFrame.Core.ExtensionMethodes
{
    public static class ExpressionExtensions
    {
        public static string GetMebmerName<T,TProperty>(this Expression<Func<T, TProperty>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException($"Expression refers to a method");

            var propInfo = member.Member as PropertyInfo;

            return propInfo == null ? throw new ArgumentException("Expression refers to a field not a property") : propInfo.Name;
        }
    }
}
