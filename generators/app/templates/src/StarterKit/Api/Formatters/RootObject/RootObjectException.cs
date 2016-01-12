using System;

namespace Digipolis.WebApi
{
	public class RootObjectException : Exception
    {
        public RootObjectException(string message, string typeName) : base(message)
        {
            this.TypeName = typeName;
        }

        public string TypeName { get; set; }
    }
}