// Esta clase es el "puente" entre C# y la base de datos SecaDB.
// Entity Framework Core usa este DbContext para leer y escribir datos.

using Microsoft.EntityFrameworkCore;
using SecaBackend.Models;

namespace SecaBackend.Data
{
    public class SecaDbContext : DbContext
    {
        // El constructor recibe las opciones, incluyendo la cadena de conexión.
        public SecaDbContext(DbContextOptions<SecaDbContext> options) : base(options)
        {
        }

        // Cada DbSet representa una tabla en la base de datos.

        // Historial del chatbot.
        public DbSet<ChatLog> ChatLogs { get; set; }

        // Historial de uso de las calculadoras.
        public DbSet<CalculatorLog> CalculatorLogs { get; set; }

        // Leads o contactos de clientes.
        public DbSet<Lead> Leads { get; set; }

        // Aquí puedo personalizar detalles de las tablas si lo necesito.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Me aseguro de que los nombres de las tablas sean exactamente estos.
            modelBuilder.Entity<ChatLog>().ToTable("ChatLogs");
            modelBuilder.Entity<CalculatorLog>().ToTable("CalculatorLogs");
            modelBuilder.Entity<Lead>().ToTable("Leads");
        }
    }
}
