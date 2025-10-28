using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Primitives;

namespace Domain.Games.Entities;

public sealed partial class GameMember : Entity<Guid>
{
	public int OrderIndex { get; private set; }
	public bool IsWinner { get; private set; }

	public GameMember(Guid userId, uint orderIndex) : base(userId) => OrderIndex = orderIndex;

	public void MarkAsWinner() => IsWinner = true;
}
