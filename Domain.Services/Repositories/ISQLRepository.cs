using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Services.Repositories
{
    public interface ISQLRepository<T> : IBaseRepository<T> where T : BaseEntity 
    {
        Task<IEnumerable<T>> Filter(Func<T, bool> filterExpression);
        Task SaveChanges();
    }
}
