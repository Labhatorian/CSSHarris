﻿// <auto-generated />
using CijferRegistratie.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CijferRegistratie.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230329162338_InitialDB")]
    partial class InitialDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CijferRegistratie.Models.Vak", b =>
                {
                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("EC")
                        .HasColumnType("int");

                    b.HasKey("Naam");

                    b.ToTable("Vakken");

                    b.HasData(
                        new
                        {
                            Naam = "Server",
                            EC = 4
                        },
                        new
                        {
                            Naam = "C#",
                            EC = 4
                        },
                        new
                        {
                            Naam = "Databases",
                            EC = 3
                        },
                        new
                        {
                            Naam = "UML",
                            EC = 3
                        },
                        new
                        {
                            Naam = "KBS",
                            EC = 9
                        });
                });
#pragma warning restore 612, 618
        }
    }
}