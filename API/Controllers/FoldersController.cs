using API.DTO;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoldersController(StoreContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<GetFolderDto>>> GetFolders()
        {
            var allFolders = await context.Folders
                .Include(f => f.ParentFolder)
                .ToListAsync();

            var rootFolders = allFolders.Where(f => f.ParentFolderId == null).ToList();

            var result = rootFolders
                .Select(folder => MapToDto(folder, allFolders))
                .ToList();

            return Ok(result);
        }

        private GetFolderDto MapToDto(Folder folder, List<Folder> allFolders)
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

        [HttpGet("{id:int}")] // api/products/2
        public async Task<ActionResult<GetFolderDto>> GetFolder(int id)
        {
            var allFolders = await context.Folders
                .Include(f => f.ParentFolder)
                .ToListAsync();

            var folder = allFolders.FirstOrDefault(f => f.Id == id); 

            if (folder == null)
                return NotFound();

            var getFolderDto = new GetFolderDto
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

            return Ok(getFolderDto);
        }

        [HttpPost]
        public async Task<ActionResult<ReturnFolderDto>> CreateFolder(FolderCreateDto folderDto)
        {
            var folder = new Folder { Name = folderDto.Name, ParentFolderId = folderDto.ParentFolderId };

            var returnFolderDto = new ReturnFolderDto();

            var foldersDB = context.Folders;

            if (folder.ParentFolderId.HasValue)
            {
                var parent = await foldersDB.FindAsync(folder.ParentFolderId);

                if (parent == null) return BadRequest();

                folder.Path = $"{parent.Path}{folder.Name}/";

                if (folder.ParentFolder == null) folder.ParentFolder = parent;

                returnFolderDto.ParentFolderName = folder.ParentFolder.Name;
            }
            else
            {
                var parent2 = foldersDB
                    .Where(e => e.Name.Contains(folder.Name))
                    .ToList();

                if (parent2.Count() > 0)
                {
                    folder.Name = $"{folder.Name}{parent2.Count()}";
                }

                folder.Path = $"/{folder.Name}/";
            }

            returnFolderDto.Name = folder.Name;
            returnFolderDto.Path = folder.Path;

            context.Folders.Add(folder);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFolder), new { id = folder.Id }, returnFolderDto);
        }

        [HttpDelete("{id:int}")] // DELETE api/folders/2
        public async Task<ActionResult> DeleteFolder(int id)
        {
            // Load all folders with their parent folder relationship
            var allFolders = await context.Folders
                .Include(f => f.ParentFolder)
                .ToListAsync();

            // Find the folder by ID
            var folderToDelete = allFolders.FirstOrDefault(f => f.Id == id);

            if (folderToDelete == null)
                return NotFound($"Folder with ID {id} not found.");

            // Delete subfolders recursively
            await DeleteSubfolders(folderToDelete, allFolders);

            // Remove the folder itself
            context.Folders.Remove(folderToDelete);
            await context.SaveChangesAsync();

            return NoContent(); // Return status 204 - No Content
        }

        private async Task DeleteSubfolders(Folder folder, List<Folder> allFolders)
        {
            // Find all subfolders of the current folder
            var subfolders = allFolders.Where(f => f.ParentFolderId == folder.Id).ToList();

            foreach (var subfolder in subfolders)
            {
                // Recursively delete subfolders
                await DeleteSubfolders(subfolder, allFolders);

                // Remove subfolder
                context.Folders.Remove(subfolder);
            }
        }
    }
}
