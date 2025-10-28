using Domain.Games.Enums;
using Infrastructure.Constants;
using Infrastructure.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GameStatesConfiguration : IEntityTypeConfiguration<GameStates>
{
	public void Configure(EntityTypeBuilder<GameStates> builder)
	{
		builder.ToTable(TableNames.GameStates);

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.HasMaxLength(50)
			.IsRequired();

		var values = Enum.GetValues<GameState>()
			.Select(e => new GameStates
			{
				Name = Enum.GetName(e)!,
				Id = e
			})
			.ToList();

		builder.HasData(values);
	}
}
