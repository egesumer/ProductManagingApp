using System.Linq.Expressions;

public interface IRepository<T> where T : class
	{
	bool Add(T entity);
    bool Delete(T entity);
    bool Update(T entity);
    
    IEnumerable<T> GetAll(bool includeDeleted = false);
    T GetByID(Guid id, bool includeDeleted = false);
    IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate, bool includeDeleted = false);
    T SingleOrDefault(Expression<Func<T, bool>> predicate, bool includeDeleted = false);
	}