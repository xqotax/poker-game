namespace Domain.Games.Models;

public sealed record GameMemberPoints(
	Guid GameId,
	Guid MemberId,
	GameMemberRoundPoints[] RoundsPoints
)
{
	public int TotalPoints => RoundsPoints.Sum(x => x.DiffPoints);
}
