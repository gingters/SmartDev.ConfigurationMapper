using System;

namespace SmartDev.ConfigurationMapper
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class ScopeAttribute: Attribute
	{
		public string Name { get; set; }

		public ScopeAttribute(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			Name = name;
		}
	}
}