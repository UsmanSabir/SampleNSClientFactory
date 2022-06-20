using System.Diagnostics;
using CommunicationServiceAbstraction;
using Microsoft.AspNetCore.Mvc;
using Sample.Contracts;

namespace SampleClientMicroService.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
       private readonly ILogger<SampleController> _logger;
        private readonly IClientFactory _clientFactory;

        public SampleController(ILogger<SampleController> logger, IClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        [HttpGet(Name = "GetBusinessPartner")]
        public async Task<IActionResult> Get()
        {
            var businessPartnerService = _clientFactory.CreateClient<IBusinessPartnerService>();
            businessPartnerService.NotifyEventId(3);
            var bpModel = businessPartnerService.GetById(1);
            Debug.WriteLine(bpModel.Id + " : " + bpModel.Name);
            bpModel = await businessPartnerService.GetByIdAsync(1);
            Debug.WriteLine(bpModel.Id + " : " + bpModel.Name);
            bpModel.Name = "abc";
            await businessPartnerService.UpdateModelAsync(3, "Temp", bpModel);

            bpModel = businessPartnerService.UpdateModel(3, "Temp", bpModel);
            Debug.WriteLine(bpModel.Id + " : " + bpModel.Name);

            return Ok(bpModel);
        }
    }
