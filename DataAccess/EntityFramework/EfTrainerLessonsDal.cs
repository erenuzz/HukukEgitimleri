using DataAccess.Abstract;
using DataAccess.Repository;
using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework
{
    public class EfTrainerLessonsDal:GenericRepository<TrainerLessons> , ITrainerLessonsDal
    {
    }
}
