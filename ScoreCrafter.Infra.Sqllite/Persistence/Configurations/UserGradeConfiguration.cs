using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ScoreCrafter.Domain.Entities;

namespace ScoreCrafter.Infra.Sqllite.Persistenc.Configurations;

public sealed class UserGradeConfiguration: IEntityTypeConfiguration<UserGrade>
{
    public void Configure(EntityTypeBuilder<UserGrade> builder)
    {
        builder.ToTable("UserGrades");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.IsCurrent)
            .IsRequired();

        builder.Property(x => x.CreatedTime)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Grade)
            .WithMany()
            .HasForeignKey("GradeId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex("UserId");

        builder.HasIndex("GradeId");

        builder.HasIndex("UserId", nameof(UserGrade.IsCurrent));
    }
}