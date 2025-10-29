using Domain.Games.Entities;
using Infrastructure.Constants;
using Infrastructure.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GameRoundsConfiguration : IEntityTypeConfiguration<GameRound>
{
	public void Configure(EntityTypeBuilder<GameRound> builder)
	{
		builder.ToTable(TableNames.GameRounds);

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.ValueGeneratedNever();

		builder
			.HasOne<GameRoundTypes>()
			.WithMany()
			.HasForeignKey(x => x.Type)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(x => x.Bets)
			.WithOne()
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.HasMany(x => x.Bribes)
			.WithOne()
			.OnDelete(DeleteBehavior.Cascade);
	}
}
