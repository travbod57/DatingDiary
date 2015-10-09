using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DatingDiary.Repository
{
	interface IRepository<T>
	{
		void Insert(T entity);
		void Delete(T entity);
		IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
		IQueryable<T> GetAll();
		T GetById(int id);
	}
}
