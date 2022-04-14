using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestEng.Data.Models;

namespace TestEng.Entities.Repositories
{
    public interface IUserRepository : IDisposable
    {
        List<UserModel> GetAll();
        Task<UserModel> GetById(int id);
        Task<bool> Exists(int id);
        void Insert(UserModel user);
        void Update(UserModel user);
        Task Save();
        void Remove(int userId);
        Task<List<UserModel>> GetActiveUsers();
    }

    public class UserRepository : IUserRepository, IDisposable
    {
        #region constructor

        private readonly TestEngContext _ctx;
        public UserRepository(TestEngContext ctx)
        {
            _ctx = ctx;
        }

        #endregion

        #region Actions methods
        public List<UserModel> GetAll()
        {
            return _ctx.User.ToList();
        }

        public async Task<UserModel> GetById(int id)
        {
            var user = await _ctx.User.FirstOrDefaultAsync(x => x.UserId == id);
            return user;
        }

        public async Task<bool> Exists(int id)
        {
            var exists = await _ctx.User.AnyAsync(x => x.UserId == id);
            return exists;
        }

        public void Insert(UserModel user)
        {
            _ctx.User.Add(user);
        }

        public void Remove(int userID)
        {
            UserModel user = _ctx.User.Find(userID);
            _ctx.User.Remove(user);
        }

        public void Update(UserModel user)
        {
            _ctx.User.Update(user);
        }

        public async Task Save()
        {
            await _ctx.SaveChangesAsync();
        }

        public async Task<List<UserModel>> GetActiveUsers()
        {
            var users = await _ctx.User.Where(x => x.IsActive).ToListAsync();
            return users;
        }

        #endregion


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
