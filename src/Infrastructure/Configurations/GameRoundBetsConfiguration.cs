using Domain.Games.Entities;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GameRoundBetsConfiguration : IEntityTypeConfiguration<GameRoundBet>
{
	public void Configure(EntityTypeBuilder<GameRoundBet> builder)
	{
		builder.ToTable(TableNames.GameRoundBets);

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.ValueGeneratedNever();
	}
}
