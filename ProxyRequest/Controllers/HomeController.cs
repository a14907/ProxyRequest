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
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private HttpClient _client;

        public HomeController(IHttpClientFactory httpClientFactory
            , ILogger<HomeController> logger)
        {
            _client = httpClientFactory.CreateClient();
            _logger = logger;
        }

        [HttpGet("~/")]
        public string Index()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff" + "(tubi=youtube,string scheme, string domain, string path, string query)");
        }

        [HttpGet("~/proxy")]
        public async Task<IActionResult> ProxyAsync(string scheme, string domain, string path, string query)
        {
            if (string.Equals(domain, "tubi", StringComparison.OrdinalIgnoreCase))
            {
                domain = "www.youtube.com";
            }
            string url = string.Empty;
            if (string.IsNullOrEmpty(query))
            {
                url = $"{scheme}://{domain}{path}";
            }
            else
            {
                url = $"{scheme}://{domain}{path}?{query}";
            }

            var response = await _client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            _logger.LogWarning($"当前请求地址：{url}，响应code：{response.StatusCode}，响应body长度：{body.Length}");
            return new ContentResult()
            {
                StatusCode = (int)response.StatusCode,
                Content = body,
                ContentType = response.Content.Headers.ContentType.ToString()
            };
        }
    }
}
