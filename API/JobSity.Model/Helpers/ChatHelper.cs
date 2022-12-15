using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JobSity.Model.Helpers
{
    public class ChatHelper
    {
        public static bool IsCommand(string message)
        {
            var regex = new Regex($"^/stock=").Matches(message);

            if (regex.Count > 0)
                return true;

            regex = new Regex($"(?<=/)(.*?)(?==)").Matches(message);

            if (regex.Count > 0)
                throw new ApplicationException($"Command '{message}' not allowed");

            return false;
        }

        public static string GetValidCommandMessage(string message)
        {

            var regex = new Regex($"(?<=/stock=).+").Matches(message);

            if (regex.Count > 0)
            {
                return regex.First().Value;
            }

            throw new ApplicationException($"Command '{message}' not allowed");
        }        
    }
}
