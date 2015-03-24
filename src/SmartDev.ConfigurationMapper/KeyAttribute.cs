using System;

namespace SmartDev.ConfigurationMapper
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class KeyAttribute: Attribute
	{
		public string Name { get; set; }

		public KeyAttribute(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			Name = name;
		}
	}
}