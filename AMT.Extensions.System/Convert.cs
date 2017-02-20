using global::System;
using System.Text;


namespace AMT.Extensions.System
{
    public static class Convert
    {
        /*
            ToBase64UrlString and FromBase64UrlString
            based on comments in https://github.com/dotnet/orleans/issues/2380
        */
        public static string ToBase64UrlString(char[] data, Encoding encoding = null)
        {
            // Use UTF8 by default
            if (null == encoding) { encoding = new UTF8Encoding(); }

            return global::System.Convert.ToBase64String(encoding.GetBytes(data))
                .Replace("=", string.Empty) // replace = end padding
                .Replace('+', '-')          // replace + with -
                .Replace('/', '_');         // replace / with _
        }


        public static string ToBase64UrlString(byte[] data, Encoding encoding = null)
        {
            // Use UTF8 by default
            if (null == encoding) { encoding = new UTF8Encoding(); }

            return ToBase64UrlString(encoding.GetString(data, 0, data.Length).ToCharArray(), encoding);
        }


        public static byte[] FromBase64UrlString(string base64UrlData)
        {
            return global::System.Convert.FromBase64String(
                base64UrlData.Replace('-', '+').Replace('_', '/')
                .PadRight(base64UrlData.Length + (4 - base64UrlData.Length % 4) % 4, '=')
            );
        }

    }
}