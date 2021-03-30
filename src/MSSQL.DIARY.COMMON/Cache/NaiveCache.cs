using System;
using System.Collections.Generic;

namespace MSSQL.DIARY.COMN.Cache
{
    public class NaiveCache<TItem>
    {
        public Dictionary<object, TItem> Cache = new Dictionary<object, TItem>();

        public TItem GetOrCreate(object key, Func<TItem> createItem)
        {

            Cache.AddOrUpdate(key, createItem());
            Cache.TryGetValue(key, out TItem objects);
            return objects;
        }
         
    }
}