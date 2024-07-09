using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class constraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Email",
                table: "Users",
                sql: "\"Email\" ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_PhoneNumber",
                table: "Users",
                sql: "\"PhoneNumber\" ~* '^\\+375(25|29|33|44)\\d{7}$'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Email",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_PhoneNumber",
                table: "Users");
        }
    }
}
