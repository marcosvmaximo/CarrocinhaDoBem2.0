using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using webApi.Models; 

namespace webApi.Context.Mappings 
{
    // A classe de mapa implementa IEntityTypeConfiguration para a entidade que ela mapeia
    public class DonationMap : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.ToTable("Donations"); // Nome da tabela no banco
            
            builder.HasKey(x => x.Id); // Chave primária (assumindo que vem de ModelBase)

            builder.Property(d => d.UserId)
                .HasColumnName("UserID");

            builder.Property(d => d.InstitutionId)
                .HasColumnName("InstitutionID")
                .IsRequired();

            builder.Property(d => d.DonationValue)
                .HasColumnName("DonationValue")
                .HasColumnType("decimal(18,2)") // Usar decimal para valores monetários
                .IsRequired();
            
            builder.Property(d => d.DonationDate)
                .HasColumnName("DonationDate")
                .HasColumnType("datetime") // Use datetime se precisar de hora/minuto/segundo
                .IsRequired();
            
            builder.Property(d => d.Description)
                .HasColumnName("Description")
                .HasMaxLength(200)
                .IsRequired(false); // Defina como false se a descrição for opcional

            builder.Property(d => d.PaymentMethod)
                .HasMaxLength(50)
                .IsRequired(false);
            
            builder.Property(d => d.Status)
                .HasMaxLength(50)
                .IsRequired();

            // Relacionamentos com outras tabelas
            builder.HasOne(d => d.Institution)
                .WithMany() // Se Institution não tem uma coleção de Donations, deixe vazio
                .HasForeignKey(d => d.InstitutionId)
                .OnDelete(DeleteBehavior.Restrict); // Usar Restrict é mais seguro que Cascade

            builder.HasOne(d => d.User)
                .WithMany() // Se User não tem uma coleção de Donations, deixe vazio
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}