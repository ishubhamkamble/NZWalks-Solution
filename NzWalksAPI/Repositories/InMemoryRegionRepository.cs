using NzWalksAPI.Models.Domain;

namespace NzWalksAPI.Repositories
{
    public class InMemoryRegionRepository : IRegionRepository
    {
        public async Task<List<Region>> GetAllAsync()
        {
            return new List<Region>()
            {
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Code = "NRTL",
                    Name = "NorthLand Region"
                }
            };
        }
    }
}
