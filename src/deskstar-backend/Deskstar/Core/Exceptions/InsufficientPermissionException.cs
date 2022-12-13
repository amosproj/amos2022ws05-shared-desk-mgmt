namespace Deskstar.Core.Exceptions;

public class InsufficientPermissionException : ArgumentException
{
    public InsufficientPermissionException(string message) : base(message) { }
}