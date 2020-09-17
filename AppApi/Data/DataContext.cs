using AppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AppApi.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Value> Values { get; set; }

  }
}