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
            _logger.LogWarning(String.Format("/slow api that has a {0}ms delay",delay));
            Thread.Sleep(delay);
            _logger.LogInformation("/slow api that returns a fixed length string");
            
            _logger.LogInformation("/slow making a call to 3rd party service");
            var rslt = await Call3rdPartyAPI();
            if (rslt != null && rslt.Length > 0)
            {
                _logger.LogInformation("/slow 3rd party service returned {0} length string",rslt.Length);
                return GetMockResponse(rslt.Length / 1000);
            }
            return GetMockResponse(size);
        }
        
        [Route("fast/{size:int}")] 
        [HttpGet] 
        public string Fast(int size)
        {
            _logger.LogInformation("/fast api that returns a fixed length string");
            return GetMockResponse(size);
        }

        [Route("roulette")]
        [HttpGet]
        public IActionResult Roulette()
        {
            Random rnd = new Random();
            int number = rnd.Next(1, 14);
            Thread.Sleep(number * 50);
            _logger.LogWarning(String.Format("/roulette api wheel lands on {0}", number));

            if (number == 13)
            {
                _logger.LogError("/roulette api says you have very bad luck!");
                return StatusCode(500);
            }

            return new OkObjectResult(new {message = GetMockResponse(number), number = number});
        }

        private string GetMockResponse(int s) {
            string phrase = "All work and no play makes .netcore a slow API";
            string response = "";
            Random rnd = new Random();
            int number   = rnd.Next(1, s);   
            for(int i=0; i<number; i++) {
                response += phrase + "\n";
                Thread.Sleep(number);
            }

            return response;
        }

        // GET: Products
        private async Task<string?> Call3rdPartyAPI()
        {
            string apiUrl = "https://bumble.com";

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
                    _logger.LogInformation("call to 3rd party API was successful");
                    return objReader.ReadToEnd();
                    
                }
                _logger.LogError("call to 3rd party API failed");
                return null;
            }
        }
    }
}