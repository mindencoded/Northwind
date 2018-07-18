using Northwind.WebRole.Domain;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Queries
{
    public class GenericSelectByIdQuery : IQuery<Entity>
    {
        public object Id { get; set; }
    }
}