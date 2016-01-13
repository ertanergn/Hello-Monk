using System;
using System.Text;

namespace Monk.Core.Exceptions
{
    public class DependencyInjectionException : Exception
    {
        private readonly string _message;

        private const string HEADER_MESSAGE =
            "## Exception has been reised in configuring the Depending Injection Container ##";

        public DependencyInjectionException(string message)
        {
            _message = message;
        }

        public override string Message
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine(HEADER_MESSAGE);
                sb.AppendLine(_message);
                return sb.ToString();
            }
        }
    }
}
