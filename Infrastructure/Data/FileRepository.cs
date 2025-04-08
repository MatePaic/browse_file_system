using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class FileRepository(StoreContext _context) : IFileRepository
    {
        public async Task<IReadOnlyList<FileItem>> GetFilesAsync()
        {
            return await _context.Files
                .Include(f => f.Folder)
                .ToListAsync();
        }

        public async Task<FileItem> GetFileByIdAsync(int id)
        {
            return await _context.Files
                .Include(f => f.Folder)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<FileItem> CreateFileAsync(FileItem file)
        {
            _context.Files.Add(file);

            await _context.SaveChangesAsync();

            return file;
        }

        public async Task DeleteFileAsync(FileItem file)
        {
            _context.Files.Remove(file);
            
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<FileItem>> GetFilesByFolderAsync(int folderId)
        {
            return await _context.Files
                .Where(f => f.FolderId == folderId)
                .ToListAsync();
        }
    }
}