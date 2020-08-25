using Microsoft.EntityFrameworkCore.Migrations;

namespace ApolloWebUI.Migrations
{
    public partial class addHoliday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHolidayDisable",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHolidayDisable",
                table: "Products");
        }
    }
}
