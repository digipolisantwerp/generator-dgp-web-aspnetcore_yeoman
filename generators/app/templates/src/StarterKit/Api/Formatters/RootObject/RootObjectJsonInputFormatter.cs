using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Digipolis.WebApi
{
	public class RootObjectJsonInputFormatter : JsonInputFormatter
	{

		public override Task<object> ReadRequestBodyAsync(InputFormatterContext context)
		{
			return Task.FromResult(ReadFromStream(context));
		}

		private object ReadFromStream(InputFormatterContext context)
		{

			var type = context.ModelType;
			var request = context.HttpContext.Request;
			MediaTypeHeaderValue requestContentType = null;
			MediaTypeHeaderValue.TryParse(request.ContentType, out requestContentType);

			// Get the character encoding for the content
			// Never non-null since SelectCharacterEncoding() throws in error / not found scenarios
			var effectiveEncoding = SelectCharacterEncoding(requestContentType);

			using (var streamReader = new StreamReader(request.Body, effectiveEncoding))
			{
				try
				{

					var rootName = RootObjectHelper.GetRootFieldName(type, false, this.SerializerSettings.ContractResolver);
					var jsonString = streamReader.ReadToEnd();
					var jsonObject = JObject.Parse(jsonString);
					var jsonToken = jsonObject.SelectToken(rootName);
					if (jsonToken == null)
						throw new RootObjectMissingException(String.Format("Root object met naam {0} ontbreekt in de input parameter. Als er toch een root object is, check dan of de spelling en casing kloppen.", rootName), type.Name);
					var dataObject = jsonToken.ToObject(type);
					if (dataObject == null)
						throw new BadRootObjectException("Input parameter volgt structuur met Root object niet.", type.Name);

					return dataObject;
				}
				catch (RootObjectException rootEx)
				{
					context.ModelState.TryAddModelError(String.Empty, rootEx);
					throw;
				}
				catch (JsonSerializationException jsonEx)
				{
					context.ModelState.TryAddModelError(String.Empty, jsonEx);
					throw new BadRootObjectException(String.Format("Input parameter volgt structuur met Root object niet : {0}", jsonEx.Message), type.Name);
				}
				catch (Exception ex)
				{
					context.ModelState.TryAddModelError(String.Empty, ex);
					throw;
				}
			}
		}
	}
}