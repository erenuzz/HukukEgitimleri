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
    public class TrainingHoursManager : ITrainingHoursService
    {
        ITrainingHoursDal _trainingHoursDal;

        public TrainingHoursManager(ITrainingHoursDal trainingHoursDal)
        {
            _trainingHoursDal = trainingHoursDal;
        }

        public void TAdd(TrainingHours t)
        {
            _trainingHoursDal.Insert(t);
        }


        public void TDelete(TrainingHours t)
        {
           _trainingHoursDal.Delete(t);
        }

        public TrainingHours TGetById(int id)
        {
            return _trainingHoursDal.GetById(id);
        }

        public IQueryable<TrainingHours> TGetList(Expression<Func<TrainingHours, bool>> predicate = null)
        {
            return _trainingHoursDal.GetList(predicate);
        }

        public void TUpdate(TrainingHours T)
        {
            _trainingHoursDal.Update(T);
        }
    }
}
