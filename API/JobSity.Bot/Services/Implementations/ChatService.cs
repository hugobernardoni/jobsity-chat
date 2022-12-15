using CsvHelper;
using JobSity.Bot.Services.Abstract;
using JobSity.Model.Helpers;
using JobSity.Model.Models;
using JobSity.Model.Models.Messaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Globalization;
using System.Security.Policy;

namespace JobSity.Bot.Services.Implementations
{
    public class ChatService : IChatService
    {
        private StockSettings _stockSettings;
        private HttpClient _client;

        public ChatService(HttpClient client,
            IConfiguration configuration)
        {

            _stockSettings = configuration.GetSection("StockSettings").Get<StockSettings>();            

            _client = client;
        }

        public async Task<StockResponseMessage> GetStockDetails(string code)
        {
            var stockResponseMessage = new StockResponseMessage(); 
            
            var queryParam = string.Format(_stockSettings.Params, code);

            var stream = new StreamReader(await _client.GetStreamAsync($"{_stockSettings.Url}{queryParam}"));
            var csvReader = new CsvReader(stream, CultureInfo.InvariantCulture);
            var stockRecords = csvReader.GetRecords<Stock>();
            stockResponseMessage.Stock = stockRecords.First();
            stockResponseMessage.Success = true;

            return await Task.FromResult<StockResponseMessage>(stockResponseMessage);
        }
    }
}
