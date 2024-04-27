namespace Taka.App.Authentication.Domain.Exceptions
{
    public class UserFailValidationException : Exception
    {
        public UserFailValidationException(string message)
        : base(message)
        {
        }
    }
}
