using Domain.Games;
using Domain.Games.Models;
using Domain.Games.Repositories;
using Domain.Games.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public sealed class GamesRepository : IGamesRepository
{
	private readonly ApplicationDbContext _dbContext;

	public GamesRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

	public async Task Add(Game game, CancellationToken cancellationToken)
	{
		await _dbContext.Set<Game>().AddAsync(game, cancellationToken);
	}

	public Task<GamePreviewInformation[]> GetAll(CancellationToken cancellationToken)
	{
		return _dbContext
			.Set<Game>()
			.Include(x => x.Members)
			.Select(g => new GamePreviewInformation(
				g.Id, 
				g.Name, 
				g.Members.Select(m => m.Id).ToArray(), 
				g.State))
			.ToArrayAsync(cancellationToken);
	}

	public Task<Game?> GetById(Guid id, CancellationToken cancellationToken)
	{
		return _dbContext.Set<Game>()
			.Include(g => g.Members)
			.Include(g => g.Rounds)
			.ThenInclude(r => r.Bets)
			.Include(g => g.Rounds)
			.ThenInclude(r => r.Bribes)
			.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
	}

	public Task<bool> NameAlreadyExist(GameName gameName, Guid? exceptId, CancellationToken cancellationToken)
	{
		var query = _dbContext.Set<Game>().AsQueryable();

		if (exceptId.HasValue)
			query = query.Where(g => g.Id != exceptId.Value);

		return query.AnyAsync(g => g.Name == gameName, cancellationToken);
	}
}
