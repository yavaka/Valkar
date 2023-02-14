using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class SendDocumentsToEmployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TempDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageToEmployee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSigned = table.Column<bool>(type: "bit", nullable: false),
                    UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SentToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TempDocuments_Drivers_SentToId",
                        column: x => x.SentToId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TempDocuments_SentToId",
                table: "TempDocuments",
                column: "SentToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempDocuments");
        }
    }
}
