using System;
using System.Reflection;

namespace SmartDev.ConfigurationMapper
{
	static class TypeHelper
	{
		public static bool IsEnum(Type type)
		{
#if ASPNETCORE50
			return type.GetTypeInfo().IsEnum;
#else
			return type.IsEnum;
#endif
		}

		public static PropertyInfo[] GetProperties(Type type)
		{
			// in aspnet50 (common CLR) this is a normal method on Type
			// in aspnetcore50 this is an extension method in System.Reflection
			return type.GetProperties();
		}
	}
}