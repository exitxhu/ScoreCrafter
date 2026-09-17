using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ScoreCrafter.Domain.Entities;

namespace ScoreCrafter.Infra.Sqllite.Persistenc.Configurations;

public sealed class FormulaConfiguration: IEntityTypeConfiguration<Formula>
{
    public void Configure(EntityTypeBuilder<Formula> builder)
    {
        builder.ToTable("Formulas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Definition)
            .HasMaxLength(10000)
            .IsRequired();

        builder.Property(x => x.Version)
            .IsRequired();

    }
}