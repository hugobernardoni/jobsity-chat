using JobSity.API.Services.Abstract;
using JobSity.Model.Helpers;
using System.Text.RegularExpressions;

namespace JobSity.API.Services.Implementations
{
    public class ChatService : IChatService
    {
        private Command _command;
        public ChatService(IConfiguration configuration)
        {
            _command = configuration.GetSection("Commands").Get<Command>();
        }

        public string GetValidCommandMessage(string message)
        {

            var regex = new Regex($"{_command.ValueCommand}").Matches(message);

            if (regex.Count > 0)
            {
                return regex.First().Value;
            }

            throw new ApplicationException($"Command '{message}' not allowed");
        }

        public bool IsCommand(string message)
        {
            var regex = new Regex($"{_command.ValidCommand}").Matches(message);

            if (regex.Count > 0)
                return true;

            regex = new Regex($"{_command.OthersCommand}").Matches(message);

            if (regex.Count > 0)
                throw new ApplicationException($"Command '{message}' not allowed");

            return false;
        }
    }
}
