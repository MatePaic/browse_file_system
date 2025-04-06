using API.DTO;
using Core.Entities;
using Core.Interfaces;

namespace API.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;

        public FolderService(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public async Task<ReturnFolderDto> CreateFolderAsync(FolderCreateDto folderDto)
        {
            var folder = new Folder { Name = folderDto.Name, ParentFolderId = folderDto.ParentFolderId };
            var returnFolderDto = new ReturnFolderDto();
            var allFolders = await _folderRepository.GetFoldersAsync();

            if (folder.ParentFolderId.HasValue)
            {
                var parentFolder = await _folderRepository.GetByIdAsync(folder.ParentFolderId.Value);
                if (parentFolder == null) return null;

                // Get all sibling folders (folders with same parent)
                var siblingFolders = allFolders.Where(f => f.ParentFolderId == parentFolder.Id).ToList();

                // Find the next available number for duplicate names
                folder.Name = GetUniqueFolderName(folder.Name, siblingFolders);
                folder.Path = $"{parentFolder.Path}{folder.Name}/";
                folder.ParentFolder = parentFolder;
                returnFolderDto.ParentFolderName = parentFolder.Name;
            }
            else
            {
                // Get all root folders (folders with no parent)
                var rootFolders = allFolders.Where(f => !f.ParentFolderId.HasValue).ToList();

                // Find the next available number for duplicate names
                folder.Name = GetUniqueFolderName(folder.Name, rootFolders);
                folder.Path = $"/{folder.Name}/";
            }

            returnFolderDto.Id = folder.Id;
            returnFolderDto.Name = folder.Name;
            returnFolderDto.Path = folder.Path;

            await _folderRepository.AddAsync(folder);
            return returnFolderDto;
        }

        public async Task DeleteFolderAsync(int id)
        {
            var allFolders = await _folderRepository.GetFoldersAsync();
            var folderToDelete = allFolders.FirstOrDefault(f => f.Id == id);

            if (folderToDelete == null) return;

            await DeleteSubfoldersRecursive(folderToDelete, allFolders);
            await _folderRepository.DeleteAsync(folderToDelete);
        }

        public async Task<IReadOnlyList<GetFolderDto>> GetAllFoldersAsync()
        {
            var allFolders = await _folderRepository.GetFoldersAsync();
            var rootFolders = allFolders.Where(f => f.ParentFolderId == null).ToList();

            return rootFolders.Select(folder => MapToDto(folder, allFolders)).ToList();
        }

        public async Task<GetFolderDto> GetFolderByIdAsync(int id)
        {
            var allFolders = await _folderRepository.GetFoldersAsync();
            var folder = allFolders.FirstOrDefault(f => f.Id == id);

            if (folder == null) return null;

            return MapToDto(folder, allFolders);
        }

        private async Task DeleteSubfoldersRecursive(Folder folder, IReadOnlyList<Folder> allFolders)
        {
            var subfolders = allFolders.Where(f => f.ParentFolderId == folder.Id).ToList();

            foreach (var subfolder in subfolders)
            {
                await DeleteSubfoldersRecursive(subfolder, allFolders);
                await _folderRepository.DeleteAsync(subfolder);
            }
        }

        private GetFolderDto MapToDto(Folder folder, IEnumerable<Folder> allFolders)
        {
            return new GetFolderDto
            {
                Id = folder.Id,
                Name = folder.Name,
                Path = folder.Path,
                ParentFolder = folder.ParentFolder?.Name,
                Subfolders = allFolders
                    .Where(f => f.ParentFolderId == folder.Id)
                    .Select(f => MapToDto(f, allFolders))
                    .ToList()
            };
        }

        private string GetUniqueFolderName(string baseName, List<Folder> existingFolders)
        {
            if (!existingFolders.Any(f => f.Name.Equals(baseName, StringComparison.OrdinalIgnoreCase)))
            {
                return baseName;
            }

            int counter = 1;
            string newName;

            do
            {
                newName = $"{baseName}{counter}";
                counter++;
            }
            while (existingFolders.Any(f => f.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)));

            return newName;
        }
    }
}
