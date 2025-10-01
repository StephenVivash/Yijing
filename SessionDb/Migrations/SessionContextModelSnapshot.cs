using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SessionDb.Data;

#nullable disable

namespace SessionDb.Migrations;

[DbContext(typeof(SessionContext))]
partial class SessionContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder.HasAnnotation("ProductVersion", "9.0.9");

        modelBuilder.Entity("SessionDb.Models.SessionEntry", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER");

            b.Property<string>("Description")
                .HasMaxLength(100)
                .HasColumnType("TEXT");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("TEXT");

            b.Property<string>("YijingCast")
                .HasMaxLength(50)
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.ToTable("Sessions");

            b.HasData(
                new
                {
                    Id = 1,
                    Name = "Dawn Study",
                    Description = "Morning meditation focusing on hexagram insight.",
                    YijingCast = "Hexagram 24"
                },
                new
                {
                    Id = 2,
                    Name = "Lunar Notes",
                    Description = "Evening reflections guided by the I Ching.",
                    YijingCast = "Hexagram 15"
                },
                new
                {
                    Id = 3,
                    Name = "Oracle Circle",
                    Description = "Group reading exploring seasonal change.",
                    YijingCast = "Hexagram 49"
                });
        });
#pragma warning restore 612, 618
    }
}
