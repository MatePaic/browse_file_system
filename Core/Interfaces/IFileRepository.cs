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
        Task<IReadOnlyList<FileItem>> SearchStartsWithAsync(string startsWith, int? folderId);
        Task<IReadOnlyList<FileItem>> SearchExactNameAsync(string name, int? folderId);
    }
}