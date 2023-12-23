namespace DesktopApp.Domain.Exceptions
{
    public class CannotFindEntityException : Exception
    {
        public CannotFindEntityException()
        {
        }

        public CannotFindEntityException(string message)
            : base(message)
        {
        }

        public CannotFindEntityException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
