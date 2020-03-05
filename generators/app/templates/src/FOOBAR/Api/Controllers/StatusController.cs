using FOOBAR.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

namespace FOOBAR.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class StatusController : Controller
    {
        /// <summary>
        /// Get the global API status.
        /// </summary>
        /// <returns></returns>
        [HttpGet("ping")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StatusResponse), 200)]
        public IActionResult GetPing()
        {
            return Ok(new StatusResponse()
            {
                Status = Status.ok
            });
        }

        /// <summary>
        /// Retrieve the runtime configuration
        /// </summary>
        /// <returns></returns>
        [HttpGet("runtime")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IDictionary<string, Object>), 200)]
        public IActionResult GetRuntimeValues()
        {
            dynamic values = new ExpandoObject();

            values.releaseVersion = Environment.GetEnvironmentVariable("RELEASE_VERSION");

            Process curProces = Process.GetCurrentProcess();
            if (curProces != null)
            {
                values.machineName = Environment.MachineName;
                values.hostName = System.Net.Dns.GetHostName();
                values.startTime = curProces.StartTime;
                values.threadCount = curProces.Threads?.Count ?? -1;
                values.processorTime = new
                {
                    user = curProces.UserProcessorTime.ToString(),
                    total = curProces.TotalProcessorTime.ToString()
                };
            }

            return new ObjectResult(values);
        }
    }
}
