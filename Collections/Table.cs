using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoliSQLLib.Core;

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
            _items = new List<TEntity>();
            foreach (var item in items)
                Add(item);
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
            var context = new EntityValidtionContext<TEntity>(item);
            var errors = new List<String>();
            if (EntityValidator<TEntity>.IsValid(item, context, errors))
                _items.Add(item);
            else
            {
               /* foreach (var error in errors)
                    throw new Exception(error);*/
            }
        }

        public void Remove(TEntity item)
        {
            _items.Remove(item);
        }
        #endregion
    }
}
