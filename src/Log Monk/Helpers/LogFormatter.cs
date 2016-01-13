using System;

namespace Monk.Log.Helpers
{
    public static class LogFormatter
    {
        public static string BuildMessageFromParams(params string[] message)
        {
            if (message != null)
                switch (message.Length)
                {
                    case 0:
                        return string.Empty;
                    case 1:
                        return message[0];
                    default:
                        return FormatLogMessage(message);
                }
            return "Unable to Log. Provide message!";
        }

        internal static string FormatLogMessage(string[] message)
        {
            var formatString = message[0];
            var stringParams = new string[message.Length - 1];
            for (var i = 1; i < message.Length; i++)
                stringParams[i - 1] = message[i];
            return String.Format(formatString, stringParams);
        }
    }
}
