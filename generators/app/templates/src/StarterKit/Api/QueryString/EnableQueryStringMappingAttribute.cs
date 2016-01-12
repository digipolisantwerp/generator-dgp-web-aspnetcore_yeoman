using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace Digipolis.WebApi
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class EnableQueryStringMappingAttribute : Attribute, IActionConstraint
	{
		public int Order
		{
			get { return Int32.MaxValue; }
		}

		public bool Accept(ActionConstraintContext context)
		{
			var queryString = context.RouteContext.HttpContext.Request.QueryString.ToUriComponent();

			var zonderVraagteken = queryString.StartsWith("?") ? queryString.Substring(1) : queryString;
			var queryData = new QueryStringParameterCollection(zonderVraagteken);

			var exactCandidates = context.Candidates.Where(c => c.Action.Parameters.Count == queryData.Count);
			var selectedCandidate = SelectExactCandidate(exactCandidates, queryData);
			if ( context.CurrentCandidate.Action == selectedCandidate?.Action )	return true;

			var overloadedCandidates = context.Candidates.Where(c => c.Action.Parameters.Count > queryData.Count)
														 .OrderByDescending(c => c.Action.Parameters.Count);

			selectedCandidate = SelectOverloadedCandidate(overloadedCandidates, queryData);
			if ( context.CurrentCandidate.Action == selectedCandidate?.Action ) return true;

			return false;
		}

		private ActionSelectorCandidate SelectExactCandidate(IEnumerable<ActionSelectorCandidate> candidates, QueryStringParameterCollection queryData)
		{
			foreach ( var candidate in candidates )
			{
				var action = candidate.Action;

				var isMatch = true;
				foreach ( var actionParam in action.Parameters )
				{
					var nameProvider = actionParam.BindingInfo as IModelNameProvider;
					var name = nameProvider?.Name ?? actionParam.Name;

					if ( queryData.ContainsKey(name) )
					{
						isMatch = HasFromQueryAttribute(actionParam);
						if ( !isMatch ) break;

						if ( queryData[name].Count() > 1 && !IsCollection(actionParam.ParameterType) )
						{
							isMatch = false;
							break;
						}
					}
					else
					{
						isMatch = false;
						break;
					}
				}

				if ( isMatch ) return candidate;
			}

			return null;
		}

		private ActionSelectorCandidate SelectOverloadedCandidate(IEnumerable<ActionSelectorCandidate> candidates, QueryStringParameterCollection queryData)
		{
			foreach ( var candidate in candidates )
			{
				var action = candidate.Action;

				var isMatch = true;
				var exactCount = 0;
				foreach ( var actionParam in action.Parameters )
				{
					var nameProvider = actionParam.BindingInfo as IModelNameProvider;
					var name = nameProvider?.Name ?? actionParam.Name;

					if ( queryData.ContainsKey(name) )
					{
						if ( HasFromQueryAttribute(actionParam) )
						{
							if ( queryData[name].Count() > 1 && !IsCollection(actionParam.ParameterType) )
							{
								isMatch = false;
								break;
							}

							exactCount++;
						}
						else
						{
							// parameter met juiste naam maar zonder FromQueryAttribute
							isMatch = false;
							break;
						}
					}
					else
					{
						if ( HasFromQueryAttribute(actionParam) )
						{
							// parameter was verwacht in de query string
							isMatch = false;
							break;
						}
					}
				}

				if ( isMatch && exactCount == queryData.Count ) return candidate;
			}

			return null;

		}


		private bool HasFromQueryAttribute(ParameterDescriptor actionParam)
		{
			var source = actionParam.BindingInfo?.BindingSource;
			if ( source == null ) return false;

			var type = actionParam.ParameterType;
			return source.CanAcceptDataFrom(BindingSource.Query);
		}

		private bool IsCollection(Type type)
		{
			if ( type == typeof(string) ) return false;		// alleen echte collections
			return typeof(IEnumerable).IsAssignableFrom(type);
		}
	}
}