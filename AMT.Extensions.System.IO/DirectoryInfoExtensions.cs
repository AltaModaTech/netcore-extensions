// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using global::System.IO;
using System.Collections.Generic;


namespace AMT.Extensions.System.IO
{
    public static class DirectoryInfoExtensions
    {
        // Std method:
        // public static System.IO.FileInfo[] GetFiles(string searchPattern, System.IO.EnumerationOptions enumerationOptions);

        // Std method:
        // public System.Collections.Generic.IEnumerable<System.IO.DirectoryInfo> EnumerateDirectories(string searchPattern, System.IO.SearchOption searchOption);
        public static IEnumerable<DirectoryInfo> EnumerateDirectories(this DirectoryInfo diCurr, SearchOptions opts)
        {
            return diCurr.EnumerateDirectories("*");
        }

        public static IEnumerable<DirectoryInfo> Find(this DirectoryInfo current, SearchOptions opts)
        {
            return current.EnumerateDirectories("*");
        }
    }

    public class SearchOptions
    {
        public List<string> ExcludeByPath { get; set; }
        public List<string> ExcludeByPattern { get; set; }
    }
}