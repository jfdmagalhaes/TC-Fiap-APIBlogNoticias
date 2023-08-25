using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityFramework.Config;
public class NoticiaConfiguration : IEntityTypeConfiguration<NoticiaDto>
{
    public void Configure(EntityTypeBuilder<NoticiaDto> builder)
    {
        builder.ToTable("Noticias");

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Titulo).HasColumnName("Titulo").HasMaxLength(60);
        builder.Property(p => p.Descricao).HasColumnName("Conteudo").HasMaxLength(100);
        builder.Property(p => p.DataPublicacao).HasColumnName("DataPublicacao");
        builder.Property(p => p.Autor).HasColumnName("Autor");

        builder.HasKey(p => p.Id);
    }
}