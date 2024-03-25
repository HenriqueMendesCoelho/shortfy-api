namespace suavesabor_api.src.Authentication.Endpoints.Dto
{
    public class LoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
