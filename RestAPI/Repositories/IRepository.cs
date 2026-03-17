using System.Linq.Expressions;
using RestAPI.Models;

namespace RestAPI.Repositories;

public interface IRepository<T>
{
    Task<T?> GetById(long id);
    Task<List<T>> GetAll();
    Task<T> Add(T item);
    Task<T> Update(T item);
    Task Delete(T item);
    Task<List<Note>> Find(Expression<Func<T, bool>> predicate);
}