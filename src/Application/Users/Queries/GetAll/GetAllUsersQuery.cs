using Domain.Users;

namespace Application.Users.Queries.GetAll;

public sealed record GetAllUsersQuery : IQuery<User[]>, ILoggingProperties;
