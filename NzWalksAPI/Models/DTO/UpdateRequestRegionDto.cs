﻿using System.ComponentModel.DataAnnotations;

namespace NzWalksAPI.Models.DTO
{
    public class UpdateRequestRegionDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be a Minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters")]
        public string Name { get; set; }

        public string? RegionImageURL { get; set; }
    }
}
