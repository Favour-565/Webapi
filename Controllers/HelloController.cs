using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webapi.Repository;
using System.Threading.Tasks;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly IVisitorInfoRepository _visitorInfoRepository;
        private readonly ILogger<HelloController> _logger;

        public HelloController(IVisitorInfoRepository visitorInfoRepository, ILogger<HelloController> logger)
        {
            _visitorInfoRepository = visitorInfoRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetHello([FromQuery] string visitor_name)
        {
            string clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            var visitorInfo = await _visitorInfoRepository.GetVisitorInfoAsync(visitor_name, clientIp);

            var response = new
            {
                client_ip = visitorInfo.ClientIp,
                location = visitorInfo.Location,
                greeting = $"Hello, {visitor_name}! The temperature is {visitorInfo.Temperature} degrees Celsius in {visitorInfo.Location}."
            };

            return Ok(response);
        }
    }
}
