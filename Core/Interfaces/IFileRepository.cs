using Core.Entities;

namespace Infrastructure.Data
{
    public interface IFileRepository
    {
        Task<IReadOnlyList<FileItem>> GetFilesAsync();
        Task<FileItem> GetFileByIdAsync(int id);
        Task<FileItem> CreateFileAsync(FileItem file);
        Task DeleteFileAsync(FileItem file);
        Task<IReadOnlyList<FileItem>> GetFilesByFolderAsync(int folderId);
    }
}