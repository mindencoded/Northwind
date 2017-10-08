using System;
using System.Collections.Generic;
using S3K.RealTimeOnline.Domain;

namespace S3K.RealTimeOnline.DataAccess.Tools
{
    public class QueryBuilder<TEntity> : IQueryBuilder where TEntity : Entity
    {
        private readonly IDictionary<string, object> _dictionary;
        private readonly dynamic _dynamic;

        public QueryBuilder(IDictionary<string, object> dictionary)
        {
            _dictionary = dictionary;
        }

        public QueryBuilder(object dynamic)
        {
            _dynamic = dynamic;
        }

        public string CreateSelect()
        {
            string query = null;

            if (_dictionary != null)
                query = "";
            else if (_dynamic != null)
                query = "";

            return query;
        }

        public string CreateUpdate()
        {
            throw new NotImplementedException();
        }

        public string CreateInsert()
        {
            throw new NotImplementedException();
        }

        public string CreateDelete()
        {
            throw new NotImplementedException();
        }

        public static string CrateQuery(string query)
        {
            throw new NotImplementedException();
        }
    }
}