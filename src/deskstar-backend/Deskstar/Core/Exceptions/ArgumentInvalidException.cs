namespace Deskstar.Core.Exceptions;

public class ArgumentInvalidException : ArgumentException
{
    public ArgumentInvalidException(string message) : base(message) { }
}