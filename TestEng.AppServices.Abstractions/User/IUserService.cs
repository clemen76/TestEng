using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestEng.Data.Models;

namespace TestEng.AppServices.Abstractions.User
{
    public interface IUserService
    {
        List<UserModel> GetUsers();
        Task<UserModel> GetUserById(int id);
        Task<bool> UserExists(int id);
        Task<bool> AddUser(UserModel model);
        Task<bool> EditUser(UserModel user);
        Task<bool> DeleteUser(int id);
        Task<List<UserModel>> GetActiveUsers();
    }
}
