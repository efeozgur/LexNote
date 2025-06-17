using LexNote.Models;
using Microsoft.EntityFrameworkCore;

namespace LexNote.Context
{
    public class MyContext : DbContext
    {
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Notum> Notlar { get; set; }

        public MyContext(DbContextOptions<MyContext> options)
            : base(options) { }
    }
}