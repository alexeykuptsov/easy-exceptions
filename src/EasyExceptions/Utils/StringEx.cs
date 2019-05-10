using System.Linq;

namespace EasyExceptions.Utils
{
    public static class StringEx
    {
        public static string QuoteIfContainsWhitespaces(this string message)
        {
            for (int i = 0; i < message.Length; i++)
                if (char.IsWhiteSpace(message[i]))
                    return "``" + message + "``";

            return message;
        }
    }
}