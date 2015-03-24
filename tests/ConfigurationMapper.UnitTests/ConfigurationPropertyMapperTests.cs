using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Framework.ConfigurationModel;

using Xunit;

using SmartDev.ConfigurationMapper;

namespace ConfigurationMapper.UnitTests
{
	public class ConfigurationPropertyMapperTests
	{

		[Fact]
		public void Mapper_Should_Throw_On_Null_Constructor_Call()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => new ConfigurationPropertyMapper(null));

			Assert.Equal("configuration", ex.ParamName);
		}

		[Fact]
		public void Mapper_Should_Not_Change_Defaults_With_No_Values()
		{
			// arrange
			var config = new MockedConfiguration();

			var obj = new TestObject()
			{
				StringValue = "Default",
				IntValue = 42,
				FloatValue = 3.14159265359,
				DateValue = new DateTime(1979, 01, 17, 20, 59, 00),
				BoolValue = true,
				CharValue = 'a',
				ByteValue = 128,
				EnumValue = TestEnum.Mon,
			};

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal("Default", obj.StringValue);
			Assert.Equal(42, obj.IntValue);
			Assert.InRange(obj.FloatValue, 3.141500, 3.141599);
			Assert.Equal(new DateTime(1979, 01, 17, 20, 59, 00), obj.DateValue);
			Assert.True(obj.BoolValue);
			Assert.Equal('a', obj.CharValue);
			Assert.Equal(128, obj.ByteValue);
			Assert.Equal(TestEnum.Mon, obj.EnumValue);
			Assert.Equal(null, obj.MappedStringValue);
		}

		[Fact]
		public void Mapper_Should_Not_Change_Defaults_With_Null_Values()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("StringValue", null);
			config.TestData.Add("IntValue", null);
			config.TestData.Add("FloatValue", null);
			config.TestData.Add("DateValue", null);
			config.TestData.Add("BoolValue", null);
			config.TestData.Add("CharValue", null);
			config.TestData.Add("ByteValue", null);
			config.TestData.Add("EnumValue", null);

			var obj = new TestObject()
			{
				StringValue = "Default",
				IntValue = 42,
				FloatValue = 3.14159265359,
				DateValue = new DateTime(1979, 01, 17, 20, 59, 00),
				BoolValue = true,
				CharValue = 'a',
				ByteValue = 128,
				EnumValue = TestEnum.Mon,
			};

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal("Default", obj.StringValue);
			Assert.Equal(42, obj.IntValue);
			Assert.InRange(obj.FloatValue, 3.141500, 3.141599);
			Assert.Equal(new DateTime(1979, 01, 17, 20, 59, 00), obj.DateValue);
			Assert.True(obj.BoolValue);
			Assert.Equal('a', obj.CharValue);
			Assert.Equal(128, obj.ByteValue);
			Assert.Equal(TestEnum.Mon, obj.EnumValue);
			Assert.Equal(null, obj.MappedStringValue);
		}

		[Fact]
		public void Mapper_Should_Not_Fail_On_Not_Existing_Values()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("OtherValue", "OtherValue");

			var obj = new TestObject();

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj); // should not throw exception

			// assert
			Assert.True(true);
		}

		[Fact]
		public void Mapper_Should_Map_String_Values()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("StringValue", "Value");

			var obj = new TestObject();
			Assert.Null(obj.StringValue);

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal("Value", obj.StringValue);
		}

		[Fact]
		public void Mapper_Should_Map_Empty_String_Values()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("StringValue", String.Empty);

			var obj = new TestObject();
			Assert.Null(obj.StringValue);

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal(String.Empty, obj.StringValue);
		}

		[Fact]
		public void Mapper_Should_Map_Integer_Values()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("IntValue", "42");

			var obj = new TestObject();
			Assert.Equal(0, obj.IntValue);

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal(42, obj.IntValue);
		}

		[Fact]
		public void Mapper_Should_Map_Float_Values()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("FloatValue", "3.14159265359");

			var obj = new TestObject();
			Assert.Equal(0.0D, obj.FloatValue);

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.InRange(obj.FloatValue, 3.141500, 3.141599);
		}

		[Fact]
		public void Mapper_Should_Map_DateTime_Values()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("DateValue", "1979-01-17T20:59:00");

			var obj = new TestObject();
			Assert.Equal(DateTime.MinValue, obj.DateValue);

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal(new DateTime(1979, 01, 17, 20, 59, 00), obj.DateValue);
		}

		[Fact]
		public void Mapper_Should_Map_Bool_Values()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("BoolValue", "true");

			var obj = new TestObject();
			Assert.False(obj.BoolValue);

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.True(obj.BoolValue);
		}

		[Fact]
		public void Mapper_Should_Map_Char_Values()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("CharValue", "b");

			var obj = new TestObject();
			Assert.Equal(default(char), obj.CharValue);

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal('b', obj.CharValue);
		}

		[Fact]
		public void Mapper_Should_Map_Byte_Values()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("ByteValue", "255");

			var obj = new TestObject();
			Assert.Equal(default(byte), obj.ByteValue);

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal(255, obj.ByteValue);
		}

		[Fact]
		public void Mapper_Should_Map_Enum_Values_By_Name()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("EnumValue", "Tue");

			var obj = new TestObject();
			Assert.Equal(default(TestEnum), obj.EnumValue);

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal(TestEnum.Tue, obj.EnumValue);
		}

		[Fact]
		public void Mapper_Should_Map_Enum_Values_By_Value()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("EnumValue", "3");

			var obj = new TestObject();
			Assert.Equal(default(TestEnum), obj.EnumValue);

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal(TestEnum.Wed, obj.EnumValue);
		}

		[Fact]
		public void Mapper_Should_Map_Values_For_Attributed_Key()
		{
			// arrange
			var config = new MockedConfiguration();
			config.TestData.Add("StringValue", "TestValue");

			var obj = new TestObject();
			Assert.Equal(default(TestEnum), obj.EnumValue);

			// act
			var mapper = new ConfigurationPropertyMapper(config);
			mapper.Map(obj);

			// assert
			Assert.Equal("TestValue", obj.StringValue);
			Assert.Equal("TestValue", obj.MappedStringValue);
		}

		[Fact]
		public void Mapper_Should_Determine_Key_From_PropertyName()
		{
			// arrange
			var type = typeof(TestObject);
			var mapper = new ConfigurationPropertyMapper(new MockedConfiguration());

			// act
			var propertyInfo = type.GetProperty("StringValue");
			var keyName = mapper.GetKeyName(propertyInfo);

			// assert
			Assert.Equal("StringValue", keyName);
		}

		[Fact]
		public void Mapper_Should_Determine_Key_From_AttributeValue()
		{
			// arrange
			var type = typeof(TestObject);
			var mapper = new ConfigurationPropertyMapper(new MockedConfiguration());

			// act
			var propertyInfo = type.GetProperty("MappedStringValue");
			var keyName = mapper.GetKeyName(propertyInfo);

			// assert
			Assert.Equal("StringValue", keyName);
		}

		private class TestObject
		{
			public String StringValue { get; set; }
			public int IntValue { get; set; }
			public double FloatValue { get; set; }
			public DateTime DateValue { get; set; }
			public bool BoolValue { get; set; }
			public char CharValue { get; set; }
			public byte ByteValue { get; set; }
			public TestEnum EnumValue { get; set; }

			[Key("StringValue")]
			public string MappedStringValue { get; set; }
		}

		private enum TestEnum
		{
			Mon = 1,
			Tue = 2,
			Wed = 3,
		}

		private class MockedConfiguration : IConfiguration
		{
			public Dictionary<string, string> TestData = new Dictionary<string, string>();

			public string Get(string key)
			{
				// simulate behaviour of IConfiguration (returns null if value is not found)
				string value;
				return (TestData.TryGetValue(key, out value)) ? value : null;
			}

			public bool TryGet(string key, out string value)
			{
				// simulate behaviour of IConfiguration
				if (TestData.TryGetValue(key, out value))
					return true;

				value = null;
				return false;
			}

			public string this[string key]
			{
				get
				{
					return Get(key);
				}

				set
				{
					Set(key, value);
				}
			}

			#region not implemented stuff

			public void Commit()
			{
				throw new NotImplementedException();
			}

			public IConfiguration GetSubKey(string key)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<KeyValuePair<string, IConfiguration>> GetSubKeys()
			{
				throw new NotImplementedException();
			}

			public IEnumerable<KeyValuePair<string, IConfiguration>> GetSubKeys(string key)
			{
				throw new NotImplementedException();
			}

			public void Reload()
			{
				throw new NotImplementedException();
			}

			public void Set(string key, string value)
			{
				throw new NotImplementedException();
			}

			#endregion
		}
	}
}