using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopOnlineAPI.Migrations
{
    public partial class AddingClientPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Client");
        }
    }
}
