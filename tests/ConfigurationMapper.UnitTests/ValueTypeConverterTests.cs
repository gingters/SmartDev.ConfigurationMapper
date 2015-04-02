using System;

using Xunit;

using SmartDev.ConfigurationMapper;

namespace ConfigurationMapper.UnitTests
{
	public class ValueTypeConverterTests
	{
		#region ctor
		[Fact]
		public void Should_Instanciate_Correctly()
		{
			// act
			var vtc = new ValueTypeConverter();

			// assert
			Assert.NotNull(vtc);
			Assert.Equal(';', vtc.ElementSeparator);
		}

		[Fact]
		public void Should_Instanciate_Correctly_With_Argument()
		{
			// act
			var vtc = new ValueTypeConverter(',');

			// assert
			Assert.NotNull(vtc);
			Assert.Equal(',', vtc.ElementSeparator);
		}
		#endregion

		#region Char
		[Theory]
		[InlineData("a", 'a')]
		[InlineData("z", 'z')]
		[InlineData("ß", 'ß')]
		[InlineData("0", '0')]
		[InlineData("!", '!')]
		[InlineData("µ", 'µ')]
		public void Should_Convert_Char(string input, char expected)
		{
			// arrange
			var targetType = typeof(Char);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(input, targetType);

			// assert
			Assert.IsType<Char>(obj);
			Assert.Equal(expected, obj);
		}

		[Fact]
		public void Should_Convert_Char_From_Null()
		{
			// arrange
			var targetType = typeof(Char);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(null, targetType);

			// assert
			Assert.IsType<Char>(obj);
			Assert.Equal(default(Char), obj);
		}

		[Fact]
		public void Should_Throw_On_Invalid_Char()
		{
			// arrange
			var targetType = typeof(Char);
			var value = "ab";
			var vtc = new ValueTypeConverter();

			// act
			Assert.Throws<FormatException>(() => vtc.Convert(value, targetType));
		}
		#endregion Char

		#region String
		[Theory]
		[InlineData("TestString")]
		[InlineData("Some\tSpecial\nChars")]
		public void Should_Convert_String(string data)
		{
			// arrange
			var targetType = typeof(String);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(data, targetType);

			// assert
			Assert.IsType<String>(obj);
			Assert.Equal(data, obj);
		}

		[Fact]
		public void Should_Convert_String_From_Null()
		{
			// arrange
			var targetType = typeof(String);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(null, targetType);

			// assert
			Assert.Null(obj);
			Assert.Equal(default(String), obj);
		}
		#endregion

		#region Int
		[Theory]
		[InlineData("0", 0)]
		[InlineData("-2147483648", Int32.MinValue)]
		[InlineData("2147483647", Int32.MaxValue)]
		public void Should_Convert_Int32(string value, int expected)
		{
			// arrange
			var targetType = typeof(Int32);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<Int32>(obj);
			Assert.Equal(expected, obj);
		}

		[Fact]
		public void Should_Convert_Int_From_Null()
		{
			// arrange
			var targetType = typeof(Int32);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(null, targetType);

			// assert
			Assert.IsType<Int32>(obj);
			Assert.Equal(default(Int32), obj);
		}

		[Theory]
		[InlineData("0", (Int64)0)]
		[InlineData("-2147483648", (Int64)Int32.MinValue)]
		[InlineData("2147483647", (Int64)Int32.MaxValue)]
		[InlineData("-9223372036854775808", Int64.MinValue)]
		[InlineData("9223372036854775807", Int64.MaxValue)]
		public void Should_Convert_Int64(string value, Int64 expected)
		{
			// arrange
			var targetType = typeof(Int64);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<Int64>(obj);
			Assert.Equal(expected, obj);
		}

		[Fact]
		public void Should_Throw_On_Invalid_Int()
		{
			// arrange
			var targetType = typeof(int);
			var value = "ab";
			var vtc = new ValueTypeConverter();

			// act
			Assert.Throws<FormatException>(() => vtc.Convert(value, targetType));
		}
		#endregion

