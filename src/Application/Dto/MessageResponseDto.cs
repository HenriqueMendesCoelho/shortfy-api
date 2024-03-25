namespace suavesabor_api.src.Application.Dto
{
    public class MessageResponseDto
    {
        public bool Success { get; set; }
        public required int Code { get; set; }
        public required string Message { get; set; }
    }
}
