using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Primitives;

namespace Domain.Games.Entities;

public sealed partial class GameMember : Entity<Guid>
{
	public Guid UserId { get; private set; }
	public int OrderIndex { get; private set; }
	public bool IsWinner { get; private set; }

	public GameMember(Guid userId, int orderIndex)
	{
		UserId = userId;
		OrderIndex = orderIndex;
		Id = Guid.CreateVersion7();
	}

	public void MarkAsWinner() => IsWinner = true;
}
