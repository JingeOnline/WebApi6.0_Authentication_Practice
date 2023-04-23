using System.Security.Cryptography;
using System.Text;

namespace WebApi_RefreshToken.Helpers
{
    public class Sha256Helper
    {
        public static string GetSha256(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text", "Cannot get the sha256 result from null.");
            }
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] textByte = Encoding.UTF8.GetBytes(text);
                byte[] resultByte = mySHA256.ComputeHash(textByte);
                return Encoding.UTF8.GetString(resultByte);
            }
        }

        public static bool VerifySha256(string text, string sha256)
        {
            if (GetSha256(text) == sha256)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
