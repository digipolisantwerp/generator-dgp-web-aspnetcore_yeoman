using System;

namespace Digipolis.WebApi
{
    public class RootObjectMissingException : RootObjectException
    {
        public RootObjectMissingException(string message, string typeName) : base(message, typeName)
        { }
    }
}