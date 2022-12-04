using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSity.Model.Helpers
{
    public class ChatHelper
    {
        public static readonly List<string> COMMANDS = new List<string>() { "stock" };

        public static bool IsCommand(string message)
        {
            if (!message.StartsWith("/"))
                return false;

            if (message.Split(' ').Count() > 1 || message.Split('=').Count() != 2)
                throw new ApplicationException("Unrecognized bot command. Please use \"/{command}={value}\"");

            return true;
        }

        public static string GetValidCommandMessage(string message)
        {
            var command = GetCommandFromMessage(message);

            if (COMMANDS.Contains(command))
            {
                var commandValue = message.Split('=')[1];
                return commandValue;
            }

            throw new ApplicationException($"Command '{command}' not allowed");
        }

        private static string GetCommandFromMessage(string message)
        {
            var command = message.Split('=').First();
            command = message.Substring(1);

            return command;
        }
    }
}
