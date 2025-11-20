// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Ext = AMT.Extensions.System;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Test.AMT.Extensions.System
{
    [ExcludeFromCodeCoverage]
    public class ConvertTests
    {

        [Fact]
        public void can_encode_decode_string_arrays() 
        {
            foreach (var unencoded in _validStrings)
            {
                // encode with base64url
                string encoded = Ext.Convert.ToBase64UrlString(unencoded);
                // decode the base64url encoded
                string decoded = Convert.ToString(Ext.Convert.FromBase64UrlString(encoded));

                // decoded should be equal to the original
                Assert.Equal(unencoded, decoded);
            }
        }


        [Fact]
        public void can_encode_decode_Uris() 
        {
            foreach (var unencoded in _validUris)
            {
                // encode with base64url
                string encoded = Ext.Convert.ToBase64UrlString(unencoded);
                // decode the base64url encoded
                string decoded = Convert.ToString(Ext.Convert.FromBase64UrlString(encoded));

                // decoded should be equal to the original
                Assert.Equal(unencoded.AbsoluteUri, decoded);
            }
        }


        [Fact]
        public void excp_on_null()
        {
            // Verify proper exception when string is null
            string nullStr = null;

            Action act = () => {
                Ext.Convert.ToBase64UrlString(nullStr);
            };

            act.Should().Throw<ArgumentNullException>();


            // Verify proper exception when Uri is null
            Uri nullUri = null;

            act = () => {
                Ext.Convert.ToBase64UrlString(nullUri);
            };

            act.Should().Throw<ArgumentNullException>();


            // Verify proper exception when encoded is null
            act = () => {
                Ext.Convert.FromBase64UrlString(null);
            };

            act.Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public void excp_on_empty_string()
        {
            // Verify proper exception when string is empty
            Action act = () => {
                Ext.Convert.ToBase64UrlString(string.Empty);
            };

            act.Should().Throw<ArgumentOutOfRangeException>();


            // Verify proper exception when encoded is empty
            act = () => {
                Ext.Convert.FromBase64UrlString(string.Empty);
            };

            act.Should().Throw<ArgumentOutOfRangeException>();
        }



        private List<string> _validStrings = new List<string> {
            "j"
            ,"jb7466"
            ,"https://AltaModaTech.com/?qryWithNoPurpose=123abc&purposeless=!@#$%^&*()"
        };

        private List<Uri> _validUris = new List<Uri> {
            new Uri("https://AltaModaTech.com/")
            ,new Uri("https://AltaModaTech.com/?qryWithNoPurpose=123abc&purposeless=!@#$%^&*()")
            ,new Uri("https://username@AltaModaTech.com:7466/?qryWithNoPurpose=123abc&purposeless=!@#$%^&*()_+&1234567890-=")
            // TODO: add more chars in url; add escaped chars
        };
    }
}
