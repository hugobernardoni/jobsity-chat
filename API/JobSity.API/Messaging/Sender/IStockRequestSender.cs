using JobSity.Model.Models.Messaging;

namespace JobSity.Messaging.Sender
{
    public interface IStockRequestSender
    {
        void SendStockRequest(StockRequestMessage stockRequestMessage);
    }
}