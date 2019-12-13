using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoliSQLLib
{
    public class Table<TEntity> : IEnumerable<TEntity>
    {
        #region Private Members
        /// <summary>
        /// Коллекция сущностей
        /// </summary>
        private List<TEntity> _items;

        #endregion


        #region Constructors
        public Table()
        {
            _items = new List<TEntity>();
        }

        public Table(IEnumerable<TEntity> items)
        {
            _items = items.ToList();
        }
        #endregion

        #region IEnumerable Methods
        public IEnumerator<TEntity> GetEnumerator()
        {
            foreach (var item in _items)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        #endregion

        #region Public Members
        public void Add(TEntity item)
        {
            _items.Add(item);
        }
        #endregion
    }
}
