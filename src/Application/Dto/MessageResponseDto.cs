using shortfy_api.src.Application.Domain;

namespace shortfy_api.src.Application.Dto
{
    public class MessageResponseDto
    {
        public bool Success { get; set; } = false;
        public int Code { get; set; } = 0;
        public string Message { get; set; } = string.Empty;

        public MessageResponseDto() { }

        public MessageResponseDto(MessageDomain domain)
        {
            Success = domain.Success;
            Code = domain.Code;
            Message = domain.Message;
        }

        public static MessageResponseDto Create(string message, int code)
        {
            var Sucess = code == 200 || code == 201 || code == 204;
            return new MessageResponseDto { Success = Sucess, Code = code, Message = message };
        }
    }
}
