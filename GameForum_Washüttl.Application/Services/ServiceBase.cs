using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameForum_Washüttl.DomainModel.Interfaces;
using GameForum_Washüttl.DomainModel.Models;

namespace GameForum_Washüttl.Application.Services
{
    /// <summary>
    /// Basisklasse die für alle Services verwednet werden kann.
    /// Basisklassen sollten nicht instanzierbar sein und daher
    /// mit dem Modifier abstract vwersehen sein, auch wenn keine
    /// der Methoden abstract ist.
    /// </summary>
    public abstract class ServiceBase<T, TKey> : IReadOnlyService<T>
        where T : class, new()
    {
        private readonly GameForumDBContext _context;

        public ServiceBase(GameForumDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Diese Methode liefert die ersten 100 Datensätze aus einer Tabelle
        /// in der DB. Die Methdoe ist egnerisch gehalten, man kann sie also
        /// für jede belibige Entität der applikation anwenden. Das ist durch den
        /// Parameter (T) neben der Methode angegeben.
        /// Da eine solche Abfrage selten vollständig ist und vermutlich noch
        /// weiter verfeinert werden wird, giebt die Methode etwas "ausführbares" 
        /// (IQueryable) zurückgegeben. Der Aufrufer kann dann den Query 
        /// mittels LinQ weiter verfeinern.
        ///
        /// IEnumerable: für Collections
        /// IQueryable:  für Abfragen
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> GetTable(
            Expression<Func<T, bool>> filterExpression = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            string x = "";
            x = x + "Hello";
            x = x + " World!";

            IQueryable<T> result = _context
                .Set<T>();

            if (filterExpression != null)
            {
                result = result.Where(filterExpression);
            }
            if (orderBy != null)
            {
                result = orderBy(result);
            }

            return result;
        }

        public Task<T> CreateAsync(T newEntity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(TKey id, T newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
