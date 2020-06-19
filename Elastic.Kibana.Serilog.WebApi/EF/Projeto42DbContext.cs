using Elastic.Kibana.Serilog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elastic.Kibana.Serilog.EF
{
    public class Projeto42DbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<Cidade> Cidades { get; set; }


        public Projeto42DbContext(DbContextOptions<Projeto42DbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations<Projeto42DbContext>();
        }
    }
}