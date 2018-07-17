using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace S3K.RealTimeOnline.WebService
{
    public class Mapper<TSource, TDest> where TDest : class
    {
        protected readonly IList<Action<TSource, TDest>> Mappings = new List<Action<TSource, TDest>>();

        public virtual void AddMapping(Action<TSource, TDest> mapping)
        {
            Mappings.Add(mapping);
        }

        protected virtual void CopyMatchingProperties(TSource source, TDest dest)
        {
            foreach (PropertyInfo destProp in typeof(TDest).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite))
            {
                PropertyInfo sourceProp =
                    typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p =>
                        p.Name == destProp.Name && p.PropertyType == destProp.PropertyType);
                if (sourceProp != null)
                {
                    destProp.SetValue(dest, sourceProp.GetValue(source, null), null);
                }
            }
        }

        public virtual TDest MapObject(TSource source, TDest dest)
        {
            CopyMatchingProperties(source, dest);
            foreach (Action<TSource, TDest> action in Mappings)
            {
                action(source, dest);
            }

            return dest;
        }

        public virtual TDest CreateMappedObject(TSource source)
        {
            TDest dest = Activator.CreateInstance<TDest>();
            return MapObject(source, dest);
        }
    }
}