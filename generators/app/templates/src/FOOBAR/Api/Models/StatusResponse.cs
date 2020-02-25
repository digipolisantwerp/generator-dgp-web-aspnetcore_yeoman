using System.ComponentModel.DataAnnotations;

namespace FOOBAR.Api.Models
{
    public class StatusResponse
    {
        /// <summary>
        /// The global status of the API.
        /// </summary>
        [Required]
        public Status Status { get; set; }
    }
}
