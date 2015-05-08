using System;
using System.Reflection;

using Xunit;

using SmartDev.ConfigurationMapper;

namespace ConfigurationMapper.Tests
{
	public class ScopeAttributeTests
	{
		[Fact]
		public void ScopeAttribute_Should_Throw_Exception_On_Null()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => new ScopeAttribute(null));

			Assert.Equal("name", ex.ParamName);
		}

		[Fact]
		public void ScopeAttribute_Should_Throw_Exception_On_EmptyString()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => new ScopeAttribute(String.Empty));

			Assert.Equal("name", ex.ParamName);
		}

		[Fact]
		public void ScopeAttribute_Should_Throw_Exception_On_Whitespace_Only()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => new ScopeAttribute(" "));

			Assert.Equal("name", ex.ParamName);
		}

		[Theory]
		[InlineData("Test")]
		[InlineData("Foo")]
		public void KeyAttribute_Should_Set_Name(string testData)
		{
			var attribute = new ScopeAttribute(testData);

			Assert.Equal(testData, attribute.Name);
		}

		[Fact]
		public void KeyAttribute_Should_Carry_Value()
		{
			var type = typeof(TestClass1);
			var attribute = type.GetTypeInfo().GetCustomAttribute<ScopeAttribute>();

			Assert.NotNull(attribute);
			Assert.Equal("TestScope", attribute.Name);
		}

		[Fact]
		public void KeyAttribute_Should_Not_Be_Inherited()
		{
			var type = typeof(TestClass2);
			var attribute = type.GetTypeInfo().GetCustomAttribute<ScopeAttribute>();

			Assert.Null(attribute);
		}

		[Scope("TestScope")]
		public class TestClass1
		{
			public string TestProperty {get; set; }
		}

		public class TestClass2: TestClass1
		{
		}
	}
}