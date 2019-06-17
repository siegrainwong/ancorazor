using Ancorazor.Entity;
using Microsoft.EntityFrameworkCore;
using Siegrain.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ancorazor.Service
{
    public class UserService
    {
        private readonly BlogContext _context;

        public UserService(BlogContext context)
        {
            _context = context;
        }

        public async Task<Users> GetByLoginNameAsync(string loginName)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.LoginName == loginName);
        }

        public async Task<Users> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> PasswordRehashAsync(Users entity, string password, bool isNewCreadential = false)
        {
            entity.Password = SecurePasswordHasher.Hash(password);
            if (isNewCreadential) entity.AuthUpdatedAt = DateTime.Now;
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
