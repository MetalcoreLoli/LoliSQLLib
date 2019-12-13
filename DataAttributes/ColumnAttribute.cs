using System;
using System.Collections.Generic;
using System.Text;

namespace LoliSQLLib.DataAttributes
{
    /// <summary>
    /// Столец из БД
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : System.Attribute
    {
        #region Public Properties
        /// <summary>
        /// Имя Столбца
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Перавичный Ключ
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Автоинкримент
        /// </summary>
        public bool IsIndetity { get; set; }

        #endregion
    }
}


/*
 a => b
 
    [Table(Name = "A")]
    internal class A
    {
        [Column(Name = "Id", IsPrimaryKey = true)]
        public Int32 Id { get; set; }

        [ForeignKey(Name = "BId", Table = "B")]
        public List<B> B { get; set; }
    }

*/
