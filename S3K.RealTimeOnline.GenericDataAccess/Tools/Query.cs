namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public class Query<TResult> : IQuery<TResult>
    {
        public Query()
        {
            Page = 1;
            PageSize = 200;
        }

        public int? Page { get; set; }

        public int? PageSize { get; set; }
    }
}