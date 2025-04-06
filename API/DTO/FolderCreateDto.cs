using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class FolderCreateDto
    {
        [Required]
        public string Name { get; set; }
        public int? ParentFolderId { get; set; }
    }
}
