using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            //Map DTO to domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
            await walkRepository.CreateAsync(walkDomainModel);

            //map Domain model to DTO
            return Ok(mapper.Map<WalksDto>(walkDomainModel));
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
    }
}
