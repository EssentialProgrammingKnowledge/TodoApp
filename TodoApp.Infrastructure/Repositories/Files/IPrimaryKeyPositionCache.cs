namespace TodoApp.Infrastructure.Repositories.Files
{
    internal interface IPrimaryKeyPositionCache<T>
        where T : class
    {
        int GetPosition(int key);
        int GetLastPosition();
        Task AddPosition(int key, int position);
        Task UpdatePositions(IEnumerable<PrimaryKeyPosition> primaryKeys);
    }
}
