namespace API.DTO
{
    public class GetFolderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ParentFolder { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<GetFolderDto> Subfolders { get; set; } = [];
        public List<GetFileWithFolderDto> Files { get; set; } = [];
    }
}
