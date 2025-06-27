using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalksAPI.CustomActionFilters;
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
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        //GET all Regions
        //GET :https://localhost:port/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from Database - Domain Model
            var regionsDomain = await regionRepository.GetAllAsync();

            //Mapping using automapper
            return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
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

            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }


        //POST to create the New region
        //POST : https://localhost:port/api/regions
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //map or convert to domain model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            //Use Domian Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map domain Model back to DTO
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDomainModel);
        }


        //Update Region
        //PUT : https://localhost:port/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRequestRegionDto updateRequestRegionDto)
        {
            //Map DTO to domain model
            var regionDomainModel = mapper.Map<Region>(updateRequestRegionDto);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDTO>(regionDomainModel));
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

            return Ok(mapper.Map<RegionDTO>(regionDomainModel));
        }
    }
}
