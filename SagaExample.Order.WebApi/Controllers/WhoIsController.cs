using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaExample.Order.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhoIsController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "order";
        }
    }
}
