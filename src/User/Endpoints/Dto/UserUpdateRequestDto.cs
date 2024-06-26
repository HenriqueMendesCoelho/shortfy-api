﻿using shortfy_api.User.Domain;

namespace shortfy_api.src.User.Endpoints.Dto
{
    public class UserUpdateRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public UserDomain ToDomain() { return new UserDomain { Name = Name, Email = Email }; }
    }
}
