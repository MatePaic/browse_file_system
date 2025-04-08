using System.Text.RegularExpressions;
using API.DTO;
using Core.Entities;
using Infrastructure.Data;

namespace API.Services
{
    public class FileService(IFileRepository _fileRepository) : IFileService
    {
        public async Task<IReadOnlyList<GetFileDto>> GetFilesAsync()
        {
            var files = await _fileRepository.GetFilesAsync();

            return files.Select(MapToGetFileDto).ToList();
        }

        public async Task<GetFileDto> GetFileByIdAsync(int id)
        {
            var file = await _fileRepository.GetFileByIdAsync(id);

            return file == null ? null : MapToGetFileDto(file);
        }

        public async Task<ReturnFileDto> CreateFileAsync(FileCreateDto fileDto)
        {
            var baseName = fileDto.Name.Trim();
            var uniqueName = await GenerateUniqueFileName(fileDto.FolderId, baseName);
            
            var fileItem = new FileItem
            {
                Name = uniqueName,
                FolderId = fileDto.FolderId,
                CreatedDate = DateTime.Now
            };

            var createdFile = await _fileRepository.CreateFileAsync(fileItem);
            return new ReturnFileDto
            {
                Id = createdFile.Id,
                Name = createdFile.Name,
                CreatedDate = createdFile.CreatedDate,
                FolderId = createdFile.FolderId
            };
        }
        public async Task<bool> DeleteFileAsync(int id)
        {
            var file = await _fileRepository.GetFileByIdAsync(id);
            
            if (file == null) return false;
            
            await _fileRepository.DeleteFileAsync(file);

            return true;
        }

        public async Task<IReadOnlyList<GetFileDto>> SearchStartsWithAsync(string startsWith, int? folderId)
        {
            var files = await _fileRepository.SearchStartsWithAsync(startsWith, folderId);
            
            return files.Select(MapToGetFileDto).ToList();
        }


        private static GetFileDto MapToGetFileDto(FileItem file)
        {
            return new GetFileDto
            {
                Id = file.Id,
                Name = file.Name,
                FolderId = file.FolderId,
                FolderName = file.Folder?.Name,
                CreatedDate = file.CreatedDate
            };
        }

        private async Task<string> GenerateUniqueFileName(int folderId, string baseName)
        {
            var existingFiles = await _fileRepository.GetFilesByFolderAsync(folderId);
            var existingNames = existingFiles.Select(f => f.Name).ToList();

            if (!existingNames.Any(n => n.StartsWith(baseName)))
            {
                return baseName;
            }

            var maxCounter = 0;
            var pattern = $@"^{baseName}(\d+)$";

            foreach (var name in existingNames)
            {
                var match = Regex.Match(name, pattern);
                if (match.Success)
                {
                    var currentCounter = int.Parse(match.Groups[1].Value);
                    if (currentCounter > maxCounter)
                    {
                        maxCounter = currentCounter;
                    }
                }
                else if (name == baseName)
                {
                    maxCounter = Math.Max(maxCounter, 1);
                }
            }

            return maxCounter == 0 ? baseName : $"{baseName}{maxCounter + 1}";
        }

        public async Task<IReadOnlyList<GetFileDto>> SearchExactNameAsync(string name, int? folderId)
        {
            var files = await _fileRepository.SearchExactNameAsync(name, folderId);
            
            return files.Select(MapToGetFileDto).ToList();
        }
    }
}