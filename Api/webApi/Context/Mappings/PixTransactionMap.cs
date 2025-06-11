using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using webApi.Models;

namespace webApi.Context.Mappings
{
    public class PixTransactionMap : IEntityTypeConfiguration<PixTransaction>
    {
        public void Configure(EntityTypeBuilder<PixTransaction> builder)
        {
            builder.ToTable("PixTransactions");

            builder.HasKey(pt => pt.Id);

            // Relacionamento com Donation (agora com tipos compatíveis: int -> int)
            builder.HasOne(pt => pt.Donation)
                .WithMany(d => d.PixTransactions)
                .HasForeignKey(pt => pt.DonationId) // Esta propriedade agora é 'int'
                .OnDelete(DeleteBehavior.Restrict); // Usar Restrict previne exclusão em cascata acidental

            // --- Configuração das outras propriedades ---
            builder.Property(pt => pt.TransactionId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pt => pt.EndToEndId)
                .HasMaxLength(100);

            builder.Property(pt => pt.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(pt => pt.CreationDate)
                .IsRequired();

            builder.Property(pt => pt.Status)
                .IsRequired()
                .HasMaxLength(50);

            // Usar um tipo de dados que suporte textos longos
            builder.Property(pt => pt.QrCode).HasColumnType("TEXT");
            builder.Property(pt => pt.CopiaECola).HasColumnType("TEXT");
            builder.Property(pt => pt.PayerInfo).HasColumnType("TEXT");
            builder.Property(pt => pt.ErrorMessage).HasColumnType("TEXT");
        }
    }
}