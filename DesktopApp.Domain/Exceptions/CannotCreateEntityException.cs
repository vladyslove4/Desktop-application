namespace DesktopApp.Domain.Exceptions
{
    public class CannotCreateEntityException : Exception
    {
        public CannotCreateEntityException()
        {
        }

        public CannotCreateEntityException(string message)
            : base(message)
        {
        }

        public CannotCreateEntityException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
