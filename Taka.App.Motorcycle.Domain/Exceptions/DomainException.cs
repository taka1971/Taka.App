using Taka.App.Motor.Domain.Enums;

namespace Taka.App.Motor.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainErrorCode ErrorCode { get; }

        public DomainException(DomainErrorCode errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
