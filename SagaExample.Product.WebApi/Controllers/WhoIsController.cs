using Microsoft.AspNetCore.Mvc;

namespace SagaExample.Product.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhoIsController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "product";
        }
    }
}
