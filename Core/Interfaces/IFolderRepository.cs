using Core.Entities;

namespace Core.Interfaces
{
    public interface IFolderRepository
    {
        Task<Folder> GetByIdAsync(int id);
        Task<IReadOnlyList<Folder>> GetFoldersAsync();
        Task<Folder> AddAsync(Folder folder);
        Task DeleteAsync(Folder folder);
    }
}
