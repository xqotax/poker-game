using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class Remove_TypeNumber_FromRound : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "TypeNumber",
			schema: "poker-game",
			table: "GameRounds");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<long>(
			name: "TypeNumber",
			schema: "poker-game",
			table: "GameRounds",
			type: "bigint",
			nullable: false,
			defaultValue: 0L);
	}
}
