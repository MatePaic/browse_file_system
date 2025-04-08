using API.DTO;

namespace API.Services
{
    public interface IFileService
    {
        Task<IReadOnlyList<GetFileDto>> GetFilesAsync();
        Task<GetFileDto> GetFileByIdAsync(int id);
        Task<ReturnFileDto> CreateFileAsync(FileCreateDto fileDto);
        Task<bool> DeleteFileAsync(int id);
        Task<IReadOnlyList<GetFileDto>> SearchStartsWithAsync(string startsWith, int? folderId);
    }
}