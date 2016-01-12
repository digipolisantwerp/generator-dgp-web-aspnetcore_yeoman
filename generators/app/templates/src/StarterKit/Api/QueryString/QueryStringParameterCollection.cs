using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNet.WebUtilities;

namespace Digipolis.WebApi
{
	public class QueryStringParameterCollection : IReadOnlyDictionary<string, string[]>
	{
		public QueryStringParameterCollection(string queryString)
		{
			if ( queryString == null ) throw new ArgumentNullException("queryString", "queryString is null.");
			_queryParts = new Dictionary<string, string[]>(QueryHelpers.ParseQuery(queryString), StringComparer.OrdinalIgnoreCase);
		}

		private readonly IDictionary<string, string[]> _queryParts;

		public IEnumerable<string> Keys
		{
			get { return _queryParts.Keys; }
		}

		public IEnumerable<string[]> Values
		{
			get { return _queryParts.Values; }
		}

		public int Count
		{
			get { return _queryParts.Count; }
		}

		public string[] this[string key]
		{
			get { return _queryParts[key]; }
		}

		public bool ContainsKey(string key)
		{
			return _queryParts.ContainsKey(key);
		}

		public bool TryGetValue(string key, out string[] value)
		{
			return _queryParts.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator()
		{
			return _queryParts.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _queryParts.GetEnumerator();
		}
	}
}