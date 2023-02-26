using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rise.PhoneBook.ReportApi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "report");

            migrationBuilder.CreateTable(
                name: "report_queue_processes",
                schema: "report",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    datas = table.Column<string>(type: "jsonb", nullable: false),
                    queueprocess = table.Column<string>(type: "jsonb", nullable: false),
                    last_queue_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    queue_status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    filename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    changed_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    state = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_queue_processes", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "report_queue_processes",
                schema: "report");
        }
    }
}
