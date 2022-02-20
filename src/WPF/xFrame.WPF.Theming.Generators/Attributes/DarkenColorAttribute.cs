﻿using System;

namespace xFrame.WPF.Theming.Generators.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DarkenColorAttribute : Attribute
    {

        public int[] Values { get; }
        public DarkenColorAttribute(params int[] values)
        {
            Values = values;
        }
    }
}