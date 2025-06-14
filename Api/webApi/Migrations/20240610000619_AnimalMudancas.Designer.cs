﻿// <auto-generated />
using System;
using CarrocinhaDoBem.Api.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using webApi.Context;

#nullable disable

namespace webApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240610000619_AnimalMudancas")]
    partial class AnimalMudancas
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("webApi.Models.Adoption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("AdoptionDate")
                        .HasColumnType("DATE")
                        .HasColumnName("adoptionDate");

                    b.Property<bool>("AdoptionStatus")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("adoptionStatus");

                    b.Property<int>("AnimalId")
                        .HasColumnType("int")
                        .HasColumnName("animalId");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userId");

                    b.HasKey("Id")
                        .HasName("adoptionID");

                    b.HasIndex("AnimalId");

                    b.HasIndex("UserId");

                    b.ToTable("Adoption", (string)null);
                });

            modelBuilder.Entity("webApi.Models.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("AnimalPic")
                        .HasColumnType("LONGBLOB")
                        .HasColumnName("AnimalPic");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("DATE")
                        .HasColumnName("BirthDate");

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnName("Breed");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("Description");

                    b.Property<int>("InstitutionId")
                        .HasColumnType("int")
                        .HasColumnName("InstitutionID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("AnimalName");

                    b.Property<int>("PetSize")
                        .HasColumnType("INT")
                        .HasColumnName("PetSize");

                    b.Property<DateTime>("RescueDate")
                        .HasColumnType("DATE")
                        .HasColumnName("RescueDate");

                    b.Property<int>("Sex")
                        .HasColumnType("INT")
                        .HasColumnName("Sex");

                    b.Property<int>("Species")
                        .HasMaxLength(50)
                        .HasColumnType("INT")
                        .HasColumnName("Species");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("Status");

                    b.HasKey("Id")
                        .HasName("AnimalID");

                    b.HasIndex("InstitutionId");

                    b.ToTable("Animal", (string)null);
                });

            modelBuilder.Entity("webApi.Models.Donation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("Description");

                    b.Property<DateTime>("DonationDate")
                        .HasColumnType("date")
                        .HasColumnName("DonationDate");

                    b.Property<double>("DonationValue")
                        .HasColumnType("double")
                        .HasColumnName("DonationValue");

                    b.Property<int>("InstitutionId")
                        .HasColumnType("int")
                        .HasColumnName("InstitutionID");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("Id")
                        .HasName("DonationID");

                    b.HasIndex("InstitutionId");

                    b.HasIndex("UserId");

                    b.ToTable("Donation", (string)null);
                });

            modelBuilder.Entity("webApi.Models.Institution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("InstitutionCNPJ")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("varchar(14)")
                        .HasColumnName("institutionCNPJ");

                    b.Property<string>("InstitutionName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("institutionName");

                    b.HasKey("Id")
                        .HasName("institutionID");

                    b.ToTable("Institution", (string)null);
                });

            modelBuilder.Entity("webApi.Models.Sponsorship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AnimalId")
                        .HasColumnType("int")
                        .HasColumnName("animalId");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("DATE")
                        .HasColumnName("endDate");

                    b.Property<DateTime>("InitialDate")
                        .HasColumnType("DATE")
                        .HasColumnName("initialDate");

                    b.Property<string>("SponsorshipType")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("sponsorshipType");

                    b.Property<double>("SponsorshipValue")
                        .HasColumnType("double")
                        .HasColumnName("sponsorshipValue");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userId");

                    b.HasKey("Id")
                        .HasName("sponsorshipID");

                    b.HasIndex("AnimalId");

                    b.HasIndex("UserId");

                    b.ToTable("Sponsorship", (string)null);
                });

            modelBuilder.Entity("webApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("address");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("LONGBLOB")
                        .HasColumnName("avatar");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("DATE")
                        .HasColumnName("birthDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .HasMaxLength(14)
                        .HasColumnType("varchar(14)")
                        .HasColumnName("phone");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("userName");

                    b.Property<string>("UserType")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("userType");

                    b.HasKey("Id")
                        .HasName("userID");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("webApi.Models.Adoption", b =>
                {
                    b.HasOne("webApi.Models.Animal", "Animal")
                        .WithMany()
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animal");

                    b.Navigation("User");
                });

            modelBuilder.Entity("webApi.Models.Animal", b =>
                {
                    b.HasOne("webApi.Models.Institution", "Institution")
                        .WithMany()
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Institution");
                });

            modelBuilder.Entity("webApi.Models.Donation", b =>
                {
                    b.HasOne("webApi.Models.Institution", "Institution")
                        .WithMany()
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Institution");

                    b.Navigation("User");
                });

            modelBuilder.Entity("webApi.Models.Sponsorship", b =>
                {
                    b.HasOne("webApi.Models.Animal", "Animal")
                        .WithMany()
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("webApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animal");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
