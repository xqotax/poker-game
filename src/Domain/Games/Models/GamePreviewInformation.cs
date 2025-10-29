using Domain.Games.Enums;
using Domain.Games.ValueObjects;

namespace Domain.Games.Models;

public sealed record GamePreviewInformation(
	Guid Id,
	GameName Name,
	Guid[] MemberIds,
	DateTime CreatedOnUtc,
	GameState State
);
