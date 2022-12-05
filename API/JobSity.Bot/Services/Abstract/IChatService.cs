using JobSity.Model.Models.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSity.Bot.Services.Abstract
{
    public interface IChatService
    {
        Task<StockResponseMessage> GetStockDetails(string code);
    }
}
