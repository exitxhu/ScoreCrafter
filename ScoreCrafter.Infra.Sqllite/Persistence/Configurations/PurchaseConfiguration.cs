using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ScoreCrafter.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ScoreCrafter.Infra.Sqllite.Persistenc.Configurations;

public sealed class PurchaseConfiguration: IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable("Purchases");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.PurchaseDate)
            .IsRequired();

        builder.Property(x => x.Metadata)
            .HasConversion(
                value => JsonSerializer.Serialize(
                    value,
                    (JsonSerializerOptions?)null),

                value => JsonSerializer.Deserialize<
                    Dictionary<string, string>>(
                        value,
                        (JsonSerializerOptions?)null)
                    ?? new Dictionary<string, string>());

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.PurchaseDate);
    }
}