using Business.Abstract;
using DataAccess.Abstract;
using Entity.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class EducationManager : IEducationService
    {
        IEducationDal _educationDal;

        public EducationManager(IEducationDal educationDal)
        {
            _educationDal = educationDal;
        }

        public void TAdd(Education t)
        {
                      
            _educationDal.Insert(t);
        }

        

        public void TDelete(Education t)
        {
            _educationDal.Delete(t);
        }

        public Education TGetById(int id)
        {
            return _educationDal.GetById(id);
        }

        public IQueryable<Education> TGetList(Expression<Func<Education, bool>> predicate=null)
        {
            return _educationDal.GetList(predicate);
        }

        public void TUpdate(Education T)
        {
            _educationDal.Update(T);
        }
    }
}
