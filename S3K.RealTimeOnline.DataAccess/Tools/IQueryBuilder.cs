namespace S3K.RealTimeOnline.DataAccess.Tools
{
    public  interface IQueryBuilder
    {
        string CreateSelect();

        string CreateUpdate();

        string CreateInsert();

        string CreateDelete();
    }
}
