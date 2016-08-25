using System;
using System.Net;
using System.Net.Http;

namespace Kadevjo.Core.Models
{
    public class GenericResponse<T>
    {
        public T Model { get; set; }
        public HttpResponseMessage Status { get; set; }

    }
}