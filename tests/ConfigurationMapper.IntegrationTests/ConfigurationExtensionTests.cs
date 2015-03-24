using System;
using System.Collections.Generic;

using Microsoft.Framework.ConfigurationModel;

using Xunit;

using SmartDev.ConfigurationMapper;

namespace ConfigurationMapper.IntegrationTests
{
	public class ConfigurationExtensionTests
	{
		[Fact]
		public void Map_Should_Instanciate_And_Fill_New_Object()
		{
			// arrange
			var config = BuildSampleConfiguration();

			// act
			var obj = config.Map<SampleObject>();

			// assert
			Assert.Equal("Value1", obj.String1);
			Assert.Equal(new DateTime(1979, 1, 17, 20, 59, 0), obj.Date1);
			Assert.Null(obj.Key1);
		}

		[Fact]
		public void Mapper_Should_Map_Values_To_Existing_Object()
		{
			// arrange
			var config = BuildSampleConfiguration();
			var obj = new SampleObject();

			// act
			config.Map(obj);

			// assert
			Assert.Equal("Value1", obj.String1);
			Assert.Equal(new DateTime(1979, 1, 17, 20, 59, 0), obj.Date1);
			Assert.Null(obj.Key1);
		}

		[Fact]
		public void Mapper_Should_Pass_Existing_Object()
		{
			// arrange
			var config = BuildSampleConfiguration();
			var obj = new SampleObject();

			// act
			var obj2 = config.Map(obj);

			// assert
			Assert.Same(obj, obj2);
			Assert.Equal("Value1", obj.String1);
			Assert.Equal(new DateTime(1979, 1, 17, 20, 59, 0), obj.Date1);
			Assert.Null(obj.Key1);
		}

		[Fact]
		public void Mapper_Should_Use_Attribute_Scope()
		{
			// arrange
			var config = BuildSampleConfiguration();
			var obj = new ScopedSampleObject();

			// act
			config.Map(obj);

			// assert
			Assert.Equal("Scope1Value1", obj.Key1);
		}

		[Fact]
		public void Mapper_Should_Use_Parameter_Scope()
		{
			// arrange
			var config = BuildSampleConfiguration();
			var obj = new SampleObject();

			// act
			config.Map(obj, "Scope1");

			// assert
			Assert.Null(obj.String1);
			Assert.Equal(DateTime.MinValue, obj.Date1);
			Assert.Equal("Scope1Value1", obj.Key1);
		}

		[Fact]
		public void Mapper_Should_Use_Parameter_Over_Attribute_Scope()
		{
			// arrange
			var config = BuildSampleConfiguration();
			var obj = new SampleObject();

			// act
			config.Map(obj, "Scope2");

			// assert
			Assert.Null(obj.String1);
			Assert.Equal(DateTime.MinValue, obj.Date1);
			Assert.Equal("Scope2Value1", obj.Key1);
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