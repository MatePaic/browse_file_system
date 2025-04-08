namespace Core.Entities
{
    public class Folder : BaseEntity
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ParentFolderId { get; set; }
        public Folder ParentFolder { get; set; }
        public List<Folder> Subfolders { get; set; } = [];
        public List<FileItem> Files { get; set; } = [];
    }
}
