namespace DesktopApp.Domain.Exceptions
{
    public class CannotUpdateEntityException : Exception
    {
        public CannotUpdateEntityException()
        {
        }

        public CannotUpdateEntityException(string message)
            : base(message)
        {
        }

        public CannotUpdateEntityException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
