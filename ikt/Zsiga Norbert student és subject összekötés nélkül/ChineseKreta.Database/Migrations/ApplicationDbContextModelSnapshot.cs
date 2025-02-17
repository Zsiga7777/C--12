﻿// <auto-generated />
using System;
using ChineseKreta.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChineseKreta.Database.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChineseKreta.Database.Entities.AddressEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<long>("StreetId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StreetId");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.CityEntity", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<long>("CountryId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("City");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.CountryEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Country");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.MarkEntity", b =>
                {
                    b.Property<long>("MarkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("MarkId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<long>("Mark")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("StudentId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<long>("SubjectId")
                        .HasColumnType("bigint");

                    b.HasKey("MarkId");

                    b.HasIndex("StudentId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Mark");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.StreetEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CityId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Street");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.StudentEntity", b =>
                {
                    b.Property<decimal>("EducationalID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<long?>("AddressId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("MothersName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("EducationalID");

                    b.HasIndex("AddressId");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.SubjectEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.AddressEntity", b =>
                {
                    b.HasOne("ChineseKreta.Database.Entities.StreetEntity", "Street")
                        .WithMany("Addresses")
                        .HasForeignKey("StreetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Street");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.CityEntity", b =>
                {
                    b.HasOne("ChineseKreta.Database.Entities.CountryEntity", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.MarkEntity", b =>
                {
                    b.HasOne("ChineseKreta.Database.Entities.StudentEntity", "Student")
                        .WithMany("Marks")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("ChineseKreta.Database.Entities.SubjectEntity", "Subject")
                        .WithMany("Marks")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.StreetEntity", b =>
                {
                    b.HasOne("ChineseKreta.Database.Entities.CityEntity", "City")
                        .WithMany("Streets")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.StudentEntity", b =>
                {
                    b.HasOne("ChineseKreta.Database.Entities.AddressEntity", "Address")
                        .WithMany("Students")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Address");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.AddressEntity", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.CityEntity", b =>
                {
                    b.Navigation("Streets");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.CountryEntity", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.StreetEntity", b =>
                {
                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.StudentEntity", b =>
                {
                    b.Navigation("Marks");
                });

            modelBuilder.Entity("ChineseKreta.Database.Entities.SubjectEntity", b =>
                {
                    b.Navigation("Marks");
                });
#pragma warning restore 612, 618
        }
    }
}
