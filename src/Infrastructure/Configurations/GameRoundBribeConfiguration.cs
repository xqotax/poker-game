using Domain.Games.Entities;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GameRoundBribeConfiguration : IEntityTypeConfiguration<GameRoundBribe>
{
	public void Configure(EntityTypeBuilder<GameRoundBribe> builder)
	{
		builder.ToTable(TableNames.GameRoundBribes);

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.ValueGeneratedNever();
	}
}
