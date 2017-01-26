using Digipolis.Errors;
using System;
using System.Net;

namespace StarterKit
{
    public class ApiExceptionMapper : ExceptionMapper
    {
        protected override void Configure()
        {
            CreateMap<UnauthorizedAccessException>((error, ex) =>
            {
                error.Title = "Access denied.";
                error.Code = "UNAUTH001";
                error.Status = (int)HttpStatusCode.Unauthorized;
            });
        }

        protected override void CreateDefaultMap(Error error, Exception exception)
        {
            error.Status = (int)HttpStatusCode.InternalServerError;
            error.Title = "We are experiencing some technical difficulties.";
        }
    }
}
