using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StrongQuiz.Models.Migrations
{
    public partial class finalversionV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "DifficultyId",
                table: "Quizzes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Above75Quote",
                table: "Quizzes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Below75Quote",
                table: "Quizzes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DifficultyId1",
                table: "Quizzes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "QuestionCount",
                table: "Quizzes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes",
                column: "DifficultyId");

            migrationBuilder.CreateTable(
                name: "Difficulties",
                columns: table => new
                {
                    DifficultyId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Above75Quote",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Below75Quote",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "DifficultyId1",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "QuestionCount",
                table: "Quizzes");

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
    }
}
