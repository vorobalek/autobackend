﻿// <auto-generated />
using System;
using AutoBackend.Sdk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations.Postgres
{
    [DbContext(typeof(PostgresAutoBackendDbContext))]
    partial class PostgresAutoBackendDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Api.Data.Album", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Artist")
                        .HasColumnType("text");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Album", "generic");
                });

            modelBuilder.Entity("Api.Data.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Author")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Book", "generic");
                });

            modelBuilder.Entity("Api.Data.Book2Albums", b =>
                {
                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AlbumId")
                        .HasColumnType("uuid");

                    b.HasKey("BookId", "AlbumId");

                    b.HasIndex("AlbumId");

                    b.ToTable("Book2Albums", "generic");
                });

            modelBuilder.Entity("Api.Data.BookShelve", b =>
                {
                    b.Property<Guid>("Book1Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Book2Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Book3Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Book4Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Book5Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Book6Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Book7Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Book8Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Book1Id", "Book2Id", "Book3Id", "Book4Id", "Book5Id", "Book6Id", "Book7Id", "Book8Id");

                    b.HasIndex("Book2Id");

                    b.HasIndex("Book3Id");

                    b.HasIndex("Book4Id");

                    b.HasIndex("Book5Id");

                    b.HasIndex("Book6Id");

                    b.HasIndex("Book7Id");

                    b.HasIndex("Book8Id");

                    b.ToTable("BookShelve", "generic");
                });

            modelBuilder.Entity("Api.Data.Note", b =>
                {
                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("Note", "generic");
                });

            modelBuilder.Entity("Api.Data.Book2Albums", b =>
                {
                    b.HasOne("Api.Data.Album", "Album")
                        .WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Album");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Api.Data.BookShelve", b =>
                {
                    b.HasOne("Api.Data.Book", "Book1")
                        .WithMany()
                        .HasForeignKey("Book1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Book", "Book2")
                        .WithMany()
                        .HasForeignKey("Book2Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Book", "Book3")
                        .WithMany()
                        .HasForeignKey("Book3Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Book", "Book4")
                        .WithMany()
                        .HasForeignKey("Book4Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Book", "Book5")
                        .WithMany()
                        .HasForeignKey("Book5Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Book", "Book6")
                        .WithMany()
                        .HasForeignKey("Book6Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Book", "Book7")
                        .WithMany()
                        .HasForeignKey("Book7Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Data.Book", "Book8")
                        .WithMany()
                        .HasForeignKey("Book8Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book1");

                    b.Navigation("Book2");

                    b.Navigation("Book3");

                    b.Navigation("Book4");

                    b.Navigation("Book5");

                    b.Navigation("Book6");

                    b.Navigation("Book7");

                    b.Navigation("Book8");
                });
#pragma warning restore 612, 618
        }
    }
}
