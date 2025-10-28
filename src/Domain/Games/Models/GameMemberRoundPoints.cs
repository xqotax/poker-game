using Domain.Games.Enums;

namespace Domain.Games.Models;

public sealed record GameMemberRoundPoints(
	Guid RoundId,
	GameRoundType RoundType,
	uint RoundNumber,
	int DiffPoints
);
