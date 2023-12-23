namespace DesktopApp.Domain.Exceptions
{
    public class CannotDeleteEntityException : Exception
    {

        public CannotDeleteEntityException()
        {
        }

        public CannotDeleteEntityException(string message)
            : base(message)
        {
        }

        public CannotDeleteEntityException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
