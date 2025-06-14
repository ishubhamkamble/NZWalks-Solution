﻿using AutoMapper;
using NzWalksAPI.Models.Domain;
using NzWalksAPI.Models.DTO;

namespace NzWalksAPI.Mappings
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRequestRegionDto, Region>().ReverseMap();

        }
    }
}
