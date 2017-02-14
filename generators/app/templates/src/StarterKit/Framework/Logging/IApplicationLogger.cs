using System;
using Microsoft.Extensions.Logging;
using StarterKit.Logging;

namespace StarterKit
{
    public interface IApplicationLogger : ILogger<ApplicationLogger>
    { }
}
