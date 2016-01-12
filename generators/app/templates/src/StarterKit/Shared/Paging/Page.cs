using System.Collections.Generic;
using Digipolis.WebApi;

namespace StarterKit
{
	[RootObject("page")]
	public class Page<TModel>
	{
		public IEnumerable<TModel> Lijst { get; set; }
		public long TotaalAantal { get; set; }
	}
}