﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.DTOs
{
    public class UserToLogoutDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
