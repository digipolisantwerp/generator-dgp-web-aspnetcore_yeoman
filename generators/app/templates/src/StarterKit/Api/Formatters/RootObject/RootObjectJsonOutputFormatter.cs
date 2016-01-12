using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace Digipolis.WebApi
{
	public class RootObjectJsonOutputFormatter : JsonOutputFormatter
    {
        public override Task WriteResponseBodyAsync(OutputFormatterContext context)
        {
            var root = RootObjectHelper.GetRootFieldName(context.Object.GetType(), false, this.SerializerSettings.ContractResolver);
            var obj = new ExpandoObject() as IDictionary<string, Object>;
            obj[root] = context.Object;
			context.Object = obj as object;
            return base.WriteResponseBodyAsync(context);
        }
    }
}