using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NzWalksAPI.Models.Domain;
using NzWalksAPI.Models.DTO;
using NzWalksAPI.Repositories;

namespace NzWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }



        //Create Walk
        //POST() : /api/walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                //Map DTO to domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
                await walkRepository.CreateAsync(walkDomainModel);

                //map Domain model to DTO
                return Ok(mapper.Map<WalksDto>(walkDomainModel));
            }
            return BadRequest(ModelState);
        }

        //GET All Walks
        //GET : /api/Walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walkDomainModel = await walkRepository.GetAllAysnc();

            //Map Domain Model to DTO using AutoMapper
            return Ok(mapper.Map<List<WalksDto>>(walkDomainModel));
        }

        //GET Walk By Id
        //GET : /api/walk/id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalksDto>(walkDomainModel));
        }

        //Update Walk by Id
        //PUT : /api/walk/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateAysnc([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                //Map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return null;
                }

                //Map Domain model to DTO
                return Ok(mapper.Map<Walk>(walkDomainModel));
            }
            return BadRequest(ModelState);
        }

        //Delete a walk by Id
        //DELETE :  /api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);
            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Model to DTO
            return Ok(mapper.Map<WalksDto>(deletedWalkDomainModel));
        }
    }
}
