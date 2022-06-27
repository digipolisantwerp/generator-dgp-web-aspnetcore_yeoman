using System;
using Digipolis.Errors;

namespace FOOBAR.Shared.Exceptions
{
	public class ExceptionLogMessage
	{
		public Error Error { get; set; }
		public Exception Exception { get; set; }
		public string ExceptionInfo { get; set; }
	}
}
