using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}


//It is time to create the generic repository that will serve us all the CRUD methods. As a result, all the methods can be called upon any repository class in our project.

//Furthermore, creating the generic repository and repository classes that use that generic repository is not going to be the final step. We will go a  step further and create a wrapper around repository classes and inject it as a service. Consequently, we can instantiate this wrapper once and then call any repository class we need inside any of our controllers.