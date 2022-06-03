using System.Diagnostics;
using CommunicationServiceAbstraction;
using Microsoft.AspNetCore.Mvc;
using Sample.Contracts;

namespace SampleHostMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;
        
        public SampleController(ILogger<SampleController> logger)
        {
            _logger = logger;            
        }

        [HttpGet(Name = "GetBusinessPartner")]
        public async Task<IActionResult> Get()
        {
            var res = new { Id = 5, Name = "User" };
            return Ok(res);
        }
    }
}