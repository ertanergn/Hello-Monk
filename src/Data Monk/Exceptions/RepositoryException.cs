using System;

namespace Monk.Data.Exceptions
{
    /// <summary>
    /// Custom exception for indentifying repository operations
    /// </summary>
    public class RepositoryException : Exception
    {
        public RepositoryException(string message)
            : base(message)
        { }

        public RepositoryException(string message, Exception ex)
            : base(message, ex)
        { }
    }
}
