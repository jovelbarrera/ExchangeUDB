using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Kadevjo.Core.Dependencies;
using Kadevjo.Core.Models;
using System;

namespace Kadevjo.Core.Services
{
    public abstract class FlurlService<I, T> : RestService<I, T>
        where I : IService<T>
        where T : Model
    {
        private FlurlClient client;

        public FlurlService()
        {
            client = new FlurlClient();
            if (Headers != null && Headers.Count > 0)
            {
                foreach (var header in Headers)
                {
                    client = client.WithHeader(header.Key, header.Value);
                }
            }
        }

        protected async override Task<R> Execute<R>(string resource, IQuery query = null)
        {
            var url = BaseUrl.AppendPathSegment(resource);

            if (query != null && query.FormattedParameters != null && query.Parameters.Count > 0)
            {
                foreach (var param in query.FormattedParameters)
                {
                    url.SetQueryParam(param.Key, param.Value);
                }
            }

            try
            {
                var response = await url.WithClient(client)
                                        .GetAsync()
                                        .ReceiveJson<R>();
                return response;
            }
            catch (FlurlHttpException e)
            {
                Debug.WriteLine("Request failed: " + e.Call.Request);
                Debug.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                var a = ex;
            }
            return default(R);
        }

        protected async override Task<bool> Execute<B>(
            string resource, HttpMethod method, B body = default(B))
        {
            var url = BaseUrl.AppendPathSegment(resource);

            Task<HttpResponseMessage> task = null;

            try
            {
                if (method == HttpMethod.Post)
                {
                    if (body != null)
                    {
                        task = url.WithClient(client).PostJsonAsync(body);
                    }
                    else
                    {
                        task = url.WithClient(client).PostAsync();
                    }
                }
                else if (method == HttpMethod.Put)
                {
                    if (body != null)
                    {
                        task = url.WithClient(client).PutJsonAsync(body);
                    }
                    else
                    {
                        task = url.WithClient(client).PutAsync();
                    }
                }
                else if (method == HttpMethod.Delete)
                {
                    task = url.WithClient(client).DeleteAsync();
                }

                var response = await task;
                return response.IsSuccessStatusCode;
            }
            catch (FlurlHttpException e)
            {
                Debug.WriteLine("Request failed: " + e.Call.Request
                                + " " + e.Call.RequestBody);
                Debug.WriteLine(e.Message);
                return false;
            }

        }

        protected async override Task<GenericResponse<R>> Execute<R, B>(
            string resource, HttpMethod method, B body = default(B))
        {

            var url = BaseUrl.AppendPathSegment(resource);
            Task<HttpResponseMessage> task = null;

            try
            {
                if (method == HttpMethod.Post)
                {
                    if (body != null)
                    {
                        task = url.WithClient(client).PostJsonAsync(body);
                    }
                    else
                    {
                        task = url.WithClient(client).PostAsync();
                    }
                }
                else if (method == HttpMethod.Put)
                {
                    // BUG ?
                    if (body != null)
                    {
                        task = url.WithClient(client).PutJsonAsync(body);
                    }
                    else
                    {
                        task = url.WithClient(client).PutAsync();
                    }
                }
                else if (method == HttpMethod.Delete)
                {
                    task = url.WithClient(client).DeleteAsync();
                }
                else if (method == HttpMethod.Get)
                {
                    task = url.WithClient(client).GetAsync();
                }
                else if (method == new HttpMethod("PATCH"))
                {
                    // BUG ?
                    if (body != null)
                    {
                        task = url.WithClient(client).PatchJsonAsync(body);
                    }
                    else
                    {
                        task = url.WithClient(client).PatchAsync();
                    }
                }

                var preResponse = await task.ReceiveJson<R>();
                var response = new GenericResponse<R>
                {
                    Model = preResponse,
                    Status = System.Net.HttpStatusCode.OK,
                    Message = "Ok"
                };
                return response;
            }
            catch (FlurlHttpException e)
            {
                Debug.WriteLine("Request failed: " + e.Call.Request
                                + " " + e.Call.RequestBody);
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.GetResponseString());

                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                Encoding utf16 = Encoding.Unicode;
                byte[] utfBytes = utf16.GetBytes(e.GetResponseString());
                byte[] isoBytes = Encoding.Convert(utf16, iso, utfBytes);
                string message = iso.GetString(isoBytes, 0, isoBytes.Length - 1);


                var response = new GenericResponse<R>
                {
                    Model = default(R),
                    Status = e.Call.Response.StatusCode,
                    Message = e.GetResponseString()
                };
                return response;
            }
        }
    }
}