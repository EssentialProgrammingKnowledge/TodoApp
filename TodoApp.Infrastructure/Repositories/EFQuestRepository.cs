using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Repositories;
using TodoApp.Infrastructure.Database;

namespace TodoApp.Infrastructure.Repositories
{
    internal sealed class EFQuestRepository : IRepository<Quest>
    {
        private readonly TodoDbContext _todoDbContext;

        public EFQuestRepository(TodoDbContext todoDbContext)
        {
            _todoDbContext = todoDbContext;
        }

        public async Task<int> Add(Quest entity)
        {
            await _todoDbContext.Quests.AddAsync(entity);
            await _todoDbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(Quest entity)
        {
            _todoDbContext.Quests.Remove(entity);
            await _todoDbContext.SaveChangesAsync();
        }

        public Task<Quest?> Get(int id)
        {
            return _todoDbContext.Quests.SingleOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IReadOnlyList<Quest>> GetAll()
        {
            return await _todoDbContext.Quests.ToListAsync();
        }

        public async Task Update(Quest entity)
        {
            _todoDbContext.Quests.Update(entity);
            await _todoDbContext.SaveChangesAsync();
        }
    }
}
