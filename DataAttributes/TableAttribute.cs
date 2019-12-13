using System;
using System.Collections.Generic;
using System.Text;

namespace LoliSQLLib.DataAttributes
{
    /// <summary>
    /// Аттрибут для обозначения таблици из БД
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : System.Attribute
    {
        /// <summary>
        /// Имя таблици
        /// </summary>
        public string Name { get; set; }
    }
}
