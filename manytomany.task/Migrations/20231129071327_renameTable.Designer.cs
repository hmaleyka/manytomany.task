﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using manytomany.task.DAL;

#nullable disable

namespace manytomany.task.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231129071327_renameTable")]
    partial class renameTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("manytomany.task.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("manytomany.task.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("SKU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("products");
                });

            modelBuilder.Entity("manytomany.task.Models.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPrime")
                        .HasColumnType("bit");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("productsImage");
                });

            modelBuilder.Entity("manytomany.task.Models.ProductTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("TagId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("TagId");

                    b.ToTable("productTag");
                });

            modelBuilder.Entity("manytomany.task.Models.Slider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgUrl")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SubTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.ToTable("sliders");
                });

            modelBuilder.Entity("manytomany.task.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tag");
                });

            modelBuilder.Entity("manytomany.task.Models.Product", b =>
                {
                    b.HasOne("manytomany.task.Models.Category", "category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.Navigation("category");
                });

            modelBuilder.Entity("manytomany.task.Models.ProductImage", b =>
                {
                    b.HasOne("manytomany.task.Models.Product", "product")
                        .WithMany("productImages")
                        .HasForeignKey("ProductId");

                    b.Navigation("product");
                });

            modelBuilder.Entity("manytomany.task.Models.ProductTag", b =>
                {
                    b.HasOne("manytomany.task.Models.Product", "product")
                        .WithMany("productTags")
                        .HasForeignKey("ProductId");

                    b.HasOne("manytomany.task.Models.Tag", "tag")
                        .WithMany("productTags")
                        .HasForeignKey("TagId");

                    b.Navigation("product");

                    b.Navigation("tag");
                });

            modelBuilder.Entity("manytomany.task.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("manytomany.task.Models.Product", b =>
                {
                    b.Navigation("productImages");

                    b.Navigation("productTags");
                });

            modelBuilder.Entity("manytomany.task.Models.Tag", b =>
                {
                    b.Navigation("productTags");
                });
#pragma warning restore 612, 618
        }
    }
}
