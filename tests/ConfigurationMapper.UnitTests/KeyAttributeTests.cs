using System;
using System.Reflection;

using Xunit;

using SmartDev.ConfigurationMapper;

namespace SmartDev.ConfigurationMapper.Tests
{
	public class KeyAttributeTests
	{
		[Fact]
		public void KeyAttribute_Should_Throw_Exception_On_Null()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => new KeyAttribute(null));

			Assert.Equal("name", ex.ParamName);
		}

		[Fact]
		public void KeyAttribute_Should_Throw_Exception_On_EmptyString()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => new KeyAttribute(String.Empty));

			Assert.Equal("name", ex.ParamName);
		}

		[Fact]
		public void KeyAttribute_Should_Throw_Exception_On_Whitespace_Only()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => new KeyAttribute(" "));

			Assert.Equal("name", ex.ParamName);
		}

		[Theory]
		[InlineData("Test")]
		[InlineData("Foo")]
		public void KeyAttribute_Should_Set_Name(string testData)
		{
			var attribute = new KeyAttribute(testData);

			Assert.Equal(testData, attribute.Name);
		}

		[Fact]
		public void KeyAttribute_Should_Carry_Value()
		{
			var type = typeof(TestClass1);
			var propertyInfo = type.GetTypeInfo().GetProperty("TestProperty");
			var attribute = propertyInfo.GetCustomAttribute(typeof(KeyAttribute)) as KeyAttribute;

			Assert.NotNull(attribute);
			Assert.Equal("TestKey", attribute.Name);
		}

		/* Wrong assumption?
		[Fact]
		public void KeyAttribute_Should_Not_Be_Inherited()
		{
			var type = typeof(TestClass2);
			var propertyInfo = type.GetTypeInfo().GetProperty("TestProperty");
			var attribute = propertyInfo.GetCustomAttribute(typeof(KeyAttribute)) as KeyAttribute;

			Assert.Null(attribute);
		}
		*/

		public class TestClass1
		{
			[Key("TestKey")]
			public string TestProperty {get; set; }
		}

		/* Not needed, in case of wrong assumption above
		public class TestClass2: TestClass1
		{
		}
		*/
	}
}