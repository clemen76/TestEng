using TestEng.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using TestEng.Entities.Repositories;
using TestEng.AppServices.Abstractions.User;

namespace TestEng.AppServices.Services
{
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

        public async Task<bool> AddUser(UserModel user)
        {
            user.IsActive = true;
            var isValid = ValidateUser(user);

            if (isValid)
            {
                try
                {
                    _userRepo.Insert(user);
                    await _userRepo.Save();
                    return true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return false;
        }

        public async Task<bool> EditUser(UserModel user)
        {
            var isValid = ValidateUser(user);

            if (isValid)
            {
                try
                {
                    _userRepo.Update(user);
                    await _userRepo.Save();
                    return true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return false;
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                _userRepo.Remove(id);
                await _userRepo.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
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
