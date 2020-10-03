using BookListMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Data
{
    public class BookListDbContext : DbContext
    {
        public BookListDbContext(DbContextOptions<BookListDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }

    }
}