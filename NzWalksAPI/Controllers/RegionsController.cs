using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalksAPI.CustomActionFilters;
using NzWalksAPI.Data;
using NzWalksAPI.Models.Domain;
using NzWalksAPI.Models.DTO;
using NzWalksAPI.Repositories;
using System.Text.Json;

namespace NzWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //GET all Regions
        //GET :https://localhost:port/api/regions
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAll(): regions action method invoked");
            //Get Data from Database - Domain Model
            var regionsDomain = await regionRepository.GetAllAsync();

            logger.LogInformation($"Finished GetAll(): regions request for data: {JsonSerializer.Serialize(regionsDomain)}");
            //Mapping using automapper
            return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
        }


        //Get region by Id
        //GET : https://localhost:port/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            logger.LogInformation("GetById(): regions action method invoked");
            //using id directly
            //var region = dbContext.Regions.Find(id);

            //Get Region Domain model From Database
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            logger.LogInformation($"Finished GetById(): region request for data by Id: {JsonSerializer.Serialize(regionDomain)}");
            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }


        //POST to create the New region
        //POST : https://localhost:port/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            logger.LogInformation("Create(): regions action method invoked");
            //map or convert to domain model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            //Use Domian Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map domain Model back to DTO
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
            logger.LogInformation($"GetAll(): region added successfully:  {JsonSerializer.Serialize(regionDomainModel)}");
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDomainModel);
        }


        //Update Region
        //PUT : https://localhost:port/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRequestRegionDto updateRequestRegionDto)
        {
            logger.LogInformation("Update(): region action method invoked");
            //Map DTO to domain model
            var regionDomainModel = mapper.Map<Region>(updateRequestRegionDto);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            logger.LogInformation($"Update(): region updated successfully :  { JsonSerializer.Serialize(regionDomainModel)}");
            return Ok(mapper.Map<RegionDTO>(regionDomainModel));
        }


        //Delete Region
        //Delete : https://localhost:port/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            logger.LogInformation("Delete(): regions action method invoked");
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            logger.LogInformation("GetAll(): region deleted successfully");
            return Ok(mapper.Map<RegionDTO>(regionDomainModel));
        }
    }
}
