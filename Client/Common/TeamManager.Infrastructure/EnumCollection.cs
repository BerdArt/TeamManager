using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace TeamManager.Infrastructure
{
    public class EnumCollection<T> : List<EnumContainer> where T: struct 
    {
        public EnumCollection()
        {
            var type = typeof (T);
            if (!type.IsEnum)
                throw new ArgumentException("This class only supports Enum types");
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var field in fields)
            {
                var container = new EnumContainer();
                container.EnumOriginalValue = field.GetValue(null);
                container.EnumValue = (int) field.GetValue(null);
                container.EnumDescription = field.Name;
                var attrs = field.GetCustomAttributes(false);
                foreach (var attr in attrs.OfType<DescriptionAttribute>())
                {
                    container.EnumDescription = (attr).Description;
                    break;
                }
                Add(container);
            }
        }
    }
}