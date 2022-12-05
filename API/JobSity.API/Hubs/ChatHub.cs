using AutoMapper;
using JobSity.API.ViewModels;
using JobSity.Messaging.Sender;
using JobSity.Model.Helpers;
using JobSity.Model.Helpers.InputModels;
using JobSity.Model.Models;
using JobSity.Model.Models.Messaging;
using JobSity.Repositories.Abstract;
using Microsoft.AspNetCore.SignalR;

namespace JobSity.API.Hubs
{
    public class ChatHub : BaseHub
    {
        private readonly IEntityBaseRepositoryAsync<Chat> _chatRepository;
        private readonly IMapper _mapper;
        private readonly ChatHubService<ChatHub> _hubMethods;
        private readonly IStockRequestSender _stockRequestSender;

        public ChatHub(ChatHubService<ChatHub> hubMethods,
                      IEntityBaseRepositoryAsync<Chat> chatRepository,
                      IMapper mapper,
                      IStockRequestSender stockRequestSender)
        {
            _mapper = mapper;           
            _chatRepository = chatRepository;            
            _hubMethods = hubMethods;
            _stockRequestSender = stockRequestSender;
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

                if (ChatHelper.IsCommand(chat.Message))
                {
                    var command = ChatHelper.GetValidCommandMessage(chat.Message);
                    var stockRequestMessage = new StockRequestMessage
                    {
                        RoomId = chatInputModel.RoomId,
                        Code = command
                    };
                    _stockRequestSender.SendStockRequest(stockRequestMessage);
                }
                else
                {
                    await _chatRepository.AddAsync(chat);
                    await _chatRepository.CommitAsync();
                }

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
