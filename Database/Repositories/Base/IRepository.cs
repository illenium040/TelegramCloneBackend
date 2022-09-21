using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        T? Get(string id);
        IEnumerable<T> GetAll();
        void Save();

    }
}
