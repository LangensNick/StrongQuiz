using Microsoft.EntityFrameworkCore.Migrations;

namespace StrongQuiz.Models.Migrations
{
    public partial class FinalVersionAddProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Below50Quote",
                table: "Quizzes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Below50Quote",
                table: "Quizzes");
        }
    }
}
