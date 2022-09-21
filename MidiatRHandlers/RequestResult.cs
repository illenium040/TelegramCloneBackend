using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MidiatRHandlers
{
    public class RequestResult
    {
        public bool Succeeded { get; set; } = false;
        public IEnumerable<string> Errors { get; set; }
        public HttpStatusCode? Status { get; set; }

        public static RequestResult BadRequest(IEnumerable<string> errors = null)
        {
            return new RequestResult
            {
                Errors = errors,
                Succeeded = false,
                Status = HttpStatusCode.BadRequest
            };
        }

        public static RequestResult NotAuthorize(IEnumerable<string> errors = null)
        {
            return new RequestResult
            {
                Errors = errors,
                Succeeded = false,
                Status = HttpStatusCode.Unauthorized
            };
        }

        public static RequestResult OK()
        {
            return new RequestResult
            {
                Succeeded = true,
                Status = HttpStatusCode.OK
            };
        }
    }

    public class RequestResult<T> : RequestResult
    {
        public T Data { get; set; }
    }
}
