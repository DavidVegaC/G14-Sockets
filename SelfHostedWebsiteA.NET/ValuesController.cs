using IPC.HTTP.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace SelfHostedWebsiteA.NET
{
    public class ValuesController : ApiController
    {
        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new string[] { "Website A", "Response" };
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values 
        public async Task<SampleMessage> Post([FromBody]SampleMessage request)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{DateTime.UtcNow.ToString("ss.fff")} Procesando petición de {request.Message}");
            return await Task.FromResult(new SampleMessage { Message = $"Respuesta de Cliente B" });
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}
