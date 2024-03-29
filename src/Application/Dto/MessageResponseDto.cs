namespace suavesabor_api.src.Application.Dto
{
    public class MessageResponseDto
    {
        public bool Success { get; set; }
        public required int Code { get; set; }
        public required string Message { get; set; }

        public static MessageResponseDto Create(string message, int code)
        {
            var Sucess = code == 200 || code == 201 || code == 204;
            return new MessageResponseDto { Success = Sucess, Code = code, Message = message };
        }
    }
}
