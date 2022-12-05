using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSity.Model.Models.Messaging
{
    public class StockResponseMessage
    {
        public StockResponseMessage()
        {
            Stock = new Stock();
        }

        public bool Success { get; set; }
        public string RoomId { get; set; }
        public Stock Stock { get; set; }
    }
}
