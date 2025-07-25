﻿// <auto-generated />
using System;
using DebtsCompass.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DebtsCompass.DataAccess.Migrations
{
    [DbContext(typeof(DebtsCompassDbContext))]
    [Migration("20240317190152_AddNewCategories")]
    partial class AddNewCategories
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.Debt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("BorrowReason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBorrowing")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeadlineDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("EurExchangeRate")
                        .HasColumnType("decimal(18,4)");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal?>("UsdExchangeRate")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.ToTable("Debts");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.DebtAssignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatorUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("DebtId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("NonUserDebtAssignmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SelectedUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorUserId");

                    b.HasIndex("DebtId");

                    b.HasIndex("NonUserDebtAssignmentId");

                    b.HasIndex("SelectedUserId");

                    b.ToTable("DebtAssignments");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("EurExchangeRate")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("UsdExchangeRate")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.ExpenseCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ExpenseCategories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("2af4b793-e624-4061-abc6-986f0c733268"),
                            Name = "Food"
                        },
                        new
                        {
                            Id = new Guid("1261d5b0-be1b-45ad-9167-79cbcf3f0a8b"),
                            Name = "Clothes"
                        },
                        new
                        {
                            Id = new Guid("5547d0a9-2bb8-4185-8777-b2c945a1c471"),
                            Name = "Invoices"
                        },
                        new
                        {
                            Id = new Guid("27d0153c-a7c0-4479-9953-7f69a62e6653"),
                            Name = "Rent"
                        },
                        new
                        {
                            Id = new Guid("ff668d2f-4433-4459-afd7-c52b18ba4639"),
                            Name = "Car"
                        },
                        new
                        {
                            Id = new Guid("ea40f9a8-ef13-40d4-be5e-246a2dc6e5aa"),
                            Name = "Debts"
                        });
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.Friendship", b =>
                {
                    b.Property<string>("RequesterUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SelectedUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("RequesterUserId", "SelectedUserId");

                    b.HasIndex("SelectedUserId");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.Income", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("EurExchangeRate")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("UsdExchangeRate")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Incomes");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.IncomeCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("IncomeCategories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("bcd5c257-d3e1-412f-98cc-b7edeb5b92ee"),
                            Name = "Salary"
                        },
                        new
                        {
                            Id = new Guid("b4c55cf5-499b-4a3c-be10-fc10cc303008"),
                            Name = "Savings"
                        },
                        new
                        {
                            Id = new Guid("43d27aae-2fb4-479d-994a-e32c061de230"),
                            Name = "Debts"
                        });
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.NonUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PersonEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonFirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonLastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NonUsers");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrencyPreference")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpireTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserInfoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("UserInfoId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.UserInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.DebtAssignment", b =>
                {
                    b.HasOne("DebtsCompass.Domain.Entities.Models.User", "CreatorUser")
                        .WithMany("CreatedDebts")
                        .HasForeignKey("CreatorUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DebtsCompass.Domain.Entities.Models.Debt", "Debt")
                        .WithMany("DebtAssignments")
                        .HasForeignKey("DebtId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DebtsCompass.Domain.Entities.Models.NonUser", "NonUser")
                        .WithMany()
                        .HasForeignKey("NonUserDebtAssignmentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("DebtsCompass.Domain.Entities.Models.User", "SelectedUser")
                        .WithMany("DebtsAssigned")
                        .HasForeignKey("SelectedUserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("CreatorUser");

                    b.Navigation("Debt");

                    b.Navigation("NonUser");

                    b.Navigation("SelectedUser");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.Expense", b =>
                {
                    b.HasOne("DebtsCompass.Domain.Entities.Models.ExpenseCategory", "Category")
                        .WithMany("Expenses")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("DebtsCompass.Domain.Entities.Models.User", "User")
                        .WithMany("Expenses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.ExpenseCategory", b =>
                {
                    b.HasOne("DebtsCompass.Domain.Entities.Models.User", "User")
                        .WithMany("ExpenseCategories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.Friendship", b =>
                {
                    b.HasOne("DebtsCompass.Domain.Entities.Models.User", "RequesterUser")
                        .WithMany()
                        .HasForeignKey("RequesterUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DebtsCompass.Domain.Entities.Models.User", "SelectedUser")
                        .WithMany()
                        .HasForeignKey("SelectedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("RequesterUser");

                    b.Navigation("SelectedUser");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.Income", b =>
                {
                    b.HasOne("DebtsCompass.Domain.Entities.Models.IncomeCategory", "Category")
                        .WithMany("Incomes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("DebtsCompass.Domain.Entities.Models.User", "User")
                        .WithMany("Incomes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.IncomeCategory", b =>
                {
                    b.HasOne("DebtsCompass.Domain.Entities.Models.User", "User")
                        .WithMany("IncomeCategories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.User", b =>
                {
                    b.HasOne("DebtsCompass.Domain.Entities.Models.UserInfo", "UserInfo")
                        .WithMany()
                        .HasForeignKey("UserInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.UserInfo", b =>
                {
                    b.HasOne("DebtsCompass.Domain.Entities.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.Debt", b =>
                {
                    b.Navigation("DebtAssignments");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.ExpenseCategory", b =>
                {
                    b.Navigation("Expenses");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.IncomeCategory", b =>
                {
                    b.Navigation("Incomes");
                });

            modelBuilder.Entity("DebtsCompass.Domain.Entities.Models.User", b =>
                {
                    b.Navigation("CreatedDebts");

                    b.Navigation("DebtsAssigned");

                    b.Navigation("ExpenseCategories");

                    b.Navigation("Expenses");

                    b.Navigation("IncomeCategories");

                    b.Navigation("Incomes");
                });
#pragma warning restore 612, 618
        }
    }
}
