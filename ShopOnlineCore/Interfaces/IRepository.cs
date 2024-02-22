using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineCore.Interfaces
{
    public interface IRepository<T>
    {
        /// <summary>
		/// Search list of elements given a search term and with pagination
		/// </summary>
		/// <param name="filter">Search term</param>
		/// <returns>Returns a list elements</returns>
		public List<T> Find(object search = null, int page = 1, int itemPerPage = 25);
        /// <summary>
        /// Count the total number of elements given a search term
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public int Count(object search = null);
        /// <summary>
        /// Search element by identifier
        /// </summary>
        /// <param name="filter">Search term</param>
        /// <returns>Return element</returns>
        public T FindOne(string Id);
        /// <summary>
        /// Add element
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity);
        /// <summary>
        /// Update element
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity);
        /// <summary>
        /// Remove Element
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity);
    }
}
