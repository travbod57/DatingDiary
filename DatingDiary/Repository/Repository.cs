using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq.Expressions;
using System.Data.Linq;
using System.Linq;
using DatingDiary.Model;

namespace DatingDiary.Repository
{
	public class Repository<T> : IRepository<T> where T : class
    {
        protected Table<T> DataTable;

        public Repository(DatingAppContext dataContext)
        {
            DataTable = dataContext.GetTable<T>();
        }
 
        #region IRepository<T> Members
 
        public void Insert(T entity)
        {
            DataTable.InsertOnSubmit(entity);
        }
 
        public void Delete(T entity)
        {
            DataTable.DeleteOnSubmit(entity);
        }
 
        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return DataTable.Where(predicate);
        }
 
        public IQueryable<T> GetAll()
        {
            return DataTable;
        }
 
        public T GetById(int id)
        {
            // Sidenote: the == operator throws NotSupported Exception!
            // 'The Mapping of Interface Member is not supported'
            // Use .Equals() instead
            return DataTable.Single(e => e.Equals(id));
        }
 
        #endregion

  	}
}
