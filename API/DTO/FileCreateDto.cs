using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class FileCreateDto
    {
        [Required]
        public int FolderId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
