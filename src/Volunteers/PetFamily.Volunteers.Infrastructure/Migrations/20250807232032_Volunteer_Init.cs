using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Volunteers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Volunteer_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    experience_years = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sur_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_soft_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    date_deletion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    banking_details = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    health_info = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    weight = table.Column<decimal>(type: "numeric", nullable: false),
                    height = table.Column<decimal>(type: "numeric", nullable: false),
                    is_neutered = table.Column<bool>(type: "boolean", nullable: true),
                    is_vaccinated = table.Column<bool>(type: "boolean", nullable: true),
                    date_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    pet_status = table.Column<string>(type: "text", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    addr_apartment = table.Column<int>(type: "integer", nullable: false),
                    addr_city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    addr_country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    addr_house_liter = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    addr_house_number = table.Column<int>(type: "integer", nullable: false),
                    addr_street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    bank_description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    bank_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    breed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    is_soft_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    date_deletion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    file_storages = table.Column<string>(type: "jsonb", nullable: true),
                    phones = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pets", x => x.id);
                    table.ForeignKey(
                        name: "fk_pets_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "volunteers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pets_volunteer_id",
                table: "pets",
                column: "volunteer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pets");

            migrationBuilder.DropTable(
                name: "volunteers");
        }
    }
}
