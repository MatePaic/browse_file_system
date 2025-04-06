using Core.Entities;

namespace API.DTO
{
    public class ReturnFolderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ParentFolderName { get; set; }
    }
}
