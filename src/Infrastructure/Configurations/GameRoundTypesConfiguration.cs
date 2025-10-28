using Domain.Games.Enums;
using Infrastructure.Constants;
using Infrastructure.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GameRoundTypesConfiguration : IEntityTypeConfiguration<GameRoundTypes>
{
	public void Configure(EntityTypeBuilder<GameRoundTypes> builder)
	{
		builder.ToTable(TableNames.GameRoundTypes);

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.HasMaxLength(50)
			.IsRequired();

		var values = Enum.GetValues<GameRoundType>()
			.Select(e => new GameRoundTypes
			{
				Name = Enum.GetName(e)!,
				Id = e
			})
			.ToList();

		builder.HasData(values);
	}
}
