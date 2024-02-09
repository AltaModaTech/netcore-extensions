// Copyright (c) AltaModa Technologies. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;


namespace AMT.Extensions.System.Collections.Generic
{

    public interface ILabeledCollection<TLabel, TContent> : ICollection<KeyValuePair<TLabel, TContent>>
    {
        #region ICollection overrides

        new void Add(KeyValuePair<TLabel, TContent> kvp);
        new void Clear();
        new bool Contains(KeyValuePair<TLabel, TContent> kvp);
        new void CopyTo(KeyValuePair<TLabel, TContent>[] array, int arrayIndex);
        new bool Remove(KeyValuePair<TLabel, TContent> kvp);
        new int Count { get; }
        new bool IsReadOnly { get; }

        #endregion ICollection overrides
    }

}