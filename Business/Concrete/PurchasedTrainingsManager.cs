using Business.Abstract;
using DataAccess.Abstract;
using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class PurchasedTrainingsManager : IPurchasedTrainingsService
	{
		IPurchasedTrainingsDal _purchasedTrainings;
        public PurchasedTrainingsManager(IPurchasedTrainingsDal purchasedTrainingsDal)
        {
            _purchasedTrainings = purchasedTrainingsDal;
        }
        public void TAdd(PurchasedTrainings t)
		{
			_purchasedTrainings.Insert(t);
		}
		       

        public void TDelete(PurchasedTrainings t)
		{
			_purchasedTrainings.Delete(t);
		}

		public PurchasedTrainings TGetById(int id)
		{
			return _purchasedTrainings.GetById(id);
		}

		public IQueryable<PurchasedTrainings> TGetList(Expression<Func<PurchasedTrainings, bool>> predicate = null)
		{
			return _purchasedTrainings.GetList(predicate);
		}

		public void TUpdate(PurchasedTrainings T)
		{
			_purchasedTrainings.Update(T);
		}
	}
}
