namespace Presentation.Games.ViewModels;

public sealed record GameMemberViewModel(
	string Id,
	string Name,
	int OrderIndex,
	bool Winner
);
