using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taka.App.Deliverer.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deliverers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CNPJ = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CNHNumber = table.Column<string>(type: "text", nullable: false),
                    CNHType = table.Column<int>(type: "integer", nullable: false),
                    CNHImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliverers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliverers_CNHNumber",
                table: "Deliverers",
                column: "CNHNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliverers_CNPJ",
                table: "Deliverers",
                column: "CNPJ",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deliverers");
        }
    }
}
