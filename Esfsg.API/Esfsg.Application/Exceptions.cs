namespace Esfsg.Application
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }
        public ValidationException(IEnumerable<string> errors)
            : base("Uma ou mais validações falharam.")
        {
            Errors = errors;
        }
    }

}
