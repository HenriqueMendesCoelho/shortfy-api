namespace shortfy_api.src.Application.Domain
{
    public class MessageDomain
    {
        public bool Success { get; set; }
        public required int Code { get; set; }
        public required string Message { get; set; }
    }
}
