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

        public static string ToBase64UrlString(Uri uri)
        {
            if (null == uri)  { throw new ArgumentNullException("uri"); }

            return global::System.Convert.ToBase64String( Encoding.UTF8.GetBytes(uri.AbsoluteUri))
                .Replace("=", string.Empty).Replace('+', '-').Replace('/', '_');
        }


        public static string ToBase64UrlString(string key)
        {
            if (null == key)  { throw new ArgumentNullException("key"); }
            if (0 == key.Length)  { throw new ArgumentOutOfRangeException("key", "Must not be empty."); }

            string encodedKey = global::System.Convert.ToBase64String(Encoding.UTF8.GetBytes(key))
                .Replace("=", string.Empty).Replace('+', '-').Replace('/', '_');

            return encodedKey;
        }


        public static string FromBase64UrlString(string encodedKey)
        {
            if (null == encodedKey)  { throw new ArgumentNullException("encodedKey"); }
            if (0 == encodedKey.Length)  { throw new ArgumentOutOfRangeException("encodedKey", "Must not be empty."); }

            var forDecode = global::System.Convert.FromBase64String(
                encodedKey.Replace('-', '+').Replace('_', '/')
                .PadRight(encodedKey.Length + (4 - encodedKey.Length % 4) % 4, '='));
            string decodedKey = Encoding.UTF8.GetString(forDecode, 0, forDecode.Length);
            
            return decodedKey;
        }

    }
}