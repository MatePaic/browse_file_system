using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class FolderRepository(StoreContext context) : IFolderRepository
    {
        public async Task<Folder> GetByIdAsync(int id)
        {
            return await context.Folders
                .Include(f => f.ParentFolder)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IReadOnlyList<Folder>> GetFoldersAsync()
        {
            return await context.Folders.ToListAsync();
        }

        public async Task<Folder> AddAsync(Folder folder)
        {
            context.Folders.Add(folder);

            await context.SaveChangesAsync();

            return folder;
        }

        public async Task DeleteAsync(Folder folder)
        {
            context.Folders.Remove(folder);

            await context.SaveChangesAsync();
        }
    }
}
