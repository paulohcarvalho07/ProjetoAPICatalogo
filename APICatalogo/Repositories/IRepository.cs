using System.Linq.Expressions;

namespace APICatalogo.Repositories;

public interface IRepository<T>
{
    //cuidado para nãp violar o principio ISP
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity); 
}
