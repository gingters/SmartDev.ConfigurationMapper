using System;
using System.Reflection;
using System.Globalization;

using Microsoft.Framework.ConfigurationModel;

namespace SmartDev.ConfigurationMapper
{
	/// <summary>
	/// The <see cref="ConfigurationPropertyMapper">ConfigurationPropertyMapper</see> maps values from a given
	/// <see cref="Microsoft.Framework.ConfigurationModel.IConfiguration">IConfiguration</see> instance to properties
	/// of a given object.
	/// </summary>
	public class ConfigurationPropertyMapper
	{
		private IConfiguration _configuration;

		/// <summary>
		/// Creates a new instance of the <see cref="ConfigurationPropertyMapper">ConfigurationPropertyMapper</see>.
		/// </summary>
		/// <param name="configuration">An instance of
		/// <see cref="Microsoft.Framework.ConfigurationModel.IConfiguration">IConfiguration</see>
		/// to map values from.</param>
		public ConfigurationPropertyMapper(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		/// <summary>
		/// Determines the properties of the given object, and maps the corresponding configuration values onto them.
		/// </summary>
		/// <param name="target">The object to map the configuration values to its properties.</param>
		public void Map(object target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			// caution: GetProperties is a method on Type on normal CLR and this is an
			// extension method in System.Reflection.TypeExtensions in CoreCLR
			foreach (var propertyInfo in target.GetType().GetProperties())
			{
				var propertyType = propertyInfo.PropertyType;

				string valueString;
				if (_configuration.TryGet(propertyInfo.Name, out valueString) && !(valueString == null))
				{
					object value = (propertyType.GetTypeInfo().IsEnum)
						? Enum.Parse(propertyType, valueString)
						: Convert.ChangeType(valueString, propertyType, CultureInfo.InvariantCulture);

					propertyInfo.SetValue(target, value);
				}
			}
		}
	}
}