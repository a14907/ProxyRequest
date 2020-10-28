using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ProxyRequest.Controllers
{
    [ApiController]
    [Route("[proxy]")]
    public class HomeController : ControllerBase
    {
        private HttpClient _client;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> ProxyAsync(string url)
        {
            var response = await _client.GetAsync(url);
            var str = await response.Content.ReadAsStringAsync();
            return new ContentResult()
            {
                StatusCode = (int)response.StatusCode,
                Content = str,
                ContentType = response.Content.Headers.ContentType.ToString()
            };
        }
    }
}
