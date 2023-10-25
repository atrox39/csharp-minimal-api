using EjerciciosProgramacion.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace EjerciciosProgramacion
{
  class MongoDBContext : DbContext
  {
    public DbSet<User> Users { get; init; }

    public static MongoDBContext Create(IMongoDatabase database)
    {
      return new(new DbContextOptionsBuilder<MongoDBContext>().UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName).Options);
    }

    public MongoDBContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<User>().ToCollection("users");
    }
  }
}
