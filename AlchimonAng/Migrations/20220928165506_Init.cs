using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlchimonAng.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Nik = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    role = table.Column<string>(type: "text", nullable: true),
                    Money = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alchemons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AlName = table.Column<string>(type: "text", nullable: false),
                    Emoji = table.Column<string>(type: "text", nullable: false),
                    Hp = table.Column<int>(type: "integer", nullable: false),
                    Dmg = table.Column<int>(type: "integer", nullable: false),
                    Agi = table.Column<int>(type: "integer", nullable: false),
                    Tear = table.Column<int>(type: "integer", nullable: false),
                    Noise = table.Column<string>(type: "text", nullable: false),
                    Lvl = table.Column<int>(type: "integer", nullable: false),
                    Xp = table.Column<int>(type: "integer", nullable: false),
                    NxtLvlXp = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alchemons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alchemons_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alchemons_Id",
                table: "Alchemons",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alchemons_PlayerId",
                table: "Alchemons",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Id",
                table: "Players",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alchemons");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
