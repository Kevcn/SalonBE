using Microsoft.EntityFrameworkCore.Migrations;

namespace SalonAPI.Data.Migrations
{
    public partial class Added_UserId_In_Stock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Stocks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_UserId",
                table: "Stocks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_AspNetUsers_UserId",
                table: "Stocks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_AspNetUsers_UserId",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_UserId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Stocks");
        }
    }
}