		#region Floating point
		[Theory]
		[InlineData("3.1415927", 3.1415, 3.1416)]
		[InlineData("0", -0.01, 0.01)]
		public void Should_Convert_Float(string value, float lowerLimit, float upperLimit)
		{
			// arrange
			var targetType = typeof(Single);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<Single>(obj);
			Assert.InRange((Single)obj, lowerLimit, upperLimit);
		}

		[Fact]
		public void Should_Convert_Float_From_Null()
		{
			// arrange
			var targetType = typeof(Single);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(null, targetType);

			// assert
			Assert.IsType<Single>(obj);
			Assert.Equal(default(Single), obj);
		}

		[Fact]
		public void Should_Throw_On_Invalid_Float()
		{
			// arrange
			var targetType = typeof(Single);
			var value = "ab";
			var vtc = new ValueTypeConverter();

			// act
			Assert.Throws<FormatException>(() => vtc.Convert(value, targetType));
		}
		#endregion

		#region DateTime
		[Theory]
		[InlineData("0001-01-01T00:00:00", 1, 1, 1, 0, 0, 0, 0)]
		[InlineData("9999-12-31T23:59:59.999", 9999, 12, 31, 23, 59, 59, 999)]
		[InlineData("2000-01-01T00:00:00", 2000, 1, 1, 0, 0, 0, 0)]
		public void Should_Convert_DateTime(string value, int year, int month, int day, int hour, int minute, int second, int millisec)
		{
			// arrange
			var targetType = typeof(DateTime);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<DateTime>(obj);
			Assert.Equal(new DateTime(year, month, day, hour, minute, second, millisec), obj);
		}

		[Fact]
		public void Should_Convert_DateTime_From_Null()
		{
			// arrange
			var targetType = typeof(DateTime);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(null, targetType);

			// assert
			Assert.IsType<DateTime>(obj);
			Assert.Equal(default(DateTime), obj);
		}

		[Fact]
		public void Should_Throw_On_Invalid_DateTime_String()
		{
			// arrange
			var targetType = typeof(DateTime);
			var value = "ab";
			var vtc = new ValueTypeConverter();

			// act
			Assert.Throws<FormatException>(() => vtc.Convert(value, targetType));
		}
		#endregion

		#region Byte
		[Theory]
		[InlineData("0", (Byte)0)]
		[InlineData("255", (Byte)255)]
		public void Should_Convert_Byte(string value, Byte expected)
		{
			// arrange
			var targetType = typeof(Byte);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<Byte>(obj);
			Assert.Equal(expected, obj);
		}

		[Fact]
		public void Should_Convert_Byte_From_Null()
		{
			// arrange
			var targetType = typeof(Byte);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(null, targetType);

			// assert
			Assert.IsType<Byte>(obj);
			Assert.Equal(default(Byte), obj);
		}

		[Fact]
		public void Should_Throw_On_Invalid_Byte_String()
		{
			// arrange
			var targetType = typeof(Byte);
			var value = "ab";
			var vtc = new ValueTypeConverter();

			// act
			Assert.Throws<FormatException>(() => vtc.Convert(value, targetType));
		}
		#endregion

		#region Boolean
		[Theory]
		[InlineData("True", true)]
		[InlineData("TRUE", true)]
		[InlineData("true", true)]
		[InlineData("trUE", true)]
		[InlineData("false", false)]
		[InlineData("False", false)]
		[InlineData("FALSE", false)]
		[InlineData("falSE", false)]
		public void Should_Convert_Bool_By_String_Uppercase(string value, bool expected)
		{
			// arrange
			var targetType = typeof(Boolean);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<Boolean>(obj);
			Assert.Equal(expected, obj);
		}

