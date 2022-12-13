using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zawody.Data.Migrations
{
    public partial class Createdadditionalmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stadion",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Pojemnosc = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stadion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stadion_Address_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "Identity",
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "Identity",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamID = table.Column<int>(type: "int", nullable: true),
                    Pozycja = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Person_User_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Team",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShortenedName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StadionID = table.Column<int>(type: "int", nullable: true),
                    TrenerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Person_TrenerID",
                        column: x => x.TrenerID,
                        principalSchema: "Identity",
                        principalTable: "Person",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Team_Stadion_StadionID",
                        column: x => x.StadionID,
                        principalSchema: "Identity",
                        principalTable: "Stadion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Person_CreatedById",
                schema: "Identity",
                table: "Person",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Person_TeamID",
                schema: "Identity",
                table: "Person",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Stadion_AddressId",
                schema: "Identity",
                table: "Stadion",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_StadionID",
                schema: "Identity",
                table: "Team",
                column: "StadionID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_TrenerID",
                schema: "Identity",
                table: "Team",
                column: "TrenerID",
                unique: true,
                filter: "[TrenerID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Team_TeamID",
                schema: "Identity",
                table: "Person",
                column: "TeamID",
                principalSchema: "Identity",
                principalTable: "Team",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Team_TeamID",
                schema: "Identity",
                table: "Person");

            migrationBuilder.DropTable(
                name: "Team",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Stadion",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Address",
                schema: "Identity");
        }
    }
}
