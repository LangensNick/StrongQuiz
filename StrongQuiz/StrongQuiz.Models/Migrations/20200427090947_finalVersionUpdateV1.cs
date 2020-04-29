using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StrongQuiz.Models.Migrations
{
    public partial class finalVersionUpdateV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Difficulties_DifficultyId1",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_ScoreQuizzes_Quizzes_QuizId",
                table: "ScoreQuizzes");

            migrationBuilder.DropTable(
                name: "Difficulties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_DifficultyId1",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "DifficultyId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "DifficultyId1",
                table: "Quizzes");

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Quizzes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreQuizzes_Quizzes_QuizId",
                table: "ScoreQuizzes",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_ScoreQuizzes_Quizzes_QuizId",
                table: "ScoreQuizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Quizzes");

            migrationBuilder.AddColumn<Guid>(
                name: "DifficultyId",
                table: "Quizzes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DifficultyId1",
                table: "Quizzes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes",
                column: "DifficultyId");

            migrationBuilder.CreateTable(
                name: "Difficulties",
                columns: table => new
                {
                    DifficultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Difficulties", x => x.DifficultyId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_DifficultyId1",
                table: "Quizzes",
                column: "DifficultyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "DifficultyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Difficulties_DifficultyId1",
                table: "Quizzes",
                column: "DifficultyId1",
                principalTable: "Difficulties",
                principalColumn: "DifficultyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreQuizzes_Quizzes_QuizId",
                table: "ScoreQuizzes",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "DifficultyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
