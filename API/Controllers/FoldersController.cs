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
        public async Task<ActionResult<IEnumerable<Folder>>> GetFolders()
        {
            return await context.Folders.ToListAsync();
        }

        [HttpGet("{id:int}")] // api/products/2
        public async Task<ActionResult<Folder>> GetFolder(int id)
        {
            var folder = await context.Folders.FindAsync(id);

            if (folder == null) return NotFound();

            return folder;
        }

        [HttpPost]
        public async Task<ActionResult<Folder>> CreateFolder(Folder folder)
        {
            context.Folders.Add(folder);

            await context.SaveChangesAsync();

            return folder;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var folder = await context.Folders.FindAsync(id);

            if (folder == null) return NotFound();

            context.Remove(folder);

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
