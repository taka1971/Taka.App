﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Taka.App.Deliverer.Infra.Data.Context;

#nullable disable

namespace Taka.App.Rentals.Infra.Data.Migrations
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

            modelBuilder.Entity("Taka.App.Rentals.Domain.Entities.Rental", b =>
                {
                    b.Property<Guid>("RentalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DelivererId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("MotorcycleId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("PredictedEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("RentalPlanId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("RentalId");

                    b.HasIndex("RentalPlanId");

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("Taka.App.Rentals.Domain.Entities.RentalPlan", b =>
                {
                    b.Property<Guid>("RentalPlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("DailyRate")
                        .HasColumnType("numeric");

                    b.Property<long>("DurationDays")
                        .HasColumnType("bigint");

                    b.Property<decimal>("EarlyReturnPenaltyRate")
                        .HasColumnType("numeric");

                    b.Property<string>("RentalDescription")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("character varying(70)");

                    b.HasKey("RentalPlanId");

                    b.ToTable("RentalPlans");
                });

            modelBuilder.Entity("Taka.App.Rentals.Domain.Entities.Rental", b =>
                {
                    b.HasOne("Taka.App.Rentals.Domain.Entities.RentalPlan", "RentalPlan")
                        .WithMany()
                        .HasForeignKey("RentalPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RentalPlan");
                });
#pragma warning restore 612, 618
        }
    }
}
