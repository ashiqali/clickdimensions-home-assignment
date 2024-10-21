﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SocialSyncPortal.DAL.DataContext;

#nullable disable

namespace SocialSyncPortal.DAL.Migrations
{
    [DbContext(typeof(SocialSyncPortalDbContext))]
    partial class SocialSyncPortalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("SocialSyncPortal.DAL.Entities.SocialPost", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("TEXT");                    

                    b.Property<int>("Popularity")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SocialPosts");
                });

            modelBuilder.Entity("SocialSyncPortal.DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(2024, 10, 18, 18, 32, 42, 953, DateTimeKind.Utc).AddTicks(270),
                            ModifiedDate = new DateTime(2024, 10, 18, 18, 32, 42, 953, DateTimeKind.Utc).AddTicks(271),
                            Name = "Admin",
                            Password = "123",
                            Surname = "Admin",
                            Username = "admin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
