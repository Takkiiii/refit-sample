using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace RefitSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var uri = new Uri("http://takkiiii0204.com/");
            var extected = HttpStatusCode.Accepted;

            var mockResponse = new HttpResponseMessage(extected)
                               {
                                    Content = new StringContent($@"[""name""]")
                               };
            
            var handler = new MockResponseHandler(mockResponse);
            using (var http = new HttpClient(handler))
            {
                http.BaseAddress = uri;
                var api =  RestService.For<IApi>(http);
                try
                {
                    var ret = await api.GetUsers();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }

    public interface IApi
    {
        [Get("/users")]
        Task<List<string>> GetUsers();
    }

    public class MockResponseHandler : DelegatingHandler
    {
        private readonly HttpResponseMessage fakeMessage;

        public MockResponseHandler(HttpResponseMessage fakeMessage)
        {
            this.fakeMessage = fakeMessage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return this.fakeMessage;
        }
    }
}