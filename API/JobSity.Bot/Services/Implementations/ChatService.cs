using CsvHelper;
using JobSity.Bot.Services.Abstract;
using JobSity.Model.Models.Messaging;
using System.Globalization;

namespace JobSity.Bot.Services.Implementations
{
    public class ChatService : IChatService
    {
        public async Task<StockResponseMessage> GetStockDetails(string code)
        {
            var stockResponseMessage = new StockResponseMessage();
            using var client = new HttpClient();
            var stream = new StreamReader(await client.GetStreamAsync($"https://stooq.com/q/l/?s={code}&f=sd2t2ohlcv&h&e=csv"));
            var csvReader = new CsvReader(stream, CultureInfo.InvariantCulture);
            var stockRecords = csvReader.GetRecords<Stock>();
            stockResponseMessage.Stock = stockRecords.First();
            stockResponseMessage.Success = true;

            return await Task.FromResult<StockResponseMessage>(stockResponseMessage);
        }
    }
}
