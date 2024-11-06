using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly AppDbContext db;

    public GenericRepository(AppDbContext db)
    {
        this.db = db;
    }

    public bool Add(T entity)
    {
        try
        {
            db.Set<T>().Add(entity);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(T entity)
    {
        try
        {
            entity.IsDeleted = true;
            db.Set<T>().Update(entity);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<T> GetAll(bool includeDeleted = false)
    {
        if (includeDeleted)
        {
            return db.Set<T>().ToList();
        }
        return db.Set<T>().Where(x => !x.IsDeleted).ToList();
    }

    public T GetByID(Guid id, bool includeDeleted = false)
    {
        if (includeDeleted)
        {
            return db.Set<T>().FirstOrDefault(x => x.Id == id);
        }
        return db.Set<T>().FirstOrDefault(x => x.Id == id && !x.IsDeleted);
    }

    public IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate, bool includeDeleted = false)
    {
        if (includeDeleted)
        {
            return db.Set<T>().Where(predicate).ToList();
        }
        return db.Set<T>().Where(x => !x.IsDeleted).Where(predicate).ToList();
    }

    public T SingleOrDefault(Expression<Func<T, bool>> predicate, bool includeDeleted = false)
    {
        if (includeDeleted)
        {
            return db.Set<T>().SingleOrDefault(predicate);
        }
        return db.Set<T>().Where(x => !x.IsDeleted).SingleOrDefault(predicate);
    }

    public bool Update(T entity)
    {
        try
        {
            db.Set<T>().Update(entity);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }
}
