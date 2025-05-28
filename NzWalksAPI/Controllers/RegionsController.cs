using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalksAPI.Data;
using NzWalksAPI.Models.Domain;
using NzWalksAPI.Models.DTO;
using NzWalksAPI.Repositories;

namespace NzWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }

        //GET all Regions
        //GET :https://localhost:port/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from Database - Domain Model
            var regionsDomain = await regionRepository.GetAllAsync();

            //Map DTO to Domain Model
            var regionDto = new List<RegionDTO>();
            foreach (var regionDomain in regionsDomain) 
            {
                regionDto.Add(new RegionDTO()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageURL = regionDomain.RegionImageURL

                });
            }
            return Ok(regionDto);
        }


        //Get region by Id
        //GET : https://localhost:port/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //using id directly
            //var region = dbContext.Regions.Find(id);

            //Get Region Domain model From Database
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDTO()
            { 
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code, 
                RegionImageURL= regionDomain.RegionImageURL
            };
            return Ok(regionDto);
        }


        //POST to create the New region
        //POST : https://localhost:port/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //map or convert to domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageURL = addRegionRequestDto.RegionImageURL
            };

            //Use Domian Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map domain Model back to DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageURL = regionDomainModel.RegionImageURL
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id}, regionDomainModel);
        }


        //Update Region
        //PUT : https://localhost:port/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRequestRegionDto updateRequestRegionDto)
        {
            //Map DTO to domain model
            var regionDomainModel = new Region
            {
                Code = updateRequestRegionDto.Code,
                Name = updateRequestRegionDto.Name,
                RegionImageURL = updateRequestRegionDto.RegionImageURL
            };

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert DomainModel to DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageURL = regionDomainModel.RegionImageURL
            };

            return Ok(regionDto);
        }


        //Delete Region
        //Delete : https://localhost:port/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //return deleted region back
            //map Domain model to DTO
            var regionDto = new RegionDTO
            {

                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageURL = regionDomainModel.RegionImageURL
            };

            return Ok(regionDto);
        }
    }
}
