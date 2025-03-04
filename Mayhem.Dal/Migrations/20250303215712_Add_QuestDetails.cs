using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mayhem.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Add_QuestDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tournamentUserStatistics_tournaments_TournamentId",
                table: "tournamentUserStatistics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tournamentUserStatistics",
                table: "tournamentUserStatistics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tournaments",
                table: "tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_activeGameCodes",
                table: "activeGameCodes");

            migrationBuilder.RenameTable(
                name: "tournamentUserStatistics",
                newName: "TournamentUserStatistics");

            migrationBuilder.RenameTable(
                name: "tournaments",
                newName: "Tournaments");

            migrationBuilder.RenameTable(
                name: "activeGameCodes",
                newName: "ActiveGameCodes");

            migrationBuilder.RenameIndex(
                name: "IX_tournamentUserStatistics_TournamentId",
                table: "TournamentUserStatistics",
                newName: "IX_TournamentUserStatistics_TournamentId");

            migrationBuilder.AddColumn<int>(
                name: "HP",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Tournaments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MP",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SP",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentUserStatistics",
                table: "TournamentUserStatistics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveGameCodes",
                table: "ActiveGameCodes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "QuestDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestDetails_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestDetails_TournamentId",
                table: "QuestDetails",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentUserStatistics_Tournaments_TournamentId",
                table: "TournamentUserStatistics",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentUserStatistics_Tournaments_TournamentId",
                table: "TournamentUserStatistics");

            migrationBuilder.DropTable(
                name: "QuestDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentUserStatistics",
                table: "TournamentUserStatistics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveGameCodes",
                table: "ActiveGameCodes");

            migrationBuilder.DropColumn(
                name: "HP",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "MP",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "SP",
                table: "Tournaments");

            migrationBuilder.RenameTable(
                name: "TournamentUserStatistics",
                newName: "tournamentUserStatistics");

            migrationBuilder.RenameTable(
                name: "Tournaments",
                newName: "tournaments");

            migrationBuilder.RenameTable(
                name: "ActiveGameCodes",
                newName: "activeGameCodes");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentUserStatistics_TournamentId",
                table: "tournamentUserStatistics",
                newName: "IX_tournamentUserStatistics_TournamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tournamentUserStatistics",
                table: "tournamentUserStatistics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tournaments",
                table: "tournaments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_activeGameCodes",
                table: "activeGameCodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tournamentUserStatistics_tournaments_TournamentId",
                table: "tournamentUserStatistics",
                column: "TournamentId",
                principalTable: "tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
