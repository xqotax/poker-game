using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.EnsureSchema(
			name: "poker-game");

		migrationBuilder.CreateTable(
			name: "GameRoundTypes",
			schema: "poker-game",
			columns: table => new
			{
				Id = table.Column<int>(type: "integer", nullable: false),
				Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
			},
			constraints: table => table.PrimaryKey("PK_GameRoundTypes", x => x.Id));

		migrationBuilder.CreateTable(
			name: "Games",
			schema: "poker-game",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uuid", nullable: false),
				Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
				State = table.Column<int>(type: "integer", nullable: false),
				CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				ModifiedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
			},
			constraints: table => table.PrimaryKey("PK_Games", x => x.Id));

		migrationBuilder.CreateTable(
			name: "GameStates",
			schema: "poker-game",
			columns: table => new
			{
				Id = table.Column<int>(type: "integer", nullable: false),
				Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
			},
			constraints: table => table.PrimaryKey("PK_GameStates", x => x.Id));

		migrationBuilder.CreateTable(
			name: "Users",
			schema: "poker-game",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uuid", nullable: false),
				Username = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
				RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
			},
			constraints: table => table.PrimaryKey("PK_Users", x => x.Id));

		migrationBuilder.CreateTable(
			name: "GameMembers",
			schema: "poker-game",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uuid", nullable: false),
				OrderIndex = table.Column<int>(type: "integer", nullable: false),
				IsWinner = table.Column<bool>(type: "boolean", nullable: false),
				GameId = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_GameMembers", x => x.Id);
				table.ForeignKey(
					name: "FK_GameMembers_Games_GameId",
					column: x => x.GameId,
					principalSchema: "poker-game",
					principalTable: "Games",
					principalColumn: "Id");
			});

		migrationBuilder.CreateTable(
			name: "GameRounds",
			schema: "poker-game",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uuid", nullable: false),
				Type = table.Column<int>(type: "integer", nullable: false),
				GeneralNumber = table.Column<long>(type: "bigint", nullable: false),
				TypeNumber = table.Column<long>(type: "bigint", nullable: false),
				GameId = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_GameRounds", x => x.Id);
				table.ForeignKey(
					name: "FK_GameRounds_GameRoundTypes_Type",
					column: x => x.Type,
					principalSchema: "poker-game",
					principalTable: "GameRoundTypes",
					principalColumn: "Id",
					onDelete: ReferentialAction.Restrict);
				table.ForeignKey(
					name: "FK_GameRounds_Games_GameId",
					column: x => x.GameId,
					principalSchema: "poker-game",
					principalTable: "Games",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "GameRoundBets",
			schema: "poker-game",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uuid", nullable: false),
				MemberId = table.Column<Guid>(type: "uuid", nullable: false),
				Amount = table.Column<int>(type: "integer", nullable: true),
				GameRoundId = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_GameRoundBets", x => x.Id);
				table.ForeignKey(
					name: "FK_GameRoundBets_GameRounds_GameRoundId",
					column: x => x.GameRoundId,
					principalSchema: "poker-game",
					principalTable: "GameRounds",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "GameRoundBribes",
			schema: "poker-game",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uuid", nullable: false),
				MemberId = table.Column<Guid>(type: "uuid", nullable: false),
				Amount = table.Column<int>(type: "integer", nullable: false),
				GameRoundId = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_GameRoundBribes", x => x.Id);
				table.ForeignKey(
					name: "FK_GameRoundBribes_GameRounds_GameRoundId",
					column: x => x.GameRoundId,
					principalSchema: "poker-game",
					principalTable: "GameRounds",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.InsertData(
			schema: "poker-game",
			table: "GameRoundTypes",
			columns: ["Id", "Name"],
			values: new object[,]
			{
					{ 0, "None" },
					{ 1, "Regular" },
					{ 2, "Dark" },
					{ 3, "Meager" },
					{ 4, "Trumpless" },
					{ 5, "Golden" },
					{ 6, "Forehead" }
			});

		migrationBuilder.InsertData(
			schema: "poker-game",
			table: "GameStates",
			columns: ["Id", "Name"],
			values: new object[,]
			{
					{ 0, "None" },
					{ 1, "NotStarted" },
					{ 2, "Active" },
					{ 3, "Finished" }
			});

		migrationBuilder.CreateIndex(
			name: "IX_GameMembers_GameId",
			schema: "poker-game",
			table: "GameMembers",
			column: "GameId");

		migrationBuilder.CreateIndex(
			name: "IX_GameRoundBets_GameRoundId",
			schema: "poker-game",
			table: "GameRoundBets",
			column: "GameRoundId");

		migrationBuilder.CreateIndex(
			name: "IX_GameRoundBribes_GameRoundId",
			schema: "poker-game",
			table: "GameRoundBribes",
			column: "GameRoundId");

		migrationBuilder.CreateIndex(
			name: "IX_GameRounds_GameId",
			schema: "poker-game",
			table: "GameRounds",
			column: "GameId");

		migrationBuilder.CreateIndex(
			name: "IX_GameRounds_Type",
			schema: "poker-game",
			table: "GameRounds",
			column: "Type");

		migrationBuilder.CreateIndex(
			name: "IX_Games_CreatedOnUtc",
			schema: "poker-game",
			table: "Games",
			column: "CreatedOnUtc");

		migrationBuilder.CreateIndex(
			name: "IX_Games_Name",
			schema: "poker-game",
			table: "Games",
			column: "Name",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "IX_Users_Username",
			schema: "poker-game",
			table: "Users",
			column: "Username",
			unique: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "GameMembers",
			schema: "poker-game");

		migrationBuilder.DropTable(
			name: "GameRoundBets",
			schema: "poker-game");

		migrationBuilder.DropTable(
			name: "GameRoundBribes",
			schema: "poker-game");

		migrationBuilder.DropTable(
			name: "GameStates",
			schema: "poker-game");

		migrationBuilder.DropTable(
			name: "Users",
			schema: "poker-game");

		migrationBuilder.DropTable(
			name: "GameRounds",
			schema: "poker-game");

		migrationBuilder.DropTable(
			name: "GameRoundTypes",
			schema: "poker-game");

		migrationBuilder.DropTable(
			name: "Games",
			schema: "poker-game");
	}
}
