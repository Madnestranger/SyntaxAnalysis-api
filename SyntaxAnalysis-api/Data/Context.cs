using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SyntaxAnalysis_api.Models;

public class DatabaseContext : DbContext
{
    public DbSet<Flex> Flexes { get; set; }
    public DbSet<Nom> Nom1 { get; set; }
    public DbSet<Indent> Indents { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<Gr> Gr { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "mph_ua.db" };
        var connectionString = connectionStringBuilder.ToString();
        var connection = new SqliteConnection(connectionString);

        optionsBuilder.UseSqlite(connection);
    }
}