using System;
using System.Linq;

namespace TeamManager.Infrastructure
{
    /// <summary>
    /// Uses to convert Enum into array for ItemSource of ComboBox
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Retrieves list of fields in enum
        /// </summary>
        /// <typeparam name="T">Type of enum to convert to list</typeparam>
        /// <returns>List of fields of Enum</returns>
        public static T[] GetValues<T>()
        {
            var enumType = typeof (T);
            if (!enumType.IsEnum)
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");

            var fields = enumType.GetFields();

            return (
                from field in fields 
                where field.IsLiteral 
                select (T) field.GetValue(enumType)
                ).ToArray();
        }
    }
}