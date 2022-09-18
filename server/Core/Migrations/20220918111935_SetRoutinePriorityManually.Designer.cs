﻿// <auto-generated />
using System;
using Konek.Server.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Konek.Server.Core.Migrations
{
    [DbContext(typeof(DeviceContext))]
    [Migration("20220918111935_SetRoutinePriorityManually")]
    partial class SetRoutinePriorityManually
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("GroupLamp", b =>
                {
                    b.Property<int>("GroupsGroupId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LampsLampId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GroupsGroupId", "LampsLampId");

                    b.HasIndex("LampsLampId");

                    b.ToTable("GroupLamp");
                });

            modelBuilder.Entity("Konek.Server.Core.Models.Effect", b =>
                {
                    b.Property<int>("EffectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Brightness")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<int?>("RoutineDefinitionId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<byte>("Temperature")
                        .HasColumnType("INTEGER");

                    b.HasKey("EffectId");

                    b.HasIndex("RoutineDefinitionId");

                    b.ToTable("Effects");
                });

            modelBuilder.Entity("Konek.Server.Core.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("GroupId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Konek.Server.Core.Models.Lamp", b =>
                {
                    b.Property<int>("LampId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RemoteId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LampId");

                    b.ToTable("Lamps");
                });

            modelBuilder.Entity("Konek.Server.Core.Models.Routine", b =>
                {
                    b.Property<int>("RoutineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DefinitionRoutineDefinitionId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Expiry")
                        .HasColumnType("TEXT");

                    b.Property<int?>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LampId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.HasKey("RoutineId");

                    b.HasIndex("DefinitionRoutineDefinitionId");

                    b.HasIndex("GroupId");

                    b.HasIndex("LampId");

                    b.ToTable("Routines");
                });

            modelBuilder.Entity("Konek.Server.Core.Models.RoutineDefinition", b =>
                {
                    b.Property<int>("RoutineDefinitionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("RoutineDefinitionId");

                    b.ToTable("RoutineDefinitions");
                });

            modelBuilder.Entity("GroupLamp", b =>
                {
                    b.HasOne("Konek.Server.Core.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Konek.Server.Core.Models.Lamp", null)
                        .WithMany()
                        .HasForeignKey("LampsLampId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Konek.Server.Core.Models.Effect", b =>
                {
                    b.HasOne("Konek.Server.Core.Models.RoutineDefinition", null)
                        .WithMany("Effects")
                        .HasForeignKey("RoutineDefinitionId");
                });

            modelBuilder.Entity("Konek.Server.Core.Models.Routine", b =>
                {
                    b.HasOne("Konek.Server.Core.Models.RoutineDefinition", "Definition")
                        .WithMany()
                        .HasForeignKey("DefinitionRoutineDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Konek.Server.Core.Models.Group", null)
                        .WithMany("Routines")
                        .HasForeignKey("GroupId");

                    b.HasOne("Konek.Server.Core.Models.Lamp", null)
                        .WithMany("Routines")
                        .HasForeignKey("LampId");

                    b.Navigation("Definition");
                });

            modelBuilder.Entity("Konek.Server.Core.Models.Group", b =>
                {
                    b.Navigation("Routines");
                });

            modelBuilder.Entity("Konek.Server.Core.Models.Lamp", b =>
                {
                    b.Navigation("Routines");
                });

            modelBuilder.Entity("Konek.Server.Core.Models.RoutineDefinition", b =>
                {
                    b.Navigation("Effects");
                });
#pragma warning restore 612, 618
        }
    }
}
