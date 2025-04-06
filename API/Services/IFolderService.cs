using API.DTO;

namespace API.Services
{
    public interface IFolderService
    {
        Task<IReadOnlyList<GetFolderDto>> GetAllFoldersAsync();
        Task<GetFolderDto> GetFolderByIdAsync(int id);
        Task<ReturnFolderDto> CreateFolderAsync(FolderCreateDto folderDto);
        Task DeleteFolderAsync(int id);
    }
}
