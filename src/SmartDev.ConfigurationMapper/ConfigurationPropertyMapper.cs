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
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			_configuration = configuration;
		}

		/// <summary>
		/// Determines the properties of the given object, and maps the corresponding configuration values onto them.
		/// </summary>
		/// <param name="target">The object to map the configuration values to its properties.</param>
		public void Map(object target, string scope = null)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var config = _configuration;

			scope = GetScopeName(target, scope);
			if (scope != null)
			{
				config = config.GetSubKey(scope);
			}

			// caution: GetProperties is a method on Type on normal CLR and this is an
			// extension method in System.Reflection.TypeExtensions in CoreCLR
			foreach (var propertyInfo in target.GetType().GetProperties())
			{
				var propertyType = propertyInfo.PropertyType;
				var keyName = GetKeyName(propertyInfo);

				string valueString;
				if (config.TryGet(keyName, out valueString) && !(valueString == null))
				{
					object value = (propertyType.GetTypeInfo().IsEnum)
						? Enum.Parse(propertyType, valueString)
						: Convert.ChangeType(valueString, propertyType, CultureInfo.InvariantCulture);

					propertyInfo.SetValue(target, value);
				}
			}
		}

		/// <summary>
		/// Determines the configuration key name for a given <see cref="PropertyInfo"/> instance, by either
		/// checking the <see cref="KeyAttribute"/> value of the property or simply its name.
		/// </summary>
		/// <param name="propertyInfo">The <see cref="PropertyInfo"/> instance for a property to determine it's configuration key name from.</param>
		/// <returns>The value of a <see cref="KeyAttribute"/> name, if set; otherwise the property name.</returns>
		public string GetKeyName(PropertyInfo propertyInfo)
		{
			var attribute = propertyInfo.GetCustomAttribute<KeyAttribute>(false);
			if (attribute != null)
				return attribute.Name;

			return propertyInfo.Name;
		}

		/// <summary>
		/// Determines the scope name for a given object or by parameter.
		/// </summary>
		/// <param name="target">The object to determine the scope from.</param>
		/// <param name="scope">The scope passed in as an argument.</param>
		/// <returns>The argument scope, if set; the attribute defined scope, if set; otherwise null.</returns>
		public string GetScopeName(object target, string scope)
		{
			// early out, passed in scope has precedence
			if (!String.IsNullOrWhiteSpace(scope))
				return scope;

			var attribute = target.GetType().GetTypeInfo().GetCustomAttribute<ScopeAttribute>();
			if (attribute != null)
				return attribute.Name;

			return null;
		}
	}
}