		[Fact]
		public void Should_Convert_Bool_From_Null()
		{
			// arrange
			var targetType = typeof(Boolean);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(null, targetType);

			// assert
			Assert.IsType<Boolean>(obj);
			Assert.Equal(default(Boolean), obj);
		}

		[Fact]
		public void Should_Throw_On_Invalid_Boolean_String()
		{
			// arrange
			var targetType = typeof(Boolean);
			var value = "ab";
			var vtc = new ValueTypeConverter();

			// act
			Assert.Throws<FormatException>(() => vtc.Convert(value, targetType));
		}
		#endregion

		#region Enum
		public enum TestEnum
		{
			Mon = 1,
			Tue = 3,
			Wed = 5,
		}

		[Theory]
		[InlineData("Mon", TestEnum.Mon)]
		[InlineData("Tue", TestEnum.Tue)]
		[InlineData("Wed", TestEnum.Wed)]
		public void Should_Convert_Enum_By_Name(string value, TestEnum expected)
		{
			// arrange
			var targetType = typeof(TestEnum);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<TestEnum>(obj);
			Assert.Equal(expected, obj);
		}

		[Fact]
		public void Should_Convert_Enum_From_Null()
		{
			// arrange
			var targetType = typeof(TestEnum);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(null, targetType);

			// assert
			Assert.IsType<TestEnum>(obj);
			Assert.Equal(default(TestEnum), obj);
		}

		[Theory]
		[InlineData("1", TestEnum.Mon)]
		[InlineData("3", TestEnum.Tue)]
		[InlineData("5", TestEnum.Wed)]
		public void Should_Convert_Enum_By_Number(string value, TestEnum expected)
		{
			// arrange
			var targetType = typeof(TestEnum);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<TestEnum>(obj);
			Assert.Equal(expected, obj);
		}

		[Fact]
		public void Should_Throw_On_Invalid_Enum_String()
		{
			// arrange
			var targetType = typeof(TestEnum);
			var value = "ab";
			var vtc = new ValueTypeConverter();

			// act
			Assert.Throws<ArgumentException>(() => vtc.Convert(value, targetType));
		}
		#endregion

		#region Array
		[Theory]
		[InlineData("Test0", 1)]
		[InlineData("Test0;Test1", 2)]
		[InlineData("Test0;Test1;Test2", 3)]
		[InlineData("Test0;Test1;Test2;Test3", 4)]
		public void Should_Convert_Array_With_Different_Length(string value, int amount)
		{
			// arrange
			var targetType = typeof(String[]);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<String[]>(obj);
			var testObj = (String[])obj;

			Assert.Equal(amount, testObj.Length);
			for (int i = 0; i < amount; i++)
			{
				Assert.Equal("Test" + i, testObj[i]);
			}
		}

		[Theory]
		[InlineData(',', "Test1,Test2,Test3")]
		[InlineData(';', "Test1;Test2;Test3")]
		[InlineData('µ', "Test1µTest2µTest3")]
		public void Should_Convert_Array_By_Different_Separators(char separator, string value)
		{
			// arrange
			var targetType = typeof(String[]);
			var vtc = new ValueTypeConverter(separator);

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<String[]>(obj);
			String[] testObj = (String[])obj;
			Assert.Equal(3, testObj.Length);
			Assert.Equal("Test1", testObj[0]);
			Assert.Equal("Test2", testObj[1]);
			Assert.Equal("Test3", testObj[2]);
		}

		[Fact]
		public void Should_Convert_Array_From_Null()
		{
			// arrange
			var targetType = typeof(String[]);
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(null, targetType);

			// assert
			Assert.NotNull(obj);
			Assert.IsType<String[]>(obj);
			var array = (String[])obj;
			Assert.Equal(0, array.Length);
		}

		[Fact]
		public void Should_Convert_Array_Of_Char()
		{
			// arrange
			var targetType = typeof(Char[]);
			var value = "a;b;µ";
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<Char[]>(obj);
			Char[] testObj = (Char[])obj;
			Assert.Equal(3, testObj.Length);
			Assert.Equal('a', testObj[0]);
			Assert.Equal('b', testObj[1]);
			Assert.Equal('µ', testObj[2]);
		}

