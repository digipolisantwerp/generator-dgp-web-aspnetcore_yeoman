using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Serialization;
using Digipolis.Helpers;

namespace Digipolis.WebApi
{
	public static class RootObjectHelper
    {

		public static string GetRootFieldName(Type type, bool inList, IContractResolver resolver)
		{
			if (typeof(IEnumerable).IsAssignableFrom(type) && !inList)
			{
				var underlyingType = type.GetGenericArguments().FirstOrDefault();
				if (underlyingType == null) return GetRootFieldName(type, true, resolver);

				var rootName = GetRootFieldName(underlyingType, true, resolver);
				return rootName;
			}

			var result = String.Empty;
			var typeName = type.Name;

			var attributeType = inList ? typeof(RootListObjectAttribute) : typeof(RootObjectAttribute);
			var attributes = type.CustomAttributes.Where(x => x.AttributeType == attributeType).ToList();

			if (attributes.Count == 0)
			{
				if (inList) result = "ListOf";
				result += typeName;
			}
			else
			{
				var name = attributes.First().ConstructorArguments.First();
				result = name.Value.ToString();
			}

			return ToCamelCaseIfNeeded(result, resolver);
		}

		private static string ToCamelCaseIfNeeded(string input, IContractResolver resolver)
		{
			if (resolver == null) return input;

			if (resolver.GetType().IsAssignableFrom(typeof(CamelCasePropertyNamesContractResolver)))
				return StringHelper.ToCamelCase(input);
			else
				return input;
		}
	}
}