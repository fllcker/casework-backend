﻿// <auto-generated />
using System;
using CaseWork.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CaseWork.Migrations
{
    [DbContext(typeof(CaseWorkContext))]
    partial class CaseWorkContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CaseWork.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("CaseWork.Models.Invite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedAt")
                        .HasColumnType("integer");

                    b.Property<int>("InitiatorId")
                        .HasColumnType("integer");

                    b.Property<int>("InviteEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("InviteType")
                        .HasColumnType("integer");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDenied")
                        .HasColumnType("boolean");

                    b.Property<string>("LinkHash")
                        .HasColumnType("text");

                    b.Property<int>("TargetId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("InitiatorId");

                    b.HasIndex("TargetId");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("CaseWork.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("CaseWork.Models.RoleRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("RoleRelations");
                });

            modelBuilder.Entity("CaseWork.Models.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AcceptedTime")
                        .HasColumnType("integer");

                    b.Property<string>("Assignment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CompletedTime")
                        .HasColumnType("integer");

                    b.Property<int>("CreatedAt")
                        .HasColumnType("integer");

                    b.Property<int>("DeadLine")
                        .HasColumnType("integer");

                    b.Property<int>("EmployerId")
                        .HasColumnType("integer");

                    b.Property<int>("ExecutorId")
                        .HasColumnType("integer");

                    b.Property<bool?>("IsComplete")
                        .HasColumnType("boolean");

                    b.Property<int?>("SubTaskId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("Urgency")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EmployerId");

                    b.HasIndex("ExecutorId");

                    b.HasIndex("SubTaskId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("CaseWork.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<string>("Country")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<bool?>("Horse")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CaseWork.Models.Invite", b =>
                {
                    b.HasOne("CaseWork.Models.User", "Initiator")
                        .WithMany()
                        .HasForeignKey("InitiatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseWork.Models.User", "Target")
                        .WithMany()
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Initiator");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("CaseWork.Models.RoleRelation", b =>
                {
                    b.HasOne("CaseWork.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseWork.Models.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseWork.Models.Task", b =>
                {
                    b.HasOne("CaseWork.Models.User", "Employer")
                        .WithMany()
                        .HasForeignKey("EmployerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseWork.Models.User", "Executor")
                        .WithMany()
                        .HasForeignKey("ExecutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaseWork.Models.Task", "SubTask")
                        .WithMany()
                        .HasForeignKey("SubTaskId");

                    b.Navigation("Employer");

                    b.Navigation("Executor");

                    b.Navigation("SubTask");
                });

            modelBuilder.Entity("CaseWork.Models.User", b =>
                {
                    b.HasOne("CaseWork.Models.Company", null)
                        .WithMany("Users")
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("CaseWork.Models.Company", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("CaseWork.Models.User", b =>
                {
                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
