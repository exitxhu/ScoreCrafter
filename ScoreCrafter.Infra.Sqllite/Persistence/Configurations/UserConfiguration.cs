using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ScoreCrafter.Domain.Entities;

namespace ScoreCrafter.Infra.Sqllite.Persistenc.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.CreatedTime)
            .IsRequired();

        builder.HasIndex(x => x.Id)
            .IsUnique();
    }
}
public sealed class UserScoreConfiguration
    : IEntityTypeConfiguration<UserScore>
{
    public void Configure(EntityTypeBuilder<UserScore> builder)
    {
        builder.ToTable("UserScores");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Score)
            .HasPrecision(18, 6)
            .IsRequired();

        builder.Property(x => x.FormulaVersion)
            .IsRequired();

        builder.Property(x => x.CalculatedAt)
            .IsRequired();

        builder.Property(x => x.IsCurrent)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Formula)
            .WithMany()
            .HasForeignKey(x => x.FormulaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(
            x => new
            {
                x.UserId,
                x.IsCurrent
            });

        builder.HasIndex(
            x => new
            {
                x.FormulaId,
                x.FormulaVersion
            });
    }
}