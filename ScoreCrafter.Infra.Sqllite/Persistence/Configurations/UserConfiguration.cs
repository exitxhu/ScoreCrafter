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

        builder.HasMany(a => a.UserGradeHistory).WithOne(a => a.User);
        builder.HasOne(a => a.CurrentUserGrade);
    }
}
