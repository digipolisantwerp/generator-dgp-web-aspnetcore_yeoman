using System;
using Microsoft.Extensions.Logging;
using FOOBAR.Logging;

namespace FOOBAR
{
    public interface IApplicationLogger : ILogger<ApplicationLogger>
    { }
}
