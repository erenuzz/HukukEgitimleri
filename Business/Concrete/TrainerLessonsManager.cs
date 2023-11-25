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
    public class TrainerLessonsManager : ITrainerLessonsService
    {
        ITrainerLessonsDal _trainerLessonsDal;

        public TrainerLessonsManager(ITrainerLessonsDal trainerLessonsDal)
        {
            _trainerLessonsDal = trainerLessonsDal;
        }

        public void TAdd(TrainerLessons t)
        {
            _trainerLessonsDal.Insert(t);
        }       

        public void TDelete(TrainerLessons t)
        {
           _trainerLessonsDal.Delete(t);
        }

        public TrainerLessons TGetById(int id)
        {
            return _trainerLessonsDal.GetById(id);
        }

        public IQueryable<TrainerLessons> TGetList(Expression<Func<TrainerLessons, bool>> predicate = null)
        {
           return _trainerLessonsDal.GetList(predicate);
        }

        public void TUpdate(TrainerLessons T)
        {
            _trainerLessonsDal.Update(T);
        }

        
    }
}
