using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using WebApplication1.Models;
using WebApplication1.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/addintegration")]
    [ApiController]
    public class IntegrationController : ControllerBase
    {
        private IIntegrationService IntegrationService { get; set; }

        public IntegrationController(IIntegrationService integrationService)
        {
            this.IntegrationService = integrationService;
        }

        // GET: api/<IntegrationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<IntegrationController>/5
        [HttpGet("addSpotifyAccount/{type}")]
        public string Get(IntegrationType type)
        {
            var redirectUrl = this.IntegrationService.AddNewAccountToIntegration(type);
            return JsonSerializer.Serialize(redirectUrl);
        }

        [HttpGet("callback")]
        public void CallBack([FromQuery] string code)
        {
            this.IntegrationService.HandleCallBack(code);
        }

        // POST api/<IntegrationController>
        [HttpPost]
        public void Post(IntegrationType type)
        {
           
        }

        // PUT api/<IntegrationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<IntegrationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
