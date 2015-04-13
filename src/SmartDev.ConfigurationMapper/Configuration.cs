using System;

using Microsoft.Framework.ConfigurationModel;

namespace SmartDev.ConfigurationMapper
{
	public abstract class Configuration
	{
		/// <summary>
		/// Gets the <see cref="IConfiguration"/> instance passed in the ctor.
		/// </summary>
		public IConfiguration Config { get; private set; }

		/// <summary>
		/// Gets and sets the scope to map to.
		/// </summary>
		public string Scope { get; set; }

		/// <summary>
		/// Private to avoid instanciating without an instance of <see cref="IConfiguration"/>.
		/// </summary>
		private Configuration() { }

		/// <summary>
		/// INitializes a new instance of this configuration object.
		/// </summary>
		/// <param name="configuration">An instance of <see cref="IConfiguration"/> to map the config values from.</param>
		/// <param name="instantMapping">Optional parameter. Set to false to prevent a direct initial mapping of values. You can call <see cref="Refresh"/> to map the values afterwards.</param>
		public Configuration(IConfiguration configuration, bool instantMapping = true)
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			Config = configuration;

			if (instantMapping)
				Refresh();
		}

		/// <summary>
		/// Reloads the values for the passed <see cref="IConfiguration"/> instance and
		/// refreshes the configuration values.
		/// </summary>
		public void Reload()
		{
			Config.Reload();
			Refresh();
		}

		/// <summary>
		/// Refreshes the values of this object from the configuration object.
		/// </summary>
		public void Refresh()
		{
			Config.Map(this, Scope);
		}
	}
}