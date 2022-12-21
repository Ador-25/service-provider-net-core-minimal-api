using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceProviderApi.Migrations
{
    public partial class LastMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "UserRequestsServices");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceProviderID",
                table: "UserRequestsServices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Task",
                table: "UserRequestsServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAddress",
                table: "UserRequestsServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserBid",
                table: "UserRequestsServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "UserRequestsServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPhoneNumber",
                table: "UserRequestsServices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Task",
                table: "UserRequestsServices");

            migrationBuilder.DropColumn(
                name: "UserAddress",
                table: "UserRequestsServices");

            migrationBuilder.DropColumn(
                name: "UserBid",
                table: "UserRequestsServices");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "UserRequestsServices");

            migrationBuilder.DropColumn(
                name: "UserPhoneNumber",
                table: "UserRequestsServices");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceProviderID",
                table: "UserRequestsServices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "UserRequestsServices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
