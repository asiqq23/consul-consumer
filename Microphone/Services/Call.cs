using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services
{
    public static class Call
    {
        public static async Task<string> Service(Func<string> path)
        {
            using (var http = new HttpClient())
            {
                var result = await http.GetAsync(path());

                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}