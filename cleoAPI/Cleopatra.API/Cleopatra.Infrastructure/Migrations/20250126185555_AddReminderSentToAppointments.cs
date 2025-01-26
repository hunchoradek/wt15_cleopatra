using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cleopatra.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReminderSentToAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Clients_ClientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_EmployeeId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Employees_EmployeeId",
                table: "Vacations");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Vacations",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Vacations",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Vacations",
                newName: "employee_id");

            migrationBuilder.RenameColumn(
                name: "VacationId",
                table: "Vacations",
                newName: "vacation_id");

            migrationBuilder.RenameIndex(
                name: "IX_Vacations_EmployeeId",
                table: "Vacations",
                newName: "IX_Vacations_employee_id");

            migrationBuilder.RenameColumn(
                name: "Specialties",
                table: "Employees",
                newName: "specialties");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Employees",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Employees",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Employees",
                newName: "isDeleted");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Employees",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "WorkingHours",
                table: "Employees",
                newName: "working_hours");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Employees",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Employees",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Employees",
                newName: "employee_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Appointments",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Service",
                table: "Appointments",
                newName: "service");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Appointments",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Appointments",
                newName: "start_time");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Appointments",
                newName: "end_time");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Appointments",
                newName: "employee_id");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Appointments",
                newName: "client_id");

            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                table: "Appointments",
                newName: "appointment_date");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Appointments",
                newName: "appointment_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_EmployeeId",
                table: "Appointments",
                newName: "IX_Appointments_employee_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_ClientId",
                table: "Appointments",
                newName: "IX_Appointments_client_id");

            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "password_hash",
                table: "Employees",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "Clients",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "notes",
                table: "Appointments",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "ReminderSent",
                table: "Appointments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "employee_name",
                table: "Appointments",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Business",
                columns: table => new
                {
                    business_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    opening_hours = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.business_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.report_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServiceCategories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCategories", x => x.category_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Services_category_id",
                table: "Services",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Clients_client_id",
                table: "Appointments",
                column: "client_id",
                principalTable: "Clients",
                principalColumn: "client_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_employee_id",
                table: "Appointments",
                column: "employee_id",
                principalTable: "Employees",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceCategories_category_id",
                table: "Services",
                column: "category_id",
                principalTable: "ServiceCategories",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Employees_employee_id",
                table: "Vacations",
                column: "employee_id",
                principalTable: "Employees",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Clients_client_id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_employee_id",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceCategories_category_id",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Employees_employee_id",
                table: "Vacations");

            migrationBuilder.DropTable(
                name: "Business");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "ServiceCategories");

            migrationBuilder.DropIndex(
                name: "IX_Services_category_id",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "password_hash",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ReminderSent",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "employee_name",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "Vacations",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "Vacations",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "employee_id",
                table: "Vacations",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "vacation_id",
                table: "Vacations",
                newName: "VacationId");

            migrationBuilder.RenameIndex(
                name: "IX_Vacations_employee_id",
                table: "Vacations",
                newName: "IX_Vacations_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "specialties",
                table: "Employees",
                newName: "Specialties");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "Employees",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Employees",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "isDeleted",
                table: "Employees",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Employees",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "working_hours",
                table: "Employees",
                newName: "WorkingHours");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Employees",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Employees",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "employee_id",
                table: "Employees",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Appointments",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "service",
                table: "Appointments",
                newName: "Service");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "Appointments",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "start_time",
                table: "Appointments",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "end_time",
                table: "Appointments",
                newName: "EndTime");

            migrationBuilder.RenameColumn(
                name: "employee_id",
                table: "Appointments",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "Appointments",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "appointment_date",
                table: "Appointments",
                newName: "AppointmentDate");

            migrationBuilder.RenameColumn(
                name: "appointment_id",
                table: "Appointments",
                newName: "AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_employee_id",
                table: "Appointments",
                newName: "IX_Appointments_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_client_id",
                table: "Appointments",
                newName: "IX_Appointments_ClientId");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Notes",
                keyValue: null,
                column: "Notes",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Clients_ClientId",
                table: "Appointments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "client_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_EmployeeId",
                table: "Appointments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Employees_EmployeeId",
                table: "Vacations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
