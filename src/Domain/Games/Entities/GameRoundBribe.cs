using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Primitives;

namespace Domain.Games.Entities;

public sealed partial class GameRoundBribe : Entity<Guid>
{
	public Guid MemberId { get; private set; }
	public int Amount { get; private set; }

	public GameRoundBribe(Guid memberId, int amount)
	{
		Id = Guid.CreateVersion7();
		MemberId = memberId;
		Amount = amount;
	}
}
