using Ext = AMT.Extensions.System;
using System;
using System.Collections.Generic;
using Xunit;


namespace SystemTests
{
    public class ConvertTests
    {
        public void can_encode_decode_string_arrays() 
        {
            foreach (var unencoded in _validStrings)
            {
                // encode with base64url
                string encoded = Ext.Convert.ToBase64UrlString(unencoded.ToCharArray());
                // decode the base64url encoded
                string decoded = Convert.ToString(Ext.Convert.FromBase64UrlString(encoded));

                // decoded should be equal to the original
                Assert.Equal(unencoded, decoded);
            }
        }


        private List<string> _validStrings = new List<string> {
            "j", "jb7466", 
            "https://AltaModaTech.com/?qryWithNoPurpose=123abc&purposeless=!@#$%^&*()",
            string.Empty
        };

    }
}
