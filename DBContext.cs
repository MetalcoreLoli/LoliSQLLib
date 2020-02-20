
namespace LoliSQLLib
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Configuration;
    
    using LoliSQLLib.DataAttributes;
    /// <summary>
    /// Data Base Context
    /// </summary>
    public abstract class DBContext : IDisposable
    {
        #region Private Members
        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        private String _connectionString;
        #endregion

        #region Public Properties
        /// <summary>
        /// База данных
        /// </summary>
        public DataBase DataBase { get; private set; }
        #endregion

        #region Constructors 
        public DBContext(string connectionStringName)
        {
            //Получение строки подключения из AppConfig'а
           // _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _connectionString = connectionStringName;

            //создание подключения к базе данных
            DataBase = new DataBase(_connectionString);
        }
        /// <summary>
        /// Уничtожает объект
        /// </summary>
        public void Dispose()
        {
            DataBase.Dispose();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Сохраняет изменения в базу данных
        /// </summary>
        public abstract void SaveChanges();



        #endregion
    }
}
