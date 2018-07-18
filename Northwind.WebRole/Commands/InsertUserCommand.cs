namespace Northwind.WebRole.Commands
{
    public class InsertUserCommand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }
    }
}