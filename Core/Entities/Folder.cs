namespace Core.Entities
{
    public class Folder : BaseEntity
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int? ParentFolderId { get; set; }
        public Folder ParentFolder { get; set; }
        public ICollection<Folder> Subfolders { get; set; } = [];
        public ICollection<FileItem> Files { get; set; } = [];
    }
}
