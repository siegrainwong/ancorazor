using Ancorazor.Entity;
using System.Threading.Tasks;

namespace Ancorazor.Service
{
    public class ImageStorageService
    {
        private readonly BlogContext _context;

        public ImageStorageService(BlogContext context)
        {
            _context = context;
        }

        public async Task<int> InsertAsync(ImageStorage entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
