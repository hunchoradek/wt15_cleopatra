using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cleopatra.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                           WHERE TABLE_SCHEMA = 'cleo_db' 
                           AND TABLE_NAME = 'Clients')
            BEGIN
                CREATE TABLE `Clients` (
                    `client_id` int NOT NULL AUTO_INCREMENT,
                    `name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
                    `phone_number` varchar(15) CHARACTER SET utf8mb4 NOT NULL,
                    `email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
                    `notes` longtext CHARACTER SET utf8mb4 NOT NULL,
                    CONSTRAINT `PK_Clients` PRIMARY KEY (`client_id`)
                ) CHARACTER SET=utf8mb4;
            END
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");
        }
    }

}
