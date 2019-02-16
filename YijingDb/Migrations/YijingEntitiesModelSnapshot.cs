﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YijingDb;

namespace YijingDb.Migrations
{
    [DbContext(typeof(YijingEntities))]
    partial class YijingEntitiesModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("YijingDb.Label", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LabelData")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("LabelSeryId");

                    b.Property<int>("TypeId");

                    b.Property<int>("ValueId");

                    b.HasKey("Id");

                    b.HasIndex("LabelSeryId");

                    b.HasIndex("TypeId", "ValueId");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("YijingDb.LabelSery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("LabelSeries");
                });

            modelBuilder.Entity("YijingDb.Ratio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RatioData");

                    b.Property<int>("RatioSeryId");

                    b.Property<int>("TypeId");

                    b.Property<int>("ValueId");

                    b.HasKey("Id");

                    b.HasIndex("RatioSeryId");

                    b.HasIndex("TypeId", "ValueId");

                    b.ToTable("Ratios");
                });

            modelBuilder.Entity("YijingDb.RatioSery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("RatioSeries");
                });

            modelBuilder.Entity("YijingDb.Sequence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("SequenceData");

                    b.Property<int>("SequenceSeryId");

                    b.Property<int>("TypeId");

                    b.Property<int>("ValueId");

                    b.HasKey("Id");

                    b.HasIndex("SequenceSeryId");

                    b.HasIndex("TypeId", "ValueId");

                    b.ToTable("Sequences");
                });

            modelBuilder.Entity("YijingDb.SequenceSery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("SequenceSeries");
                });

            modelBuilder.Entity("YijingDb.Text", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TextData")
                        .IsRequired();

                    b.Property<int>("TextSeryId");

                    b.Property<int>("TextTypeId");

                    b.Property<int>("TypeId");

                    b.Property<int>("ValueId");

                    b.HasKey("Id");

                    b.HasIndex("TextSeryId");

                    b.HasIndex("TextTypeId");

                    b.HasIndex("TypeId", "ValueId");

                    b.ToTable("Texts");
                });

            modelBuilder.Entity("YijingDb.TextSery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("TextSeries");
                });

            modelBuilder.Entity("YijingDb.TextType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("TextTypes");
                });

            modelBuilder.Entity("YijingDb.Type", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("Size");

                    b.HasKey("Id");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("YijingDb.Value", b =>
                {
                    b.Property<int>("TypeId");

                    b.Property<int>("ValueId");

                    b.HasKey("TypeId", "ValueId");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("YijingDb.Label", b =>
                {
                    b.HasOne("YijingDb.LabelSery", "LabelSery")
                        .WithMany("Labels")
                        .HasForeignKey("LabelSeryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YijingDb.Type", "Type")
                        .WithMany("Labels")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("YijingDb.Value", "Value")
                        .WithMany("Labels")
                        .HasForeignKey("TypeId", "ValueId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("YijingDb.LabelSery", b =>
                {
                    b.HasOne("YijingDb.Type", "Type")
                        .WithMany("LabelSeries")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YijingDb.Ratio", b =>
                {
                    b.HasOne("YijingDb.RatioSery", "RatioSery")
                        .WithMany("Ratios")
                        .HasForeignKey("RatioSeryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YijingDb.Type", "Type")
                        .WithMany("Ratios")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("YijingDb.Value", "Value")
                        .WithMany("Ratios")
                        .HasForeignKey("TypeId", "ValueId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("YijingDb.RatioSery", b =>
                {
                    b.HasOne("YijingDb.Type", "Type")
                        .WithMany("RatioSeries")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YijingDb.Sequence", b =>
                {
                    b.HasOne("YijingDb.SequenceSery", "SequenceSery")
                        .WithMany("Sequences")
                        .HasForeignKey("SequenceSeryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YijingDb.Type", "Type")
                        .WithMany("Sequences")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("YijingDb.Value", "Value")
                        .WithMany("Sequences")
                        .HasForeignKey("TypeId", "ValueId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("YijingDb.SequenceSery", b =>
                {
                    b.HasOne("YijingDb.Type", "Type")
                        .WithMany("SequenceSeries")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YijingDb.Text", b =>
                {
                    b.HasOne("YijingDb.TextSery", "TextSery")
                        .WithMany("Texts")
                        .HasForeignKey("TextSeryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YijingDb.TextType", "TextType")
                        .WithMany("Texts")
                        .HasForeignKey("TextTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YijingDb.Type", "Type")
                        .WithMany("Texts")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("YijingDb.Value", "Value")
                        .WithMany("Texts")
                        .HasForeignKey("TypeId", "ValueId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("YijingDb.TextSery", b =>
                {
                    b.HasOne("YijingDb.Type", "Type")
                        .WithMany("TextSeries")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YijingDb.Value", b =>
                {
                    b.HasOne("YijingDb.Type", "Type")
                        .WithMany("Values")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
