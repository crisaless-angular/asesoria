using Newtonsoft.Json;
using System.Threading.Tasks;
using RestSharp;
using Web.Models;
using Web.Business.Interfaces;

namespace Web.Business.Services
{
    public class Mail : IMail
    {
        public Mail()
        {

        }

        public async Task SendEmailAsync(JsonMail mailRequest)
        {
            string Json = JsonConvert.SerializeObject(mailRequest);
            RestClient client = new RestClient("https://wppruebas.crisaless.es/wp-json/Users/EnvioMail");
            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("postman-token", "3325f50e-3b1e-3262-7372-0a90019d8a6f");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", Json, ParameterType.RequestBody);
            await client.ExecuteAsync(request);
        }

    }
}
