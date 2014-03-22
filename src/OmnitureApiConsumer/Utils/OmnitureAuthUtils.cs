using System;
using System.Security.Cryptography;
using System.Text;

namespace OmnitureApiConsumer.Utils
{
    /// <summary>
    /// This utility class provides helper methods to authenticate with Omniture Analytics Reporting API 1.4.
    /// 
    /// Sample test data generated using the Omniture API explorer:
    /// https://developer.omniture.com/en_US/get-started/api-explorer
    /// Username: randomName:RandomCompany
    /// Secret: 92cc3685bd9b9f5c9d141fa241aa747e
    /// Generated Header: X-WSSE: UsernameToken Username="randomName:RandomCompany", PasswordDigest="e2fSxqZDVgAQEI9OCvY/vho4C2k=", Nonce="MTRkZTJhMGNiNGMwYWZlOWU5YmRmYzhk", Created="2014-03-16T04:10:43Z"
    /// </summary>
    internal class OmnitureAuthUtils
    {
        #region Private Properties

        private const string HeadersFormatString =
            "X-WSSE: UsernameToken Username=\"{0}\", PasswordDigest=\"{1}\", Nonce=\"{2}\", Created=\"{3}\"";

        private static readonly RNGCryptoServiceProvider Random = new RNGCryptoServiceProvider();

        #endregion

        /// <summary>
        /// Given a username and secret key, generates the Omniture API authentication headers.
        /// </summary>
        /// <param name="username">Username string</param>
        /// <param name="secret">Secret key string</param>
        /// <returns>X-WSSE Header string</returns>
        public static string GetAuthenticationHeaders(string username, string secret)
        {
            var nonce = GenerateNonce(24);
            var timestamp = GenerateTimestamp();
            var digest = GetBase64Digest(nonce, timestamp, secret);
            return string.Format(HeadersFormatString, username, digest, EncodeToBase64(nonce), timestamp);
        }

        #region Private Helper Methods

        /// <summary>
        /// Encripts secret key with base-64 SHA1 digest as specified by the Omniture API.
        /// </summary>
        /// <param name="nonce">Nonce string</param>
        /// <param name="timeCreated">ISO8601 time string in "yyyy-MM-ddTHH:mm:ssZ" format</param>
        /// <param name="secret">Secret key string</param>
        /// <returns>SHA1 digest encoded to base-64</returns>
        private static string GetBase64Digest(string nonce, string timeCreated, string secret)
        {
            var input = nonce + timeCreated + secret;
            var sha = new SHA1Managed();
            var asciiEncoding = new ASCIIEncoding();
            var byteData = asciiEncoding.GetBytes(input);
            var digest = sha.ComputeHash(byteData);
            return Convert.ToBase64String(digest);
        }

        /// <summary>
        /// Generates an arbitrary number of specified length to be used only once for authentication.
        /// The number is base-64 encoded as per the Omniture API specification.
        /// </summary>
        /// <param name="length">Length of the generated nonce</param>
        /// <returns>Nonce of specified length</returns>
        private static string GenerateNonce(int length)
        {
            var data = new byte[length];
            Random.GetNonZeroBytes(data);
            return EncodeToBase16(MD5.Create().ComputeHash(data));
        }

        /// <summary>
        /// Generates the timestamp in ISO8601 format ("yyyy-MM-ddTHH:mm:ssZ") as specified by the Omniture API.
        /// </summary>
        /// <returns>ISO8601 formatted timestamp</returns>
        private static string GenerateTimestamp()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        /// <summary>
        /// Converts the given string data to its equivalent base-64 representation.
        /// </summary>
        /// <param name="data">Data string to encode</param>
        /// <returns>Encoded string</returns>
        private static string EncodeToBase64(string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Converts the given byte array to its equivalent base-16 (hex) representation.
        /// </summary>
        /// <param name="data">Byte array to encode</param>
        /// <returns>Encoded string</returns>
        private static string EncodeToBase16(byte[] data)
        {
            var hex = BitConverter.ToString(data);
            return hex.Replace("-", "");
        }

        #endregion

    }
}
