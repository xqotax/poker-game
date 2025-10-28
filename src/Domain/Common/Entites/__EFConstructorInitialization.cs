//Private empty constructor should exist to allow EF Core get entities from DB
//But "null possible" warning are important in C# in general and you should avoid this warnings
//So purpose of this file to store these private ctors

#pragma warning disable CS8618

namespace Domain.Games
{
	public partial class Game { private Game() { } }
}


namespace Domain.Games.Entities
{
	public sealed partial class GameMember { private GameMember() { } }
	public sealed partial class GameRound { private GameRound() { } }
	public sealed partial class GameRoundBet { private GameRoundBet() { } }
	public sealed partial class GameRoundBribe { private GameRoundBribe() { } }
}

namespace Domain.Users
{
	public partial class User { private User() { } }
}
