using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace pdxpartyparrot.Core.Util
{
    /// <summary>
    /// Enumeration description utilities
    /// </summary>
    /// <remarks>
    /// Taken from http://stackoverflow.com/questions/4367723/get-enum-from-description-attribute
    /// </remarks>
    public static class EnumDescription
    {
        /// <summary>
        /// Gets the description from an enum value.
        /// </summary>
        /// <param name="value">The enum value.</param>
        /// <returns>The description of the enum value</returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static string GetDescription(this Enum value)
        {
            if(null == value) {
                throw new ArgumentNullException(nameof(value));
            }

            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Gets the enum value from the description.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The enum value description.</param>
        /// <returns>The enum value.</returns>
        /// <exception cref="System.ArgumentException">Type is not an enum!</exception>
        public static T GetEnumValueFromDescription<T>(string description)
        {
            Type type = typeof(T);
            if(!type.IsEnum) {
                throw new ArgumentException("Type is not an enum!");
            }

            FieldInfo[] fields = type.GetFields();
            var field = fields
                .SelectMany(f => f.GetCustomAttributes(typeof(DescriptionAttribute), false), (f, a) => new { Field = f, Att = a })
                .SingleOrDefault(a => ((DescriptionAttribute)a.Att)
                                      .Description == description);
            return (T)field?.Field.GetRawConstantValue();
        }
    }
}
