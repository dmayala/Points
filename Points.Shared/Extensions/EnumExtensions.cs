using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Points.Shared.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get value serialization name
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Result name</returns>
        public static string GetSerializationName(this Enum value)
        {
            // Get enum attribute
            var enumAttribute = value.GetCustomAttribute<EnumMemberAttribute>();

            // Ensure it was found
            if (enumAttribute != null)
            {
                return enumAttribute.Value;
            }

            // Not found
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        /// Get custom attribute defined on given enum value
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type</typeparam>
        /// <param name="enumValue">Enum value</param>
        /// <returns>Matching custom attribute. Null if attribute not found.</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            // Get member info for enum value
            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

            // Return attribute
            return (TAttribute) memberInfo?.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();
        }

        /// <summary>
        /// Gets Enum from EnumMemberAttribute
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>Matching enum. Return Default if not found.</returns>
        public static T ToEnum<T>(this string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }
            return default(T);
        }
    }
}