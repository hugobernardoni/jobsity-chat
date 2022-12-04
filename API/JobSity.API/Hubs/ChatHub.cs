using AutoMapper;
using JobSity.API.ViewModels;
using JobSity.Model.Helpers.InputModels;
using JobSity.Model.Models;
using JobSity.Repositories.Abstract;
using Microsoft.AspNetCore.SignalR;

namespace JobSity.API.Hubs
{
    public class ChatHub : BaseHub
    {
        private readonly IEntityBaseRepositoryAsync<Chat> _chatRepository;
        private readonly IMapper _mapper;
        private readonly ChatHubService<ChatHub> _hubMethods;

        public ChatHub(ChatHubService<ChatHub> hubMethods,
                      IEntityBaseRepositoryAsync<Chat> chatRepository,
                      IMapper mapper)
        {
            _mapper = mapper;           
            _chatRepository = chatRepository;            
            _hubMethods = hubMethods;
        }

        public async Task SendMessage(ChatInputModel chatInputModel)
        {
            var chat = _mapper.Map<Chat>(chatInputModel);
            chat.UserId = this.CurrentUser.UserId;
            chat.TimeStamp = DateTime.Now;

            var chatViewModel = _mapper.Map<ChatViewModel>(chat);
            chatViewModel.Username = this.CurrentUser.Username;
            await _hubMethods.DispatchMessageToClients(chatViewModel);

            try
            {

                await _chatRepository.AddAsync(chat);
                await _chatRepository.CommitAsync();

            }
            catch (ApplicationException ex)
            {
                chatViewModel.UserId = "";
                chatViewModel.Username = "Bot";
                chatViewModel.Message = ex.Message;
                await _hubMethods.DispatchMessageToClients(chatViewModel);
            }
            catch (Exception ex)
            {
                //log message
                var log = ex.ToString();
            }
        }
    }
}
