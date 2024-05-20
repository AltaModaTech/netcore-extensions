# AMT.Extensions.System

Extensions for .NET's System namespace

## Base64 Support for Urls 

* Convert.ToBase64Url - encodes using base64url
* Convert.FromBase64Url - decodes from base64url

## Labeled Collection [deprecated]

* Collections.Generic.ILabeledCollection - provides labeling for collections.
* Collections.Generic.LabeledCollection - implements ILabeledCollection.

### Deprecation Notice

Our labeled collections were designed for better performance with large data sets. We find little performance difference with the data sets we have encountered, so there is little value in maintaining them.

Our recommendations for associating labels with values are:

* List<KeyValuePair<string, TItem>> - a collection of items associated with _nonunique_ labels.
* Dictionary<string, TItem> - a collection of items associated with _unique_ labels.
