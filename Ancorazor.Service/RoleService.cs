using Ancorazor.Entity;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Siegrain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Ancorazor.Service
{
    public class RoleService
    {
        private readonly BlogContext _context;

        public RoleService(BlogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetByUserAsync(int id)
        {
            return await _context.UserRole.Where(x => x.UserId == id).Select(x => x.Role).ToListAsync();
        }
    }
}
