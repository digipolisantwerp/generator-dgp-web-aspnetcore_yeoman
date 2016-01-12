using System;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Routing;
using Digipolis.WebApi;
using Xunit;

namespace StarterKit.UnitTests.QueryStringMapping
{
    public class EnableQueryStringMappingTests
    {
		[Fact]
		public void AcceptOneArgumentWithCorrectName()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("argument_int", typeof(int), new FromQueryAttribute()) };
			
			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int=3");
						
			var accepted = mapper.Accept(context);

			Assert.True(accepted);		
		}

		[Fact]
		public void AcceptTwoArgumentsWithCorrectName()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>()
									{
										CreateArgument("argument_int_1", typeof(int), new FromQueryAttribute()),
										CreateArgument("argument_int_2", typeof(int), new FromQueryAttribute()),
									};

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int_1=3&argument_int_2=7");

			var accepted = mapper.Accept(context);

			Assert.True(accepted);
		}

		[Fact]
		public void AcceptTwoArgumentsInDifferentOrder()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>()
									{
										CreateArgument("argument_int_1", typeof(int), new FromQueryAttribute()),
										CreateArgument("argument_int_2", typeof(int), new FromQueryAttribute()),
									};

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int_2=6&argument_int_1=4");

			var accepted = mapper.Accept(context);

			Assert.True(accepted);
		}

		[Fact]
		public void RejectFromQueryArgumentWithWrongName()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("argument_int", typeof(int), new FromQueryAttribute()) };

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?wrong_name=3");

			var accepted = mapper.Accept(context);

			Assert.False(accepted);
		}

		[Fact]
		public void AcceptArgumentWithDifferentCasingInQueryString()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("argument_int", typeof(int), new FromQueryAttribute()) };

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?ARGUMENT_INT=3");

			var accepted = mapper.Accept(context);

			Assert.True(accepted);
		}

		[Fact]
		public void AcceptArgumentWithDifferentCasingInActionParameter()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("Argument_Int", typeof(int), new FromQueryAttribute()) };

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int=3");

			var accepted = mapper.Accept(context);

			Assert.True(accepted);
		}

		[Fact]
		public void RejectTooMuchArgumentsInQueryString()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("argument_int_1", typeof(int), new FromQueryAttribute()) };

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int_1=3&argument_int_2=5");

			var accepted = mapper.Accept(context);

			Assert.False(accepted);
		}

		[Fact]
		public void RejectNotEnoughArgumentsInQueryString()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>()
									{
										CreateArgument("argument_int_1", typeof(int), new FromQueryAttribute()),
										CreateArgument("argument_int_2", typeof(int), new FromQueryAttribute())
									};

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int_1=3");

			var accepted = mapper.Accept(context);

			Assert.False(accepted);
		}

		[Fact]
		public void AcceptOverloadedActionWithExpectedQueryString()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>()
									{
										CreateArgument("argument_int", typeof(int), new FromQueryAttribute()),
										CreateArgument("body_string", typeof(string), new FromBodyAttribute()),
									};

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int=3");

			var accepted = mapper.Accept(context);

			Assert.True(accepted);
		}

		[Fact]
		public void AcceptOverloadedActionWithoutQueryString()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>()
									{
										CreateArgument("body_int", typeof(int), new FromBodyAttribute()),
										CreateArgument("body_string", typeof(string), new FromBodyAttribute()),
									};

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("");

			var accepted = mapper.Accept(context);

			Assert.True(accepted);
		}

		[Fact]
		public void RejectNoArgumentsInQueryStringWhenExpected()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>()
									{
										CreateArgument("argument_int_1", typeof(int), new FromQueryAttribute()),
										CreateArgument("argument_int_2", typeof(int), new FromQueryAttribute())
									};

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("");

			var accepted = mapper.Accept(context);

			Assert.False(accepted);
		}

		[Fact]
		public void AcceptNoArgumentsInQueryStringWhenNoneExpected()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>();

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("");

			var accepted = mapper.Accept(context);

			Assert.True(accepted);
		}

		[Fact]
		public void RejectQueryStringArgumentWithoutBinderMetadata()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("argument_int", typeof(int), null) };

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int=3");

			var accepted = mapper.Accept(context);

			Assert.False(accepted);
		}

		[Fact]
		public void RejectQueryStringArgumentWithWrongBinderMetadata()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("argument_int", typeof(int), new FromBodyAttribute()) };

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int=3");

			var accepted = mapper.Accept(context);

			Assert.False(accepted);
		}

		[Fact]
		public void AcceptTwoArgumentsWithDifferentTypes()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>()
									{
										CreateArgument("argument_int", typeof(int), new FromQueryAttribute()),
										CreateArgument("argument_string", typeof(string), new FromQueryAttribute()),
									};

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int=3&argument_string=abc");

			var accepted = mapper.Accept(context);

			Assert.True(accepted);
		}

		[Fact]
		public void AcceptIntValueWhenStringExpected()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("argument_string" , typeof(string), new FromQueryAttribute()) };

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_string=3");

			var accepted = mapper.Accept(context);

			Assert.True(accepted);
		}

		[Fact]
		public void AcceptMultiValueIntInCollection()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("argument_int", typeof(IEnumerable<int>), new FromQueryAttribute()) };

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int=1&argument_int=2&argument_int=3");

			var accepted = mapper.Accept(context);

			Assert.True(accepted);
		}

		[Fact]
		public void AcceptMultiValueStringInCollection()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("argument_string", typeof(IEnumerable<string>), new FromQueryAttribute()) };

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_string=abc&argument_string=def&argument_string=xyz");

			var accepted = mapper.Accept(context);

			Assert.True(accepted);
		}

		[Fact]
		public void RejectMultiValueStringWithoutCollection()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("argument_string", typeof(string), new FromQueryAttribute()) };

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_string=abc&argument_string=def&argument_string=xyz");

			var accepted = mapper.Accept(context);

			Assert.False(accepted);
		}

		[Fact]
		public void RejectMultiValueIntWithoutCollection()
		{
			var action = new ActionDescriptor();
			action.Parameters = new List<ParameterDescriptor>() { CreateArgument("argument_int", typeof(int), new FromQueryAttribute()) };

			var mapper = new EnableQueryStringMappingAttribute();

			var context = new ActionConstraintContext();
			context.Candidates = new List<ActionSelectorCandidate>() { new ActionSelectorCandidate(action, new[] { mapper }) };
			context.CurrentCandidate = context.Candidates[0];
			context.RouteContext = CreateRouteContext("?argument_int=1&argument_int=2&argument_int=3");

			var accepted = mapper.Accept(context);

			Assert.False(accepted);
		}

		// ToDo : checken met FromQuery(Name=custom) als upgrade MVC packages is gebeurd

		private ParameterDescriptor CreateArgument(string name, Type type, IBindingSourceMetadata metadata)
		{
			return new ParameterDescriptor()
			{
				BindingInfo = BindingInfo.GetBindingInfo(new[] { metadata }),
				Name = name,
				ParameterType = type
			};
		}

		private RouteContext CreateRouteContext(string queryString = null, object routeValues = null)
		{
			var httpContext = new DefaultHttpContext();
			if ( queryString != null )
			{
				httpContext.Request.QueryString = new QueryString(queryString);
			}

			var routeContext = new RouteContext(httpContext);
			routeContext.RouteData = new RouteData();

			foreach ( var kvp in new RouteValueDictionary(routeValues) )
			{
				routeContext.RouteData.Values.Add(kvp.Key, kvp.Value);
			}

			return routeContext;
		}
	}
}