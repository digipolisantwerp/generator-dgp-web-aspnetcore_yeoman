using System;

namespace Digipolis.WebApi
{
    public class BadRootObjectException : RootObjectException
    {
        public BadRootObjectException(string message, string typeName) : base(message, typeName)
        { }
    }
}