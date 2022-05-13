namespace FOOBAR.Framework.Extensions
{
  public static class StringExtensions
  {
    public static string ToCamelCase(this string str)
    {
      if (string.IsNullOrWhiteSpace(str)) return str;
      var input = str.Trim();

      if (input.Length == 1) return str.ToLowerInvariant();

      return char.ToLowerInvariant(input[0]) + input.Substring(1);
    }
  }
}
