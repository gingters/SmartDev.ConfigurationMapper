using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Framework.ConfigurationModel;

using Xunit;

using SmartDev.ConfigurationMapper;
using Config = SmartDev.ConfigurationMapper.Configuration;

namespace ConfigurationMapper.UnitTests
{
	public class ConfigurationTests
	{
		[Fact]
		public void Configuration_Ctor_Should_Throw_On_Null_Argument()
		{
			Assert.Throws<ArgumentNullException>(() => new TestClass(null));
		}

		private class TestClass: Config
		{
			public string SomeString { get; set; }

			public TestClass(IConfiguration configuration)
				: base(configuration)
			{
			}
		}
	}
}