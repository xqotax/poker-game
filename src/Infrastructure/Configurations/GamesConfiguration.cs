using Domain.Games;
using Domain.Games.ValueObjects;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GamesConfiguration : IEntityTypeConfiguration<Game>
{
	public void Configure(EntityTypeBuilder<Game> builder)
	{
		builder.ToTable(TableNames.Games);

		builder.HasKey(x => x.Id);

		builder
			.HasMany(x => x.Rounds)
			.WithOne()
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.Property(x => x.Name)
			.HasConversion(
				v => v.Value,
				v => GameName.Create(v).Value)
			.HasMaxLength(GameName.MaxLength)
			.IsRequired();

		builder
			.HasIndex(x => x.Name)
			.IsUnique();

		builder.HasIndex(x => x.CreatedOnUtc);
	}
}
