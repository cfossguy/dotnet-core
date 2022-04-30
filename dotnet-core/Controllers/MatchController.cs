using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        [Route("slow/{size:int}/{delay:int}")] 
        [HttpGet] 
        public string Slow(int size, int delay)
        {
            _logger.LogWarning(String.Format("/slow api that has a {0}ms delay",delayTime));
            Thread.Sleep(delay);
            _logger.LogInformation("/slow api that returns a fixed length string");
            return getMockResponse(size);
        }
        
        [Route("fast/{size:int}")] 
        [HttpGet] 
        public string Fast(int size)
        {
            _logger.LogInformation("/fast api that returns a fixed length string");
            return getMockResponse(size);
        }
        
        [Route("roulette")] 
        [HttpGet] 
        public IActionResult Roulette()
        {
            Random rnd = new Random();
            int number   = rnd.Next(1, 14);   // creates a number between 1 and 13
            
            _logger.LogWarning(String.Format("/roulette api wheel lands on {0}",number));

            if (number == 13)
            {
                _logger.LogError("/roulette api says you have very bad luck!");
                return StatusCode(500);
            }
            
            return new OkObjectResult(new { message = "You have very good luck!", number = number });
        }
        
        [Route("helloworld")] 
        [HttpGet] 
        public string Helloworld()
        {
            return "Helloworld-1.1...";
        }
        
        private string getMockResponse(int s) {
            string phrase = "All work and no play makes .netcore a slow API";
            string response = "";
            for(int i=0; i<s; i++) {
                response += phrase + "\n";
                Thread.Sleep(10);
            }

            return response;
        }
    }
}