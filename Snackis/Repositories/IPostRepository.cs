using Snackis.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Snackis.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPosts();
        Task<List<string>> GetCategories(string forum);
        Task <Post> GetPostsById(Guid Id);
        Task<HttpResponseMessage> UpdatePost(Guid id, Post updatedPost);
        Task<bool> OnPostReactAsync(Guid id, string value);
        Task<HttpResponseMessage> AddPostAsync(Post postModel);
        Task<HttpResponseMessage> DeletePostById(Guid id);
        Task<List<string>> GetForums();
    }
}