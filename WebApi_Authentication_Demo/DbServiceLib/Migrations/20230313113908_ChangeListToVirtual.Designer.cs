﻿// <auto-generated />
using DbServiceLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DbServiceLib.Migrations
{
    [DbContext(typeof(StudentManagementDbContext))]
    [Migration("20230313113908_ChangeListToVirtual")]
    partial class ChangeListToVirtual
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("DbServiceLib.Models.Student", b =>
                {
                    b.Property<int>("PkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PkId");

                    b.ToTable("tb_Student", (string)null);
                });

            modelBuilder.Entity("DbServiceLib.Models.Subject", b =>
                {
                    b.Property<int>("PkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PkId");

                    b.ToTable("tb_Subject", (string)null);
                });

            modelBuilder.Entity("StudentSubject", b =>
                {
                    b.Property<int>("StudentsPkId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SubjectsPkId")
                        .HasColumnType("INTEGER");

                    b.HasKey("StudentsPkId", "SubjectsPkId");

                    b.HasIndex("SubjectsPkId");

                    b.ToTable("tb_Student_Subject", (string)null);
                });

            modelBuilder.Entity("StudentSubject", b =>
                {
                    b.HasOne("DbServiceLib.Models.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsPkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DbServiceLib.Models.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectsPkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}