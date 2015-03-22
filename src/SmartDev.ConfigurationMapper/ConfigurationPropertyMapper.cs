using System;
using System.Reflection;
using System.Globalization;
using Microsoft.Framework.ConfigurationModel;

namespace SmartDev.ConfigurationMapper
{
	public class ConfigurationPropertyMapper
	{
		private IConfiguration _configuration;

		public ConfigurationPropertyMapper(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Map(object target)
		{
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