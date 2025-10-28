using Domain.Users.ValueObjects;

namespace Domain.Users.Repositories;

public interface IUsersRepository
{
	Task Add(User user, CancellationToken cancellationToken);
	Task<User?> GetById(Guid id, CancellationToken cancellationToken);

	Task<bool> NameAlreadyExist(UserName userName, Guid? exceptId, CancellationToken cancellationToken);
}
