using Microsoft.EntityFrameworkCore.Migrations;

namespace RpgBot.Migrations
{
    public partial class User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    GroupId = table.Column<string>(type: "TEXT", nullable: true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Reputation = table.Column<int>(type: "INTEGER", nullable: false),
                    Experience = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    HealthPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxHealthPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    ManaPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxManaPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    StaminaPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxStaminaPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    MessagesCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_UserId_GroupId",
                table: "User",
                columns: new[] { "UserId", "GroupId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
