using System.ComponentModel.DataAnnotations.Schema;

namespace NzWalksAPI.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeinBytes  { get; set; }
        public string? FilePath  { get; set; }
    }
}
