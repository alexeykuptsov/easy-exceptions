using System.Data.Entity;

namespace EasyExceptions.Tests.EfContext
{
    public class BooksContext : DbContext
    {
        public IDbSet<Book> Books { get; set; } 
    }
}