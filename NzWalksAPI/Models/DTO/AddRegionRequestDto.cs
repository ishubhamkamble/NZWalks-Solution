namespace NzWalksAPI.Models.DTO
{
    public class AddRegionRequestDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string? RegionImageURL { get; set; }
    }
}
