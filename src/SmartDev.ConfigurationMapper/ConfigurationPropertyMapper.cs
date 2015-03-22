using System;
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
			foreach (var propertyInfo in TypeHelper.GetProperties(target.GetType()))
			{
				var propertyType = propertyInfo.PropertyType;

				string valueString;
				if (_configuration.TryGet(propertyInfo.Name, out valueString) && !(valueString == null))
				{
					object value = (TypeHelper.IsEnum(propertyType))
						? Enum.Parse(propertyType, valueString)
						: Convert.ChangeType(valueString, propertyType, CultureInfo.InvariantCulture);

					propertyInfo.SetValue(target, value);
				}
			}
		}
	}
}