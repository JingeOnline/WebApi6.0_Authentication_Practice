using DbServiceLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace DbServiceLib
{
    public class StudentManagementDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public string DbPath { get; }


        public StudentManagementDbContext()
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            //DbPath = System.IO.Path.Join(path, "blogging.db");

            //配置数据库的存储路径
            var folder = Environment.CurrentDirectory;
            var parentFolder = Directory.GetParent(folder);
            DbPath = Path.Join(parentFolder.FullName, "StudentManagement.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Student>().ToTable("tb_Student").HasKey(x=>x.PkId);
            builder.Entity<Subject>().ToTable("tb_Subject").HasKey(x=>x.PkId);
            builder.Entity<Student>().HasMany(s => s.Subjects).WithMany(t => t.Students).UsingEntity(j=>j.ToTable("tb_Student_Subject"));
        }
    }
}