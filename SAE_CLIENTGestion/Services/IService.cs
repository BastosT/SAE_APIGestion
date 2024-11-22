namespace SAE_CLIENTGestion.Services
{
    public interface IService<TEntity>
    {

        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task PostAsync(TEntity entity);
        Task PutAsync(int id, TEntity entity);
        Task DeleteAsync(int id);

    }
}
