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
        [Route("asyncronous")]
        public IActionResult PostAsyncronous()
        {
            Task.Run(() => DoWork());

            return Ok();
        }

        [HttpPost]
        [Route("hangfire")]
        public IActionResult PostWithHangfire()
        {
            BackgroundJob.Enqueue(() => DoWork());

            return Ok();
        }

        public void DoWork() 
        {
            var milliseconds = new Random().Next(300, 500);
            Thread.Sleep(milliseconds);
            _logger.LogInformation("\nWork Done!");
        }
    }
}
