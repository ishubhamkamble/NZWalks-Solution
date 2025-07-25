﻿using Microsoft.EntityFrameworkCore;
using NzWalksAPI.Models.Domain;

namespace NzWalksAPI.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {
            //Contructor
        }

        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed the data for difficulties : Easy, Medium, Hard
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("c29f3cc7-dd27-4e51-8249-edb7bef53552"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("1a852f67-f936-4571-b9f9-41822d2c3acc"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("7909f90b-946c-435d-9bfb-ef178265b69e"),
                    Name = "Hard"
                }
            };

            //Seed difficulties data to the databases
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            //Seed data for Regions
            var regions = new List<Region>()
            {
                new Region
                {
                    Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageURL = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageURL = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageURL = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageURL = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageURL = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageURL = null
                },
            };

            //Seed regions data to the databases
            modelBuilder.Entity<Region>().HasData(regions);


        }
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Commands for Seeding data OR migration of data from code to database
/// - Open Package manager console in Visual studio : 
/// PM> Add-Migration "Seeding data for Difficulties and Regions"
/// 
/// PM> Update-Database
/// 
/// 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
