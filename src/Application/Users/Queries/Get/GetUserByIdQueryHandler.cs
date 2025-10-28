using Domain.Users;
using Domain.Users.DomainErrors;
using Domain.Users.Repositories;

namespace Application.Users.Queries.Get;

public sealed class GetUserByIdQueryHandler(
	IUsersRepository _usersRepository) : IQueryHandler<GetUserByIdQuery, User>
{
	public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var user = await _usersRepository.GetById(request.Id, cancellationToken);

		if (user is null)
			return Result.Failure<User>(UserDomainErrors.User.NotFound);

		return user;
	}
}
