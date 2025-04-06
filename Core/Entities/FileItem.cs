 namespace Core.Entities
{
    public class FileItem : BaseEntity
    {
        public string Name { get; set; }
        public int FolderId { get; set; }
        public Folder Folder { get; set; }
    }
}
