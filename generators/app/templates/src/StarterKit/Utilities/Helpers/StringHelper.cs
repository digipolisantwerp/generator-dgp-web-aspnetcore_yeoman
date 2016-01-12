using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Digipolis.Helpers
{
    public class StringHelper
    {
		public static string Camelize(string input)
		{
			return Char.ToLower(input[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
		}

		public static string Pascalize(string input)
		{
			return Char.ToUpper(input[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
		}

		public static string ToCamelCase(string input)
		{
			if ( String.IsNullOrWhiteSpace(input) ) return input;

			if ( input.Length < 2 )
			{
				if ( !Char.IsUpper(input[0]) )
				{
					return input;
				}
				else
				{
					return Camelize(input);
				}
			}

			var clean = Regex.Replace(input, @"[\W]", " ");

			var words = clean.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
			var result = "";
			for ( int i = 0; i < words.Length; i++ )
			{
				var currentWord = words[i];
				if ( i == 0 )
					result += Camelize(currentWord);
				else
					result += Pascalize(currentWord);

				if ( currentWord.Length > 1 ) result += currentWord.Substring(1);
			}

			return result;
		}
	}
}