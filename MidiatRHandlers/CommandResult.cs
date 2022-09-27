using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CQRSLayer
{
    public class CommandResult
    {
        public bool Succeeded { get; set; } = false;
        public IEnumerable<string> Errors { get; set; }
        public HttpStatusCode? Status { get; set; }

        public static CommandResult BadRequest(IEnumerable<string> errors = null)
        {
            return new CommandResult
            {
                Errors = errors,
                Succeeded = false,
                Status = HttpStatusCode.BadRequest
            };
        }

        public static CommandResult NotAuthorize(IEnumerable<string> errors = null)
        {
            return new CommandResult
            {
                Errors = errors,
                Succeeded = false,
                Status = HttpStatusCode.Unauthorized
            };
        }

        public static CommandResult OK()
        {
            return new CommandResult
            {
                Succeeded = true,
                Status = HttpStatusCode.OK
            };
        }
    }

    public class CommandResult<T>
    {
        public T Data { get; set; }
        public CommandResult Result { get; set; }
    }
}
