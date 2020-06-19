using Elastic.Kibana.Serilog.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elastic.Kibana.Serilog.EF.Configurations
{
    public class CidadeConfiguration : IEntityTypeConfiguration<Cidade>
    {
        public void Configure(EntityTypeBuilder<Cidade> builder)
        {
            builder
                .ToTable("Cidade")
                .HasKey(p => p.Id);

            builder.Property(p => p.Nome).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Uf).HasMaxLength(2).IsRequired();
        }
    }
}