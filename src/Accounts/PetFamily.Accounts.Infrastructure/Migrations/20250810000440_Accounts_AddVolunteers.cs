using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Accounts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Accounts_AddVolunteers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_participant_accounts_asp_net_users_user_id",
                table: "participant_accounts");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id1",
                table: "participant_accounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_participant_accounts_user_id1",
                table: "participant_accounts",
                column: "user_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_participant_accounts_users_user_id",
                table: "participant_accounts",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_participant_accounts_users_user_id1",
                table: "participant_accounts",
                column: "user_id1",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_participant_accounts_users_user_id",
                table: "participant_accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_participant_accounts_users_user_id1",
                table: "participant_accounts");

            migrationBuilder.DropIndex(
                name: "ix_participant_accounts_user_id1",
                table: "participant_accounts");

            migrationBuilder.DropColumn(
                name: "user_id1",
                table: "participant_accounts");

            migrationBuilder.AddForeignKey(
                name: "fk_participant_accounts_asp_net_users_user_id",
                table: "participant_accounts",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
