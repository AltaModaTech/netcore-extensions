// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;


namespace AMT.Extensions.System.Collections.Generic
{
    public class LabeledCollection<TLabel, TContent> : ILabeledCollection<TLabel, TContent>
    {

        #region ICollection impl
        /*
            All ICollection impls delegate to ILabeledCollection
        */
        void ICollection<KeyValuePair<TLabel, TContent>>.Add(KeyValuePair<TLabel, TContent> kvp)
        {
            ((ILabeledCollection<TLabel, TContent>)this).Add(kvp);
        }
        void ICollection<KeyValuePair<TLabel, TContent>>.Clear()
        {
            ((ILabeledCollection<TLabel, TContent>)this).Clear();
        }
        bool ICollection<KeyValuePair<TLabel, TContent>>.Contains(KeyValuePair<TLabel, TContent> kvp)
        {
            return ((ILabeledCollection<TLabel, TContent>)this).Contains(kvp);
        }
        void ICollection<KeyValuePair<TLabel, TContent>>.CopyTo(KeyValuePair<TLabel, TContent>[] array, int arrayIndex)
        {
            ((ILabeledCollection<TLabel, TContent>)this).CopyTo(array, arrayIndex);
        }
        int ICollection<KeyValuePair<TLabel, TContent>>.Count
        {
            get { return ((ILabeledCollection<TLabel, TContent>)this).Count; }
        }
        bool ICollection<KeyValuePair<TLabel, TContent>>.IsReadOnly
        {
            get { return ((ILabeledCollection<TLabel, TContent>)this).IsReadOnly; }
        }
        bool ICollection<KeyValuePair<TLabel, TContent>>.Remove(KeyValuePair<TLabel, TContent> kvp)
        {
            return ((ILabeledCollection<TLabel, TContent>)this).Remove(kvp);
        }

        #endregion ICollection impl

        public void Add(KeyValuePair<TLabel, TContent> kvp)
        {
            collection.Add(kvp);
            labels.Add(kvp.Key);
        }
        public void Clear()
        {
            collection.Clear();
            labels.Clear();
        }
        /// <summary>
        /// Determines if a specific label exists in the collection.
        /// </summary>
        /// <param name="kvp">The key-value pair to find. Only the key (label) portion is used.</param>
        /// <returns>True if the label is in the collection; false otherwise.</returns>
        public bool Contains(KeyValuePair<TLabel, TContent> kvp)
        {
            return labels.Contains(kvp.Key);
        }

        // Copies the elements of the ICollection<T> to an Array, starting at a particular Array index.
        public void CopyTo(KeyValuePair<TLabel, TContent>[] array, int arrayIndex)
        {
            int i = 0;
            foreach (var kvp in this.collection)
            {
                array[i++] = kvp;
            }
        }

        
        public int Count { get { return collection.Count; } }
        public bool IsReadOnly  { get { return false; } }
        public bool Remove(KeyValuePair<TLabel, TContent> kvp)
        {
            // TODO: handle cases of one Remove failing
            return (collection.Remove(kvp) && labels.Remove(kvp.Key));
        }


        #region ILabeledCollection impl

        void ILabeledCollection<TLabel, TContent>.Add(KeyValuePair<TLabel, TContent> kvp)
        {
            this.Add(kvp);
        }
        void ILabeledCollection<TLabel, TContent>.Clear()
        {
            this.Clear();
        }
        /// <summary>
        /// Determines if a specific label exists in the collection.
        /// </summary>
        /// <param name="kvp">The key-value pair to find. Only the key (label) portion is used.</param>
        /// <returns>True if the label is in the collection; false otherwise.</returns>
        bool ILabeledCollection<TLabel, TContent>.Contains(KeyValuePair<TLabel, TContent> kvp)
        {
            return this.Contains(kvp);
        }
        void ILabeledCollection<TLabel, TContent>.CopyTo(KeyValuePair<TLabel, TContent>[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }
        int ILabeledCollection<TLabel, TContent>.Count { get { return this.Count; } }
        bool ILabeledCollection<TLabel, TContent>.IsReadOnly  { get { return this.IsReadOnly; } }
        bool ILabeledCollection<TLabel, TContent>.Remove(KeyValuePair<TLabel, TContent> kvp)
        {
            // TODO: handle cases of one Remove failing
            return this.Remove(kvp);
        }

        #endregion ILabeledCollection impl


        #region IEnumerable<KeyValuePair<TLabel, TContent>>

        public IEnumerator<KeyValuePair<TLabel, TContent>> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        #endregion IEnumerable<KeyValuePair<TLabel, TContent>>


        private List<TLabel> labels = new List<TLabel>();
        private List<KeyValuePair<TLabel, TContent>> collection = new List<KeyValuePair<TLabel, TContent>>();
    }

}