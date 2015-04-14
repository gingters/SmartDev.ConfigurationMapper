using System;
using System.Globalization;
using System.Reflection;

namespace SmartDev.ConfigurationMapper
{
	/// <summary>
	/// The <see cref="ValueTypeConverter"/> is responsible for converting strings
	/// to corresponding objects or value types.
	/// </summary>
	public class ValueTypeConverter
	{
		/// <summary>
		/// The default character used for separating array elements in values
		/// that are processed by the <see cref="ValueTypeConverter"/>. Every
		/// instance creation of <see cref="ValueTypeConverter"/> will use the
		/// current value of this static field, if not specified otherwise.
		/// </summary>
		public static char DefaultArrayElementSeparator = ';';

		/// <summary>
		/// Gets the character used for separating array elements.
		/// </summary>
		public char ElementSeparator { get; private set; }

		/// <summary>
		/// Creates a new instance of the <see cref="ValueTypeConverter"/>, using
		/// the <see cref="ValueTypeConverter.DefaultArrayElementSeparator"/> for
		/// separating array elements.
		/// </summary>
		public ValueTypeConverter()
			: this(DefaultArrayElementSeparator) { }

		/// <summary>
		/// Creates a new instance of the <see cref="ValueTypeConverter"/>, using
		/// the <paramref name="elementSeparator"/> for separating array elements.
		/// </summary>
		/// <param name="elementSeparator">The char used for separating array elements.</param>
		public ValueTypeConverter(char elementSeparator)
		{
			ElementSeparator = elementSeparator;
		}

		/// <summary>
		/// Converst a given string value into the given target type.
		/// </summary>
		/// <param name="value">The string value to convert.</param>
		/// <param name="targetType">The target type to convert the value to-</param>
		/// <returns>The converted value.</returns>
		public object Convert(string value, Type targetType)
		{
			var typeInfo = targetType.GetTypeInfo();

			if (typeInfo.IsArray)
				return CreateArrayFromValue(targetType.GetElementType(), value);

			if (value == null)
			{
				return (typeInfo.IsValueType)
					? Activator.CreateInstance(targetType)
					: null;
			}

			if (targetType == typeof(TimeSpan))
				return TimeSpan.Parse(value);

			if (typeInfo.IsEnum)
				return Enum.Parse(targetType, value);

			return System.Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Creates an array from the given string value, using the
		/// <see cref="ArrayElementSeparator"/> character to separate the distinct
		/// values.
		/// </summary>
		/// <param name="elementType">The designated type for each element in the array.</param>
		/// <param name="value">The string to convert into an array.</param>
		/// <returns>An array with converted values.</returns>
		public Array CreateArrayFromValue(Type elementType, string value)
		{
			// credits go to luisrudge: https://github.com/luisrudge/configurator/blob/master/src/Configurator/Configurator.cs#L36-L45
			string[] values = (value != null)
				? value.Split(ElementSeparator)
				: new string[0];

			var array = Array.CreateInstance(elementType, values.Length);

			for (int i = 0; i < values.Length; i++)
			{
				array.SetValue(Convert(values[i], elementType), i);
			}

			return array;
		}
	}
}