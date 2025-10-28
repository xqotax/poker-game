using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Primitives;
using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Shared;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public sealed partial class User : AggregateRoot<Guid>
{
	public UserName Username { get; private set; }
	public DateTime RegistrationDate { get; private set; }


	public static Result<User> Create(UserName username)
	{
		var user = new User(username);

		return user;
	}
	private User(UserName username)
	{
		Id = Guid.CreateVersion7();
		Username = username;
		RegistrationDate = DateTime.UtcNow;
	}

	public Result Update(UserName newUsername)
	{
		Username = newUsername;

		return Result.Success();
	}
}
