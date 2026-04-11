// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AMT.Extensions.System.IO;
using FluentAssertions;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using System.Linq;

namespace Test.AMT.Extensions.System.IO
{
    [ExcludeFromCodeCoverage]
    public class DirectoryInfoTests
    {

        [Fact]
        public void can_find()
        {
            var pathsToExclude = new List<string>
            {
                "/Users/jb/src/github.com/jburnett"
            };
            var opts = new SearchOptions
            {
                ExcludeByPath = pathsToExclude,
                ExcludeByPattern = new List<string>()
            };

            var di = new DirectoryInfo("/Users/jb/src/github.com");
            var found = di.Find(opts);

            found.Count().Should().BeGreaterThan(0);
        }


        [Fact]
        public void can_exclude_by_paths() 
        {
            var pathsToExclude = new List<string>
            {
                "/Users/jb/src/github.com/jburnett"
            };
            var opts = new SearchOptions
            {
                ExcludeByPath = pathsToExclude,
                ExcludeByPattern = new List<string>()
            };

            var beginDir = new DirectoryInfo("/Users/jb/src/github.com");
            foreach (DirectoryInfo di in beginDir.EnumerateDirectories(opts))
            {
                foreach (var p2e in pathsToExclude)
                {
                    di.FullName.Should().NotBe(p2e);
                }
            }

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
