using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SagaExample.Gateways.Handlers;
using System.Threading.Tasks;

namespace SagaExample.Gateways.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhoIsController : ControllerBase
    {
        private readonly INotifierMediatorService _notifierMediatorService;
        private readonly IPublishEndpoint _publishEndpoint;

        public WhoIsController(INotifierMediatorService notifierMediatorService,
            IPublishEndpoint publishEndpoint)
        {
            _notifierMediatorService = notifierMediatorService ?? throw new System.ArgumentNullException(nameof(notifierMediatorService));
            _publishEndpoint = publishEndpoint ?? throw new System.ArgumentNullException(nameof(publishEndpoint));
        }

        [HttpGet]
        public async Task<string> Get()
        {
            await _publishEndpoint.Publish<Message>(new { Text = "gateway" });
            _notifierMediatorService.Notify("gateway");
            return "gateway";
        }
    }
}
