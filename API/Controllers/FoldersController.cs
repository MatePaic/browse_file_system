using API.DTO;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoldersController : ControllerBase
    {
        private readonly IFolderService _folderService;

        public FoldersController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetFolderDto>>> GetFolders()
        {
            var folders = await _folderService.GetAllFoldersAsync();

            return Ok(folders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetFolderDto>> GetFolder(int id)
        {
            var folder = await _folderService.GetFolderByIdAsync(id);

            if (folder == null)
                return NotFound();

            return Ok(folder);
        }

        [HttpPost]
        public async Task<ActionResult<ReturnFolderDto>> CreateFolder(FolderCreateDto folderDto)
        {
            var result = await _folderService.CreateFolderAsync(folderDto);

            if (result == null)
                return BadRequest("Parent folder not found");

            return CreatedAtAction(nameof(GetFolder), new { id = result.Id }, result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteFolder(int id)
        {
            var result = await _folderService.DeleteFolderAsync(id);
            
            return result ? NoContent() : NotFound();
        }
    }
}
