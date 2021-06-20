using Snackis.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Snackis.Repositories
{
    public interface IChatRepository
    {
        HttpClient Client { get; set; }

        Task<List<Chat>> GetAllChats();
        Task<HttpResponseMessage> PostAsync(Chat chatModel);
        Task<HttpResponseMessage> UpdateChatAsync(Guid id, Chat updatedPost);
    }
}