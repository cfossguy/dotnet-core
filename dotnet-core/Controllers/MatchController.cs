﻿using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_core.Controllers
{
    [ApiController]
    [Route("match")]
    public class MatchController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;
        
        public MatchController(ILogger<MatchController> logger)
        {
            _logger = logger;
        }

        [Route("slow/{delay:int}")] 
        [HttpGet] 
        public async Task<string?> Slow(int delay)
        {
            _logger.LogWarning(String.Format("/slow api that has a {1}ms delay traceID={0}", Activity.Current.RootId,delay));
            Thread.Sleep(delay);
            
            _logger.LogInformation("/slow api that returns a fixed length string traceID={0}",Activity.Current.RootId);
            
            _logger.LogInformation("/slow making a call to 3rd party service traceID={0}",Activity.Current.RootId);
            var rslt = await Call3rdPartyAPI();
            if (rslt != null && rslt.Length > 0)
            {
                _logger.LogInformation("/slow 3rd party service returned {1} length string traceID={0}",Activity.Current.RootId,rslt.Length);
                return GetMockResponse(rslt.Length / 1000);
            }
            return GetMockResponse(delay);
        }
        
        [Route("fast/{size:int}")] 
        [HttpGet] 
        public string Fast(int size)
        {
            _logger.LogInformation("/fast api that returns a fixed length string traceID={0}",Activity.Current.RootId);
            return GetMockResponse(size);
        }

        [Route("roulette")]
        [HttpGet]
        public IActionResult Roulette()
        {
            Random rnd = new Random();
            int number = rnd.Next(1, 14);
            Thread.Sleep(number * 50);
            _logger.LogWarning("/roulette api wheel lands on {1} traceID={0}",Activity.Current.RootId, number);

            if (number == 13)
            {
                _logger.LogError("/roulette api says you have very bad luck! traceID={0}",Activity.Current.RootId);
                return StatusCode(500);
            }

            return new OkObjectResult(new {message = GetMockResponse(number), number = number});
        }

        private string GetMockResponse(int s) {
            string phrase = "All work and no play makes .netcore a slow API ";
            string response = "";
            Random rnd = new Random();
            int number   = rnd.Next(1, s);   
            
            for(int i=0; i<number; i++) {
                var myActivitySource = new ActivitySource("dotnet-core");
                using var activity = myActivitySource.StartActivity("bad code loop");
                response += phrase + "\n";
                Thread.Sleep(number);
            }

            return response;
        }

        // GET: Products
        private async Task<string?> Call3rdPartyAPI()
        {
            string apiUrl = "https://bumble.com";
            var myActivitySource = new ActivitySource("dotnet-core");
            using var activity = myActivitySource.StartActivity("bumble");
            using (HttpClient client=new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var buffer = await response.Content.ReadAsStreamAsync();
                    //Stream decompressed = new GZipStream(buffer, CompressionMode.Decompress);
                    StreamReader objReader = new StreamReader(buffer, Encoding.UTF8);
                    _logger.LogInformation("call to 3rd party API was successful traceID={0}",Activity.Current.RootId);
                    
                    return objReader.ReadToEnd();
                    
                }
                activity?.SetTag("http.status_code", response.StatusCode);
                activity?.SetTag("http.route", apiUrl);
                _logger.LogError("call to 3rd party API failed traceID={0}",Activity.Current.RootId);
                return null;
            }
        }
    }
}