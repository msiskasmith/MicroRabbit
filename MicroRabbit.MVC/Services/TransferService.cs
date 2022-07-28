using MicroRabbit.MVC.Models.DTO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MicroRabbit.MVC.Services
{
    public class TransferService : ITransferService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TransferService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task Transfer(TransferDto transferDto)
        {
            var client = _httpClientFactory.CreateClient("microconnection");

            var response = await client.PostAsJsonAsync($"Banking"
                , transferDto);
        }
    }
}
