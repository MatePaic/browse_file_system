using Core.Entities;

namespace Core.Interfaces
{
    public interface IFolderRepository
    {
        Task<Folder> GetByIdAsync(int id);
        Task<List<Folder>> GetFoldersAsync();
        Task<Folder> AddAsync(Folder folder);
        Task DeleteAsync(Folder folder);
    }
}
