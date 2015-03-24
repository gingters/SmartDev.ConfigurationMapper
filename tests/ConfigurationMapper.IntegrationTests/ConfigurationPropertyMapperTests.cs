using System;
using System.Collections.Generic;

using Microsoft.Framework.ConfigurationModel;

using Xunit;

using SmartDev.ConfigurationMapper;

namespace ConfigurationMapper.IntegrationTests
{
	public class ConfigurationPropertyMapperTests
	{
		[Fact]
		public void Mapper_Should_Map_Values_Without_Scope()
		{
			// arrange
			var config = BuildSampleConfiguration();
			var obj = new SampleObject();

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal("Value1", obj.String1);
			Assert.Equal(new DateTime(1979, 1, 17, 20, 59, 0), obj.Date1);
			Assert.Null(obj.Key1);
		}

		[Fact]
		public void Mapper_Should_Map_Scoped_Values()
		{
			// arrange
			var config = BuildSampleConfiguration();
			var obj = new SampleObject();

			// act
			config = config.GetSubKey("Scope1");

			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Null(obj.String1);
			Assert.Equal(DateTime.MinValue, obj.Date1);
			Assert.Equal(obj.Key1, "Scope1Value1");
		}

		[Fact]
		public void Mapper_Should_Determine_Scope_From_Attribute()
		{
			// arrange
			var mapper = new ConfigurationPropertyMapper(BuildSampleConfiguration());
			var obj = new ScopedSampleObject();

			// act
			mapper.Map(obj);

			// assert
			Assert.Equal(obj.Key1, "Scope1Value1");
		}

		[Fact]
		public void Mapper_Should_Determine_Scope_From_Parameter()
		{
			// arrange
			var mapper = new ConfigurationPropertyMapper(BuildSampleConfiguration());
			var obj = new SampleObject();

			// act
			mapper.Map(obj, "Scope2");

			// assert
			Assert.Equal(obj.Key1, "Scope2Value1");
		}

		[Fact]
		public void Mapper_Should_Use_Scope_From_Parameter_Over_Attribute()
		{
			// arrange
			var mapper = new ConfigurationPropertyMapper(BuildSampleConfiguration());
			var obj = new ScopedSampleObject();

			// act
			mapper.Map(obj, "Scope2");

			// assert
			Assert.Equal(obj.Key1, "Scope2Value1");
		}

		private class SampleObject
		{
			public string String1 { get; set; }
			public DateTime Date1 { get; set; }
			public string Key1 { get; set; }
		}

		[Scope("Scope1")]
		private class ScopedSampleObject
		{
			public string Key1 { get; set; }
		}

		private IConfiguration BuildSampleConfiguration()
		{
			var data = new Dictionary<string, string>()
			{
				{ "String1", "Value1" },
				{ "Date1", "1979-01-17T20:59:00" },
				{ "Scope1:Key1", "Scope1Value1" },
				{ "Scope2:Key1", "Scope2Value1" },
			};

			var memoryConfigSource = new MemoryConfigurationSource(data);

			var config = new Configuration();
			config.Add(memoryConfigSource);

			return config;
		}
	}
}