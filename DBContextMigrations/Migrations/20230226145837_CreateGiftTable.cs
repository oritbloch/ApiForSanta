using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContextMigrations.Migrations
{
    /// <inheritdoc />
    public partial class CreateGiftTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiftRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<float>(type: "real", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GiftsOfChild",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    giftId = table.Column<int>(type: "int", nullable: false),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    requestID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftsOfChild", x => x.id);
                    table.ForeignKey(
                        name: "FK_GiftsOfChild_GiftRequests_requestID",
                        column: x => x.requestID,
                        principalTable: "GiftRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GiftsOfChild_Gifts_giftId",
                        column: x => x.giftId,
                        principalTable: "Gifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GiftsOfChild_giftId",
                table: "GiftsOfChild",
                column: "giftId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftsOfChild_requestID",
                table: "GiftsOfChild",
                column: "requestID");

            migrationBuilder.InsertData(
       table: "Gifts",
       columns: new[] { "Name", "Price" },
       values: new object[,]
       {
            { "PSP",  50 },
            { "Rocket",  45 },
            { "RC Car", 25 },
            { "Lego", 15 },
            { "Barbie", 10 },
            { "Cryon's", 10 },
            { "Candies", 5 },
            { "Mittens", 3 },
       });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiftsOfChild");

            migrationBuilder.DropTable(
                name: "GiftRequests");

            migrationBuilder.DropTable(
                name: "Gifts");
        }
    }
}
