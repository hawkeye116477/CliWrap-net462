using System.Security;

namespace CliWrap.PowerKit.Extensions;

internal static class StringExtensions
{
    extension(string str)
    {
      
        /// <summary>
        /// Converts the string to a <see cref="SecureString" />.
        /// </summary>
        public SecureString ToSecureString()
        {
            var secure = new SecureString();

            foreach (var ch in str)
            {
                secure.AppendChar(ch);
            }

            secure.MakeReadOnly();
            return secure;
        }
    }
}
