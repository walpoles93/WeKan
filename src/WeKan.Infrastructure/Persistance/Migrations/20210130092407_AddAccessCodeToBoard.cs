using Microsoft.EntityFrameworkCore.Migrations;

namespace WeKan.Infrastructure.Persistance.Migrations
{
    public partial class AddAccessCodeToBoard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessCode",
                table: "Boards",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessCode",
                table: "Boards");
        }
    }
}
