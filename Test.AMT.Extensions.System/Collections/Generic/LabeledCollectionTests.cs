// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Ext = AMT.Extensions.System.Collections.Generic;
using FluentAssertions;
using global::System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using AMT.Extensions.System.Collections.Generic;


namespace Test.AMT.Extensions.System.Collections.Generic
{
    [ExcludeFromCodeCoverage]
    public class LabeledCollectionTests
    {

        [Fact]
        public void can_add_and_remove_kvp()
        {
            var c = new Ext.LabeledCollection<string,string>();

            foreach (var kvp in _validKvps)
            {
                c.Add(kvp);
            }
            c.Count.Should().Be(_validKvps.Count);

            foreach (var kvp in _validKvps)
            {
                c.Contains(kvp).Should().BeTrue();
            }

            foreach (var kvp in _validKvps)
            {
                c.Remove(kvp).Should().BeTrue();
            }
            c.Count.Should().Be(0);

            c.IsReadOnly.Should().BeFalse();
        }


        [Fact]
        public void allows_duplicate_labels()
        {
            var c = new Ext.LabeledCollection<string,string>();

            for (int i=0; i<2; ++i)
            {
                foreach (var kvp in _validKvps)
                {
                    c.Add(kvp);
                }
            }
            c.Count.Should().Be(_validKvps.Count*2);
        }


        [Fact]
        public void can_add_and_remove_kvp_as_ICollection()
        {
            ICollection<KeyValuePair<string,string>> c = new Ext.LabeledCollection<string,string>();
            ICollection_add_remove_kvp(c);
        }


        [Fact]
        public void can_add_and_remove_kvp_as_ILabeledCollection()
        {
            Ext.ILabeledCollection<string,string> c = new Ext.LabeledCollection<string,string>();
            ILabeledCollection_add_remove_kvp(c);
        }

        [Fact]
        public void can_use_empty_collection()
        {
            Ext.ILabeledCollection<string,string> c = new Ext.LabeledCollection<string,string>();

            c.Count.Should().Be(0);
            c.Clear();
            c.Remove(_validKvps[0]).Should().BeFalse();
            c.IsReadOnly.Should().BeFalse();
        }

        [Fact]
        public void can_CopyTo()
        {
            ILabeledCollection<string,string> ilabeled = new Ext.LabeledCollection<string,string>();
            ICollection<KeyValuePair<string, string>> iColl = new Ext.LabeledCollection<string,string>();

            foreach (var kvp in _validKvps)
            {
                ilabeled.Add(kvp);
                iColl.Add(kvp);
            }
            ilabeled.Count.Should().Be(_validKvps.Count);
            iColl.Count.Should().Be(_validKvps.Count);

            // Use ILabeledCollection.CopyTo
            var fromLabeled = new KeyValuePair<string, string>[_validKvps.Count];
            ilabeled.CopyTo(fromLabeled, 0);
            // Use ICollection.CopyTo
            var fromCollection = new KeyValuePair<string, string>[_validKvps.Count];
            iColl.CopyTo(fromCollection, 0);

            // Verify values were copied correctly
            int i = 0;
            foreach (var kvp in _validKvps)
            {
                ilabeled.Contains(kvp).Should().BeTrue();
                iColl.Contains(kvp).Should().BeTrue();
                fromLabeled[i].Key.Should().BeEquivalentTo(kvp.Key);
                fromCollection[i].Key.Should().BeEquivalentTo(kvp.Key);
                ++i;
            }

            foreach (var kvp in _validKvps)
            {
                ilabeled.Remove(kvp).Should().BeTrue();
                iColl.Remove(kvp).Should().BeTrue();
            }

            ilabeled.Clear();
            iColl.Clear();

        }



        private List<KeyValuePair<string, string>> _validKvps = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("foo", "bar")
            ,new KeyValuePair<string, string>("a", "A")
            ,new KeyValuePair<string, string>("A", "A")
            ,new KeyValuePair<string, string>(" has spaces ", " the value has spaces, too ")
        };


        private void ICollection_add_remove_kvp(ICollection<KeyValuePair<string, string>> c)
        {
            foreach (var kvp in _validKvps)
            {
                c.Add(kvp);
            }
            c.Count.Should().Be(_validKvps.Count);

            foreach (var kvp in _validKvps)
            {
                c.Contains(kvp).Should().BeTrue();
            }

            foreach (var kvp in _validKvps)
            {
                c.Remove(kvp).Should().BeTrue();
            }
            c.Count.Should().Be(0);

            c.IsReadOnly.Should().BeFalse();

            c.Clear();
        }


        private void ILabeledCollection_add_remove_kvp(Ext.ILabeledCollection<string, string> c)
        {
            foreach (var kvp in _validKvps)
            {
                c.Add(kvp);
            }
            c.Count.Should().Be(_validKvps.Count);

            foreach (var kvp in _validKvps)
            {
                c.Contains(kvp).Should().BeTrue();
            }

            foreach (var kvp in _validKvps)
            {
                c.Remove(kvp).Should().BeTrue();
            }
            c.Count.Should().Be(0);

            c.IsReadOnly.Should().BeFalse();

            c.Clear();
        }

    }
}

