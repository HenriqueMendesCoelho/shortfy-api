namespace shortfy_api.src.Application.Exceptions
{
    public class TokenNotValidExpcetion : Exception
    {
        public TokenNotValidExpcetion() : base("Token claims is not valid") { }
    }
}
