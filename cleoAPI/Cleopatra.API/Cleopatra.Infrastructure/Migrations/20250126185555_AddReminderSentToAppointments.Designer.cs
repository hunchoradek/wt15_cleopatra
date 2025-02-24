﻿// <auto-generated />
using System;
using Cleopatra.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Cleopatra.Infrastructure.Migrations
{
    [DbContext(typeof(SalonContext))]
    [Migration("20250126185555_AddReminderSentToAppointments")]
    partial class AddReminderSentToAppointments
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Cleopatra.Domain.Appointment", b =>
                {
                    b.Property<int>("appointment_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("appointment_id"));

                    b.Property<bool>("ReminderSent")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("appointment_date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("client_id")
                        .HasColumnType("int");

                    b.Property<int>("employee_id")
                        .HasColumnType("int");

                    b.Property<string>("employee_name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<TimeSpan>("end_time")
                        .HasColumnType("time(6)");

                    b.Property<string>("notes")
                        .HasColumnType("longtext");

                    b.Property<string>("service")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<TimeSpan>("start_time")
                        .HasColumnType("time(6)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("appointment_id");

                    b.HasIndex("client_id");

                    b.HasIndex("employee_id");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Cleopatra.Domain.Client", b =>
                {
                    b.Property<int>("client_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("client_id"));

                    b.Property<string>("email")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("is_deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("notes")
                        .HasColumnType("longtext");

                    b.Property<string>("phone_number")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.HasKey("client_id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Cleopatra.Domain.Notification", b =>
                {
                    b.Property<int>("notification_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("notification_id"));

                    b.Property<int>("client_id")
                        .HasColumnType("int");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("sent_at")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("notification_id");

                    b.HasIndex("client_id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Cleopatra.Domain.Report", b =>
                {
                    b.Property<int>("report_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("report_id"));

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("report_id");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Cleopatra.Domain.Resource", b =>
                {
                    b.Property<int>("resource_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("resource_id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.Property<int>("reorder_level")
                        .HasColumnType("int");

                    b.Property<string>("unit")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("resource_id");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("Cleopatra.Domain.Service", b =>
                {
                    b.Property<int>("service_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("service_id"));

                    b.Property<int>("category_id")
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("duration")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("price")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("service_id");

                    b.HasIndex("category_id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Cleopatra.Domain.ServiceCategory", b =>
                {
                    b.Property<int>("category_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("category_id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("category_id");

                    b.ToTable("ServiceCategories");
                });

            modelBuilder.Entity("Cleopatra.Domain.Vacation", b =>
                {
                    b.Property<int>("vacation_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("vacation_id"));

                    b.Property<int>("employee_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("end_date")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("start_date")
                        .HasColumnType("datetime(6)");

                    b.HasKey("vacation_id");

                    b.HasIndex("employee_id");

                    b.ToTable("Vacations");
                });

            modelBuilder.Entity("Salon.Domain.Business", b =>
                {
                    b.Property<int>("business_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("business_id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("opening_hours")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("business_id");

                    b.ToTable("Business");
                });

            modelBuilder.Entity("Salon.Domain.Employee", b =>
                {
                    b.Property<int>("employee_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("employee_id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("password_hash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("phone_number")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("specialties")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("working_hours")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("employee_id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Cleopatra.Domain.Appointment", b =>
                {
                    b.HasOne("Cleopatra.Domain.Client", "Client")
                        .WithMany()
                        .HasForeignKey("client_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Salon.Domain.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("employee_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Cleopatra.Domain.Notification", b =>
                {
                    b.HasOne("Cleopatra.Domain.Client", "Client")
                        .WithMany("Notifications")
                        .HasForeignKey("client_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Cleopatra.Domain.Service", b =>
                {
                    b.HasOne("Cleopatra.Domain.ServiceCategory", "category")
                        .WithMany()
                        .HasForeignKey("category_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");
                });

            modelBuilder.Entity("Cleopatra.Domain.Vacation", b =>
                {
                    b.HasOne("Salon.Domain.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("employee_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Cleopatra.Domain.Client", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
