using System;

using Microsoft.Framework.ConfigurationModel;

namespace SmartDev.ConfigurationMapper
{
	/// <summary>
	/// Provides extension methods for the <see cref="IConfiguration">IConfiguration</see> interface
	/// to map configuration values to objects.
	/// </summary>
	public static class ConfigurationExtensions
	{
		/// <summary>
		/// Maps values of the provided configuration to a new instance of the specified
		/// configuration type.
		/// </summary>
		/// <typeparam name="T">The type of the configuration class to map to. Needs to
		/// have a parameterless constructor.</typeparam>
		/// <param name="configuration">An instance of <see cref="IConfiguration">IConfiguration</see>
		/// to map values from.</param>
		/// <returns>A new instance of <typeparamref name="T"/> filled with configuration values
		/// matching the property names.</returns>
		public static T Map<T>(this IConfiguration configuration)
			where T : class, new()
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			var target = new T();
			return configuration.Map(target);
		}

		/// <summary>
		/// Maps values of the provided configuration to an existing instance of the specified
		/// configuration type.
		/// </summary>
		/// <typeparam name="T">The type of the configuration class to map to. Usually can be
		/// ommited.</typeparam>
		/// <param name="configuration">An instance of <see cref="IConfiguration">IConfiguration</see>
		/// to map values from.</param>
		/// <param name="target">An existing object of type <typeparamref name="T"/>. Configuration
		/// values will be mapped to this instance.</param>
		/// <returns>The same object you passed in as <paramref name="target"/>, with the properties
		/// mapped to the configuration values.</returns>
		public static T Map<T>(this IConfiguration configuration, T target)
			where T : class
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");
			if (target == null)
				throw new ArgumentNullException("target");

			var mapper = new ConfigurationPropertyMapper(configuration);
			mapper.Map(target);

			return target;
		}
	}
}