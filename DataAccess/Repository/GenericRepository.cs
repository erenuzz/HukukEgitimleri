using DataAccess.Abstract;
using DataAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        protected readonly DbHukukEgitim _c = new DbHukukEgitim();

        public void Delete(T t)
        {
            _c.Remove(t);
            _c.SaveChanges();
        }

        public T GetById(int id)
        {
            return _c.Set<T>().Find(id);
        }

        public IQueryable<T> GetList(Expression<Func<T, bool>> predicate)
        {
            var result = _c.Set<T>().AsNoTracking().AsQueryable();
            if (predicate != null)
                result = result.Where(predicate);
            return result;

        }

        public void Insert(T t)
        {
            _c.Add(t);
            _c.SaveChanges();
        }

        public void Update(T t)
        {
            _c.Update(t);
            _c.SaveChanges();
        }
    }
}
