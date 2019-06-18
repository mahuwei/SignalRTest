﻿// <auto-generated />
using System;
using Kkd.ShortUrl.Modals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kkd.ShortUrl.Migrations
{
    [DbContext(typeof(ShortUrlContext))]
    partial class ShortUrlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Kkd.ShortUrl.Modals.MaxRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastChange");

                    b.Property<long>("No");

                    b.Property<byte[]>("RowFlag")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("MaxRecords");
                });

            modelBuilder.Entity("Kkd.ShortUrl.Modals.UrlMap", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastChange");

                    b.Property<string>("LongUrl")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Md5")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("RowFlag")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("ShortUrl")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("UrlMaps");
                });

            modelBuilder.Entity("Kkd.ShortUrl.Modals.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("LastChange");

                    b.Property<byte[]>("RowFlag")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("Status");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
