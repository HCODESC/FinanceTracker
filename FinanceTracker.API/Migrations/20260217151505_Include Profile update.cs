using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class IncludeProfileupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Users_UserId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_UserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Transactions",
                newName: "UserProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                newName: "IX_Transactions_UserProfileId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Categories",
                newName: "UserProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                newName: "IX_Categories_UserProfileId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Budgets",
                newName: "UserProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Budgets_UserId",
                table: "Budgets",
                newName: "IX_Budgets_UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_UserProfiles_UserProfileId",
                table: "Budgets",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_UserProfiles_UserProfileId",
                table: "Categories",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_UserProfiles_UserProfileId",
                table: "Transactions",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_UserProfiles_UserProfileId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_UserProfiles_UserProfileId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_UserProfiles_UserProfileId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "UserProfileId",
                table: "Transactions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_UserProfileId",
                table: "Transactions",
                newName: "IX_Transactions_UserId");

            migrationBuilder.RenameColumn(
                name: "UserProfileId",
                table: "Categories",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_UserProfileId",
                table: "Categories",
                newName: "IX_Categories_UserId");

            migrationBuilder.RenameColumn(
                name: "UserProfileId",
                table: "Budgets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Budgets_UserProfileId",
                table: "Budgets",
                newName: "IX_Budgets_UserId");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Users_UserId",
                table: "Budgets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_UserId",
                table: "Transactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
