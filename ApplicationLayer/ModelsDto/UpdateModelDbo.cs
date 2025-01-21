﻿
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.ModelsDto
{
    public class UpdateModelDto
    {
        [Required]
        public int Id { get; set; }
        public string? Description { get; set; }
    }
}
