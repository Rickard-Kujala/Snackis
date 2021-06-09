using Microsoft.Extensions.Configuration;
using Snackis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Snackis.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IConfiguration _configuration;
        
        public HttpClient Client { get; set; } = new HttpClient();
        public ChatRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<Chat>> GetAllChats()
        {
            return await Client.GetFromJsonAsync<List<Chat>>(_configuration["SnackisAPIChat"]);
        }
        public async Task<HttpResponseMessage> PostAsync(Chat chatModel)
        {
            var newChat = new Chat
            {
                Id = Guid.NewGuid(),
                ReceiverId = chatModel.ReceiverId,
                SenderId = chatModel.SenderId,
                Text=chatModel.Text,
                GroupMembers = chatModel.GroupMembers,
                GroupAdminId = chatModel.GroupAdminId
            };

            return await Client.PostAsJsonAsync(_configuration["SnackisAPIChat"], newChat);

        }
    }
}
