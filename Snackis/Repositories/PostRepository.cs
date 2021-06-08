using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snackis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Snackis.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IConfiguration _configuration;

        public HttpClient Client { get; set; } = new HttpClient();
        public PostRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Post>> GetPosts()
        {
            return await Client.GetFromJsonAsync<List<Post>>(_configuration["SnackisAPIPost"]);
        }

        public async Task<Post> GetPostsById(Guid Id)
        {
            return await Client.GetFromJsonAsync<Post>(_configuration["SnackisAPIPost"] + "/" + $"{Id.ToString()}");
        }
        public async Task<List<string>> GetCategories()
        {
            var allPosts = await GetPosts(); 
            return allPosts.Select(p => p.Category).Distinct().ToList();
        }
        public async Task<HttpResponseMessage> UpdatePost(Guid id, Post updatedPost)
        {
            var post = JsonSerializer.Serialize(updatedPost);
            var requestContent = new StringContent(post, Encoding.UTF8, "application/json");

            return await Client.PutAsync(_configuration["SnackisAPIPost"] + "/" + $"{id}", requestContent);
        }
        public async Task<bool> OnPostReactAsync(Guid id, string value)
        {
            var updated = await GetPostsById(id);
            switch (value)
            {
                case "Likes":
                    updated.Likes += 1;
                    break;
                case "Dislikes":
                    updated.DisLikes += 1;
                    break;
                case "Abuse":
                    updated.AbuseReport = true;
                    break;
            }
            var result = await UpdatePost(id, updated);
            if (!result.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
        public async Task<HttpResponseMessage> AddPostAsync(Post postModel)
        {
            var client = new HttpClient();
            {
                var post = new Post
                {
                    Id = Guid.NewGuid(),
                    Category = postModel.Category,
                    Title = postModel.Title,
                    Text = postModel.Text,
                    Heading=postModel.Heading,
                    DateTime = DateTime.Now,
                    AbuseReport = false,
                    PostParent = postModel.PostParent,
                    UserId = postModel.UserId,
                    Nickname = postModel.Nickname,
                    ImageURL=postModel.ImageURL

                };
                return await client.PostAsJsonAsync(_configuration["SnackisAPIPost"], post);
            }
        }
    }
}
