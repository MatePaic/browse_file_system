namespace API.DTO
{
    public class GetFileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // Relationship
        public int FolderId { get; set; }
        public string FolderName { get; set; }
    }
}
