using Northwind.WebRole.Domain;
using Northwind.WebRole.Tools;

namespace Northwind.WebRole.QueryHandlers
{
    public class GenericSelectByIdQuery : IQuery<Entity>
    {
        public object Id { get; set; }
    }
}