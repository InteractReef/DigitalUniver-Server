﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Schedules.Microservice.Infrastructure.Database;

#nullable disable

namespace Schedules.Microservice.Migrations
{
    [DbContext(typeof(SchedulesDbContext))]
    partial class SchedulesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InteractReef.Packets.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("InteractReef.Packets.ScheduleItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ScheduleId")
                        .HasColumnType("integer");

                    b.Property<int?>("ScheduleId1")
                        .HasColumnType("integer");

                    b.Property<string>("Subjects")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("ScheduleId1");

                    b.ToTable("ScheduleItems");
                });

            modelBuilder.Entity("InteractReef.Packets.SubjectItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("SubjectItems");
                });

            modelBuilder.Entity("InteractReef.Packets.ScheduleItem", b =>
                {
                    b.HasOne("InteractReef.Packets.Schedule", null)
                        .WithMany("denominatorWeek")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InteractReef.Packets.Schedule", null)
                        .WithMany("numeratorWeek")
                        .HasForeignKey("ScheduleId1");
                });

            modelBuilder.Entity("InteractReef.Packets.Schedule", b =>
                {
                    b.Navigation("denominatorWeek");

                    b.Navigation("numeratorWeek");
                });
#pragma warning restore 612, 618
        }
    }
}
