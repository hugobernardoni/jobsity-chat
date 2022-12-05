using JobSity.Model.Models.Messaging;


namespace JobSity.Bot.Messaging.Sender
{
    public interface IStockResponseSender
    {
        void SendStockResponse(StockResponseMessage stock);
    }
}
