using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ConsulClient.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ConsulClientWrapper _client;

        public ValuesController()
        {
            _client = new ConsulClientWrapper();
        }

        [HttpGet]
        [Route("Get")]
        public async Task<string> Get()
        {
            return await _client.Get();



        }

        [HttpGet]
        [Route("Add")]
        public async Task Add()
        {
            await _client.Add();
        }


        [HttpGet]
        [Route("Services")]
        public async Task Services()
        {
            await _client.GetServices();
        }

        [HttpGet]
        [Route("Service/{serviceName}")]
        public async Task<string> Service(string serviceName)
        {
            var service = await _client.GetService(() => serviceName);

            var serviceAddress = service.First();
            var responseFromService = await Call.Service(() => serviceAddress);

            return responseFromService;
        }
    }
}
