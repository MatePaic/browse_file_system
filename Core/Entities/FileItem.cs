 namespace Core.Entities
{
    public class FileItem : BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // Navigation Property
        public int FolderId { get; set; }
        public Folder Folder { get; set; }
    }
}
