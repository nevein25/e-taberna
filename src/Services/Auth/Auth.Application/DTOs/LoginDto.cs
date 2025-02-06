﻿using System.ComponentModel.DataAnnotations;

namespace Auth.Application.DTOs;
public record LoginDto([Required]string Username,[Required] string Password);