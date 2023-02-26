using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rise.PhoneBook.DbaApi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contact_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    type_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    type_val = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    firstname = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    lastname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    company = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: true),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contacts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contact_infos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_type_id = table.Column<int>(type: "integer", nullable: false),
                    info = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact_infos", x => x.id);
                    table.ForeignKey(
                        name: "fk_contact",
                        column: x => x.contact_id,
                        principalTable: "contacts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_contact_type",
                        column: x => x.contact_type_id,
                        principalTable: "contact_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_contact_infos_contact_id",
                table: "contact_infos",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_contact_infos_contact_type_id",
                table: "contact_infos",
                column: "contact_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contact_infos");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "contact_types");
        }
    }
}
