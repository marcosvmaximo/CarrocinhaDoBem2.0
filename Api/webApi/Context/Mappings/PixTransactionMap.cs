using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using webApi.Models;

namespace CarrocinhaDoBem.Api.Context.Mappings
{
    public class PixTransactionMap : IEntityTypeConfiguration<PixTransaction>
    {
        public void Configure(EntityTypeBuilder<PixTransaction> builder)
        {
            builder.ToTable("PixTransactions"); // Nome da tabela no banco de dados

            builder.HasKey(pt => pt.Id); // Chave primária (herdada de ModelBase ou definida aqui)

            builder.Property(pt => pt.TransactionId)
                .IsRequired()
                .HasMaxLength(100); // Ajuste o tamanho conforme necessário

            builder.Property(pt => pt.EndToEndId)
                .HasMaxLength(100); // Ajuste o tamanho conforme necessário

            builder.Property(pt => pt.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(pt => pt.CreationDate)
                .IsRequired();

            builder.Property(pt => pt.Status)
                .IsRequired()
                .HasMaxLength(50); // Ajuste o tamanho

            builder.Property(pt => pt.QrCode)
                .HasColumnType("TEXT"); // Ou um tipo apropriado para strings longas

            builder.Property(pt => pt.CopiaECola)
                .HasColumnType("TEXT"); // Ou um tipo apropriado para strings longas

            builder.Property(pt => pt.PayerInfo)
                .HasColumnType("TEXT");

            builder.Property(pt => pt.ErrorMessage)
                .HasColumnType("TEXT");

            // Relacionamento com Donation
            builder.HasOne(pt => pt.Donation)
                .WithMany(d => d.PixTransactions) // Adicionar esta propriedade de navegação em Donation.cs
                .HasForeignKey(pt => pt.DonationId)
                .OnDelete(DeleteBehavior.Restrict); // Ou Cascade, dependendo da sua regra de negócio
        }
    }
}