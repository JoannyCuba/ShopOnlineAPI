using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopOnlineAPI.Migrations
{
    public partial class AddingSuperAdminClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Name", "Email", "PhoneNumber","Password", "CreatedAt" },
                values: new object[]
                {
                    "8a946576-f9df-4b51-9187-594dacc2f9cd",
                    "Super",
                    "superadmin@gmail.com",
                    "",
                    "Passw0rd",
                    DateTime.Now,
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
