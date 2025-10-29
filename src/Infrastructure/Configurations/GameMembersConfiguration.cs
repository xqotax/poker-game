using Domain.Games.Entities;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GameMembersConfiguration : IEntityTypeConfiguration<GameMember>
{
	public void Configure(EntityTypeBuilder<GameMember> builder)
	{
		builder.ToTable(TableNames.GameMembers);

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.ValueGeneratedNever();
	}
}
