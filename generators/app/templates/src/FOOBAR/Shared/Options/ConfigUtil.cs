using System;
using System.Collections.Generic;

namespace FOOBAR.Shared.Options
{
	public static class ConfigUtil
	{
		private static string GetEnvironmentVariable(string variableKey, bool canBeNullOrWhiteSpace)
		{
			var value = Environment.GetEnvironmentVariable(variableKey);

			if (string.IsNullOrWhiteSpace(value) && !canBeNullOrWhiteSpace)
			{
				throw new ArgumentNullException($"Configuration error: invalid parameter {variableKey}");
			}

			return value;
		}

		public static string GetEnvironmentVariable(string variableKey, bool canBeNullOrWhiteSpace = false,
			string defaultWhenNullOrWhiteSpace = "")
		{
			var stringValue = GetEnvironmentVariable(variableKey, canBeNullOrWhiteSpace);

			if (string.IsNullOrWhiteSpace(stringValue)) return defaultWhenNullOrWhiteSpace;

			return stringValue;
		}

		public static int GetEnvironmentVariableAsInt(string variableKey, bool canBeNullOrWhiteSpace = false,
			int defaultWhenNullOrWhiteSpace = 0)
		{
			var stringValue = GetEnvironmentVariable(variableKey, canBeNullOrWhiteSpace);

			if (string.IsNullOrWhiteSpace(stringValue)) return defaultWhenNullOrWhiteSpace;

			if (!int.TryParse(stringValue, out var value))
			{
				throw new ArgumentNullException(
					$"Configuration error: parameter {variableKey} must have an integer value");
			}

			return value;
		}

		public static bool GetEnvironmentVariableAsBool(string variableKey, bool canBeNullOrWhiteSpace = false,
			bool defaultWhenNullOrWhiteSpace = false)
		{
			var stringValue = GetEnvironmentVariable(variableKey, canBeNullOrWhiteSpace);

			if (string.IsNullOrWhiteSpace(stringValue)) return defaultWhenNullOrWhiteSpace;

			if (!bool.TryParse(stringValue, out var value))
			{
				throw new ArgumentNullException($"Configuration error: parameter {variableKey} must be a boolean");
			}

			return value;
		}

		public static T GetEnvironmentVariable<T>(string variableKey, Func<string, T> convertToType,
			bool canBeNullOrWhiteSpace = false)
		{
			var stringValue = GetEnvironmentVariable(variableKey, canBeNullOrWhiteSpace);

			return convertToType(stringValue);
			;
		}

		public static void FillFromEnvironment(string variableKey, string dictionaryKey,
			Dictionary<string, string> environmentDict, bool canBeNullOrWhiteSpace = false)
		{
			var stringValue = GetEnvironmentVariable(variableKey, canBeNullOrWhiteSpace);

			environmentDict.Add(dictionaryKey, stringValue);
		}
	}
}
