﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TesteSygnal.Context;

#nullable disable

namespace TesteSygnal.Migrations
{
    [DbContext(typeof(TSDbContext))]
    partial class TSDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TesteSygnal.Models.Order", b =>
                {
                    b.Property<int>("ControlNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ControlNumber"));

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ControlNumber");

                    b.ToTable("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
