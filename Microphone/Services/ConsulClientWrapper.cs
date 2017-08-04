using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;

namespace Services
{
    public class ConsulClientWrapper
    {
        public const string BaseUrl = "http://192.168.44.117:8500";
        public const string BaseUrl2 = "http://192.168.44.117";

        public async Task<string> Get()
        {
            using (var client = GetClient())
            {
                var key = await client.KV.Get("my");

                return Encoding.UTF8.GetString(key.Response.Value);
            }
        }

        public async Task Add()
        {
            using (var client = GetClient())
            {
                var putPair = new KVPair("hello")
                {
                    Value = Encoding.UTF8.GetBytes("Hello Consul")
                };

                var key = await client.KV.Put(putPair);
            }
        }
        
        public ConsulClient GetClient()
        {
            return new ConsulClient(configuration =>
            {
                configuration.Address = new Uri("http://192.168.44.117:8500");
            });
        }

        public async Task<IEnumerable<string>> GetServices()
        {
            using (var client = GetClient())
            {
                var allServices = await client.Catalog.Services();

                var servicesPairs = allServices.Response.Where(p => !p.Key.Contains("consul"));

                var services = servicesPairs.Select(p => p.Key);

                return services;
            }
        }

        public async Task<IEnumerable<string>> GetService(Func<string> getService)
        {
            var uris = new List<string>();

            using (var client = GetClient())
            {
                var services = await client.Catalog.Service(getService());

                var routesEncoded = await client.KV.Get(getService());

                var route = Encoding.UTF8.GetString(routesEncoded.Response.Value);

                uris.AddRange(services.Response.Select(service => $"{BaseUrl2}:{service.ServicePort}{route}"));

                return uris;
            }
        }
    }
}
