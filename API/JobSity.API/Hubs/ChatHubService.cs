using JobSity.API.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace JobSity.API.Hubs
{
    public class ChatHubService<THub> where THub : BaseHub
    {
        private readonly IHubContext<THub> _hubContext;

        public ChatHubService(IHubContext<THub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task DispatchMessageToClients(ChatViewModel chatViewModel)
        {
            await _hubContext.Clients.All.SendAsync(HubConstants.CHAT_MESSAGE, chatViewModel);
        }
    }
}
