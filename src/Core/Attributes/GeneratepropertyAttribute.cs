using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class GeneratepropertyAttribute : Attribute
    {
    }
}
