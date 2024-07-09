namespace Application.Exceptions;

public class EntityAlreadyExistException : Exception
{
    public EntityAlreadyExistException(string message) : base(message) { }
}
// add not found exception