		[Fact]
		public void Should_Convert_Array_Of_String()
		{
			// arrange
			var targetType = typeof(String[]);
			var value = "1;2;Test";
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<String[]>(obj);
			String[] testObj = (String[])obj;
			Assert.Equal(3, testObj.Length);
			Assert.Equal("1", testObj[0]);
			Assert.Equal("2", testObj[1]);
			Assert.Equal("Test", testObj[2]);
		}

		[Fact]
		public void Should_Convert_Array_Of_Int()
		{
			// arrange
			var targetType = typeof(Int64[]);
			var value = "1;2;3";
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<Int64[]>(obj);
			Int64[] testObj = (Int64[])obj;
			Assert.Equal(3, testObj.Length);
			Assert.Equal(1, testObj[0]);
			Assert.Equal(2, testObj[1]);
			Assert.Equal(3, testObj[2]);
		}

		[Fact]
		public void Should_Convert_Array_Of_Float()
		{
			// arrange
			var targetType = typeof(Single[]);
			var value = "1;2;3";
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<Single[]>(obj);
			Single[] testObj = (Single[])obj;
			Assert.Equal(3, testObj.Length);
			Assert.InRange(testObj[0], 0.9, 1.1);
			Assert.InRange(testObj[1], 1.9, 2.1);
			Assert.InRange(testObj[2], 2.9, 3.1);
		}

		[Fact]
		public void Should_Convert_Array_Of_DateTime()
		{
			// arrange
			var targetType = typeof(DateTime[]);
			var value = "0001-01-01T00:00:00;2000-01-01T12:00:00;9999-12-31T23:59:59.9999999";
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<DateTime[]>(obj);
			DateTime[] testObj = (DateTime[])obj;
			Assert.Equal(3, testObj.Length);
			Assert.Equal(DateTime.MinValue, testObj[0]);
			Assert.Equal(new DateTime(2000, 1, 1, 12, 0, 0), testObj[1]);
			Assert.Equal(DateTime.MaxValue, testObj[2]);
		}

		[Fact]
		public void Should_Convert_Array_Of_Byte()
		{
			// arrange
			var targetType = typeof(Byte[]);
			var value = "0;127;255";
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<Byte[]>(obj);
			Byte[] testObj = (Byte[])obj;
			Assert.Equal(3, testObj.Length);
			Assert.Equal(Byte.MinValue, testObj[0]);
			Assert.Equal((Byte)127, testObj[1]);
			Assert.Equal(Byte.MaxValue, testObj[2]);
		}

		[Fact]
		public void Should_Convert_Array_Of_Boolean()
		{
			// arrange
			var targetType = typeof(Boolean[]);
			var value = "true;false;true";
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<Boolean[]>(obj);
			Boolean[] testObj = (Boolean[])obj;
			Assert.Equal(3, testObj.Length);
			Assert.Equal(true, testObj[0]);
			Assert.Equal(false, testObj[1]);
			Assert.Equal(true, testObj[2]);
		}

		[Fact]
		public void Should_Convert_Array_Of_Enum()
		{
			// arrange
			var targetType = typeof(TestEnum[]);
			var value = "Mon;3;Wed";
			var vtc = new ValueTypeConverter();

			// act
			var obj = vtc.Convert(value, targetType);

			// assert
			Assert.IsType<TestEnum[]>(obj);
			TestEnum[] testObj = (TestEnum[])obj;
			Assert.Equal(3, testObj.Length);
			Assert.Equal(TestEnum.Mon, testObj[0]);
			Assert.Equal(TestEnum.Tue, testObj[1]);
			Assert.Equal(TestEnum.Wed, testObj[2]);
		}
		#endregion
	}
}