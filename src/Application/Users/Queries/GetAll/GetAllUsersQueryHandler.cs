using Domain.Users;
using Domain.Users.Repositories;

namespace Application.Users.Queries.GetAll;

public sealed class GetAllUsersQueryHandler(IUsersRepository _usersRepository) : IQueryHandler<GetAllUsersQuery, User[]>
{
	public async Task<Result<User[]>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
	{
		var users = await _usersRepository.GetAll(tracking: false, cancellationToken);

		return users;
	}
}
