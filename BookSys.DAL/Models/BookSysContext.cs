using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookSys.DAL.Models
{
    public class BookSysContext : DbContext
    {
        public BookSysContext(DbContextOptions<BookSysContext> options) : base(options)
        {
            
        }
        public DbSet<Book> Books { get; set; }
    }
}
