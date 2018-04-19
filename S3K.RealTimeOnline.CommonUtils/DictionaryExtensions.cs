using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace S3K.RealTimeOnline.CommonUtils
{
    public static class DictionaryExtensions
    {
        public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
        {
            ExpandoObject expando = new ExpandoObject();
            IDictionary<string, object> expandoDic = expando;
            // go through the items in the dictionary and copy over the key value pairs)
            foreach (KeyValuePair<string, object> kvp in dictionary)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                if (kvp.Value is IDictionary<string, object>)
                {
                    ExpandoObject expandoValue = ((IDictionary<string, object>) kvp.Value).ToExpando();
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                else if (kvp.Value is ICollection)
                {
                    // iterate through the collection and convert any strin-object dictionaries
                    // along the way into expando objects
                    IList<object> itemList = new List<object>();
                    foreach (object item in (ICollection) kvp.Value)
                    {
                        if (item is IDictionary<string, object>)
                        {
                            ExpandoObject expandoItem = ((IDictionary<string, object>) item).ToExpando();
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    expandoDic.Add(kvp.Key, itemList);
                }
                else
                {
                    expandoDic.Add(kvp);
                }
            }
            return expando;
        }

        public static string GetKey(this IDictionary<string, object> dictionary, object value)
        {
            string key = null;
            foreach (KeyValuePair<string, object> pair in dictionary)
            {
                if (pair.Value.Equals(value))
                {
                    key = pair.Key;
                    break;
                }
            }
            return key;
            //return dictionary.FirstOrDefault(x => x.Value.Equals(value)).Key;
        }
    }
}