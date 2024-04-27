﻿// <auto-generated />
using System;
using AutoBackend.Sdk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Sample.Migrations.SqlServer
{
    [DbContext(typeof(SqlServerGenericDbContext))]
    partial class SqlServerGenericDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sample.Data.Budget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<long?>("OwnerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("Name", "OwnerId");

                    b.ToTable("Budget", "generic");
                });

            modelBuilder.Entity("Sample.Data.Participating", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("BudgetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "BudgetId");

                    b.HasIndex("BudgetId");

                    b.ToTable("Participating", "generic");
                });

            modelBuilder.Entity("Sample.Data.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(20, 4)
                        .HasColumnType("decimal(20,4)");

                    b.Property<Guid>("BudgetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("DateTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecretKey")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BudgetId");

                    b.HasIndex("UserId");

                    b.ToTable("Transaction", "generic");
                });

            modelBuilder.Entity("Sample.Data.TransactionVersion", b =>
                {
                    b.Property<int>("__Generic__Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("__Generic__Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(20, 4)
                        .HasColumnType("decimal(20,4)");

                    b.Property<Guid>("BudgetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("DateTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("OriginalTransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("VersionDateTimeUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("__Generic__Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("TransactionVersion", "generic");
                });

            modelBuilder.Entity("Sample.Data.User", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("ActiveBudgetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan?>("TimeZone")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("ActiveBudgetId");

                    b.ToTable("User", "generic");
                });

            modelBuilder.Entity("Sample.Data.Budget", b =>
                {
                    b.HasOne("Sample.Data.User", "Owner")
                        .WithMany("OwnedBudgets")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Sample.Data.Participating", b =>
                {
                    b.HasOne("Sample.Data.Budget", "Budget")
                        .WithMany("Participating")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sample.Data.User", "User")
                        .WithMany("Participating")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Budget");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sample.Data.Transaction", b =>
                {
                    b.HasOne("Sample.Data.Budget", "Budget")
                        .WithMany("Transactions")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sample.Data.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Budget");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sample.Data.TransactionVersion", b =>
                {
                    b.HasOne("Sample.Data.Transaction", "Transaction")
                        .WithMany("Versions")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("Sample.Data.User", b =>
                {
                    b.HasOne("Sample.Data.Budget", "ActiveBudget")
                        .WithMany("ActiveUsers")
                        .HasForeignKey("ActiveBudgetId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("ActiveBudget");
                });

            modelBuilder.Entity("Sample.Data.Budget", b =>
                {
                    b.Navigation("ActiveUsers");

                    b.Navigation("Participating");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Sample.Data.Transaction", b =>
                {
                    b.Navigation("Versions");
                });

            modelBuilder.Entity("Sample.Data.User", b =>
                {
                    b.Navigation("OwnedBudgets");

                    b.Navigation("Participating");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
