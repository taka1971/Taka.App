﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Taka.App.Deliverer.Infra.Data.Context;

#nullable disable

namespace Taka.App.Deliverer.Infra.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Taka.App.Deliverer.Domain.Entities.PersonDelivery", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("CNHImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CNHNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CNHType")
                        .HasColumnType("integer");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("character varying(14)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CNHNumber")
                        .IsUnique();

                    b.HasIndex("CNPJ")
                        .IsUnique();

                    b.ToTable("Deliverers");
                });
#pragma warning restore 612, 618
        }
    }
}
