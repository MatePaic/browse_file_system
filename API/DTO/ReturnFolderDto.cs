using Core.Entities;

namespace API.DTO
{
    public class ReturnFolderDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ParentFolderName { get; set; }
    }
}
