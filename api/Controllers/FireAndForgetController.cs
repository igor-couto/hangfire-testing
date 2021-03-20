using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FireAndForgetController : ControllerBase
    {
        private readonly ILogger<FireAndForgetController> _logger;

        public FireAndForgetController(ILogger<FireAndForgetController> logger)
            => _logger = logger;
        
        [HttpPost]
        [Route("syncronous")]
        public IActionResult Post()
        {
            DoWork();
            return Ok();
        }

        [HttpPost]
        [Route("hangfire")]
        public IActionResult PostWithHangfire()
        {
            BackgroundJob.Enqueue(() => DoWork());
            return Ok();
        }

        [HttpPost]
        [Route("asyncronous")]
        public IActionResult PostAsyncronous()
        {
            var tasks = new[]
            {
                Task.Run(() => DoWork())
            };

            return Ok();
        }

        public void DoWork() 
        {
            var seconds = new Random().Next(1, 5);
            Thread.Sleep(seconds * 1000);
            _logger.LogInformation("\nWork Done!");
        }
    }
}
