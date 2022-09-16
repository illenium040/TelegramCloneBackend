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
    }

    public class RequestResult<T>
    {
        public bool Succeeded { get; set; } = false;
        public IEnumerable<string> Errors { get; set; }
        public HttpStatusCode? Status { get; set; }
        public T Data { get; set; }
    }
}
