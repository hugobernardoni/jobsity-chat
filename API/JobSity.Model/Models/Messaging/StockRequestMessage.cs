using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSity.Model.Models.Messaging
{
    public class StockRequestMessage
    {
        public string RoomId { get; set; }
        public string Code { get; set; }
    }
}
