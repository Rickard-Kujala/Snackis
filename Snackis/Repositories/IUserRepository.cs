using Snackis.Models;
using System.Threading.Tasks;

namespace Snackis.Repositories
{
    public interface IUserRepository
    {
        Task Login(InputModel Input);
        Models.InputModel GetModel();
        bool GetUserReactions(Post p, string userId, string reaction);
    }
}