namespace RestAPI.Repositories;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}