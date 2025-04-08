using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class FolderRepository(StoreContext _context) : IFolderRepository
    {
        public async Task<Folder> GetByIdAsync(int id)
        {
            return await _context.Folders
                .Include(f => f.ParentFolder)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<Folder>> GetFoldersAsync()
        {
            return await _context.Folders.Include(x => x.Files).ToListAsync();
        }

        public async Task<Folder> AddAsync(Folder folder)
        {
            _context.Folders.Add(folder);

            await _context.SaveChangesAsync();

            return folder;
        }

        public async Task DeleteAsync(Folder folder)
        {
            _context.Folders.Remove(folder);

            await _context.SaveChangesAsync();
        }
    }
}
