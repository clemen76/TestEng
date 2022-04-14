using TestEng.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using TestEng.Entities.Repositories;

namespace TestEng.AppServices.Services
{
    public interface IUserService 
    {
        List<UserModel> GetUsers();
        Task<UserModel> GetUserById(int id);
        Task<bool> UserExists(int id);
        Task AddUser(UserModel model);
        Task EditUser(UserModel user);
        Task DeleteUser(int id);
        Task<List<UserModel>> GetActiveUsers();
    }
    public class UserService: IUserService
    {
        #region constructor

        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        #endregion

        #region actions

        public List<UserModel> GetUsers()
        {
            var users = _userRepo.GetAll();
            return users.ToList();
        }

        public async Task<UserModel> GetUserById(int id)
        {
            var user = await _userRepo.GetById(id);
            return user;
        }

        public async Task<bool> UserExists(int id)
        {
            return await _userRepo.Exists(id);
        }

        public async Task AddUser(UserModel user)
        {
            user.IsActive = true;
            var isValid = ValidateUser(user);

            if (isValid)
            {
                try
                {
                    _userRepo.Insert(user);
                    await _userRepo.Save();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public async Task EditUser(UserModel user)
        {
            var isValid = ValidateUser(user);

            if (isValid)
            {
                try
                {
                    _userRepo.Update(user);
                    await _userRepo.Save();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public async Task DeleteUser(int id)
        {
            try
            {
                _userRepo.Remove(id);
                await _userRepo.Save();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<UserModel>> GetActiveUsers()
        {
            var users = await _userRepo.GetActiveUsers();
            return users;
        }

        #endregion

        #region private methods

        private bool ValidateUser(UserModel user)
        {
            if (String.IsNullOrWhiteSpace(user.Name) || user.BirthDate == DateTime.MinValue)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
