﻿namespace suavesabor_api.Authentication.Endpoints.Dto
{
    public class LoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
