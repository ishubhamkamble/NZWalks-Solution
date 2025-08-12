using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NzWalksAPI.CustomActionFilters;
using NzWalksAPI.Models.Domain;
using NzWalksAPI.Models.DTO;
using NzWalksAPI.Repositories;
using System.Text.Json;

namespace NzWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        private readonly ILogger<WalksController> logger;

        public WalksController(IMapper mapper, IWalkRepository walkRepository, ILogger<WalksController> logger)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
            this.logger = logger;
        }



        //Create Walk
        //POST() : /api/walks
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            logger.LogInformation("Create(): action method invoked");
            //Map DTO to domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
            await walkRepository.CreateAsync(walkDomainModel);
            logger.LogInformation($"Create(): region updated successfully :  {JsonSerializer.Serialize(walkDomainModel)}");
            //map Domain model to DTO
            return Ok(mapper.Map<WalksDto>(walkDomainModel));
        }

        //GET All Walks
        //GET : /api/Walks?filterOn=name&filterQuery=Garden&sortBy=Name&IsAscending=true
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery]string? sortBy, [FromQuery]bool? isAscending,
            [FromQuery]int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            logger.LogInformation("GetAll(): action method invoked");
            var walkDomainModel = await walkRepository.GetAllAysnc(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            logger.LogInformation($"GetById(): All walks fetched successfully :  {JsonSerializer.Serialize(walkDomainModel)}");
            //Map Domain Model to DTO using AutoMapper
            return Ok(mapper.Map<List<WalksDto>>(walkDomainModel));
        }

        //GET Walk By Id
        //GET : /api/walk/id
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            logger.LogInformation("GetById(): action method invoked");
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }
            logger.LogInformation($"GetById(): walk fetched by Id successfully :  {JsonSerializer.Serialize(walkDomainModel)}");
            return Ok(mapper.Map<WalksDto>(walkDomainModel));
        }

        //Update Walk by Id
        //PUT : /api/walk/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateAysnc([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            logger.LogInformation("UpdateAysnc(): action method invoked");
            //Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return null;
            }
            logger.LogInformation($"UpdateAsync(): walk updated successfully :  {JsonSerializer.Serialize(walkDomainModel)}");
            //Map Domain model to DTO
            return Ok(mapper.Map<Walk>(walkDomainModel));
        }

        //Delete a walk by Id
        //DELETE :  /api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            logger.LogInformation("Delete(): action method invoked");
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);
            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }
            logger.LogInformation($"Delete(): walk deleted successfully :  {JsonSerializer.Serialize(deletedWalkDomainModel)}");
            //Map Domain Model to DTO
            return Ok(mapper.Map<WalksDto>(deletedWalkDomainModel));
        }
    }
}
