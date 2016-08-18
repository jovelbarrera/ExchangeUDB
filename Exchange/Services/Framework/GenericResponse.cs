using System;
using System.Net;

namespace Kadevjo.Core.Models
{
    public class GenericResponse<T>
    {
        public T Model { get; set; }
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

    }
}