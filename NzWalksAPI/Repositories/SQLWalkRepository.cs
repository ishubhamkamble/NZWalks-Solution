using Microsoft.EntityFrameworkCore;
using NzWalksAPI.Data;
using NzWalksAPI.Models.Domain;

namespace NzWalksAPI.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAysnc()
        {
            return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();

            //OR
            //Another way for Navigation properties
            //return await dbContext.Walks.Include(x=>x.DifficultyName).Include(x=>x.RegionName).ToListAsync();
        }
    }
}
