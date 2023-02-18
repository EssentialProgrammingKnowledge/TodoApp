using Dapper;
using System.Data;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Repositories;

namespace TodoApp.Infrastructure.Repositories
{
    internal class DapperQuestRepository : IRepository<Quest>
    {
        private readonly IDbConnection _dbConnection;

        public DapperQuestRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Task<int> Add(Quest entity)
        {
            return _dbConnection.QuerySingleAsync<int>("""
                INSERT INTO quests (Id, Title, Description, Status, Created, Modified) 
                    VALUES (@Id, @Title, @Description, @Status, @Created, @Modified);
                SELECT LAST_INSERT_ID();
                """, entity);
        }

        public Task Delete(Quest entity)
        {
            return _dbConnection.ExecuteAsync("""
                DELETE FROM quests WHERE Id = @Id
                """, new { entity.Id });
        }

        public Task<Quest?> Get(int id)
        {
            return _dbConnection.QuerySingleOrDefaultAsync<Quest?>(
                """
                SELECT Id, Title, Description, Status, Created, Modified
                FROM quests
                WHERE Id = @Id
                """, new { Id = id });
        }

        public async Task<IReadOnlyList<Quest>> GetAll()
        {
            var result = await _dbConnection.QueryAsync<Quest>(
                """
                SELECT Id, Title, Description, Status, Created, Modified
                FROM quests
                """);
            return result.ToList();
        }

        public Task Update(Quest entity)
        {
            return _dbConnection.ExecuteAsync("""
                UPDATE quests SET Title = @Title, Description = @Description, 
                Status = @Status, Created = @Created, Modified = @Modified 
                WHERE Id = @Id
                """, entity);
        }
    }
}
