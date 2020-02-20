namespace LoliSQLLib
{
    using LoliSQLLib.DataAttributes;
    using System;
    using System.Collections.Generic;
    //Используется для создания подключения к базе данных и работе с ней
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Абстракция описывающая базу данных
    /// </summary>
    public class DataBase : IDisposable
    {
        #region Private Members
        /// <summary>
        /// Строка подключения
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        private SqlConnection _sqlConnection;
        #endregion

        #region Constructors
        public DataBase(string connectionString)
        {
            this._connectionString = connectionString;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Выполняет запрос к базе данных
        /// </summary>
        /// <param name="command"> Текст запроса </param>
        public void ExecuteCommand(string command)
        {
            using (_sqlConnection = new SqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand(command, _sqlConnection))
                    cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Выполняет запрос к базе данных
        /// </summary>
        /// <param name="command">Текст запроса</param>
        /// <param name="args">Аргументы запроса</param>
        public void ExecuteCommand(string command, params string[] args)
        {
            string query = string.Format(command, args);
            using (_sqlConnection = new SqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                using (SqlCommand cmd = new SqlCommand(query, _sqlConnection))
                    cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Выполняет запрос к базе данных
        /// </summary>
        /// <param name="command">Текст запроса</param>
        /// <param name="args">Аргументы запроса</param>
        public async void ExecuteCommandAsync(string command, params string[] args)
        {
            string query = string.Format(command, args);
            using (_sqlConnection = new SqlConnection(_connectionString))
            {
                await _sqlConnection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, _sqlConnection))
                    await cmd.ExecuteNonQueryAsync();
            }
        }

        public void Dispose()
        {

        }

        /// <summary>
        /// Заполняет сущность данными из базы данных
        /// </summary>
        /// <typeparam name="TEntity">Сущность</typeparam>
        /// <returns>Возвращает коллекцию элементов из TEnity</returns>
        public Table<TEntity> FillEntity<TEntity>()
            where TEntity : class, new()
        {
            return FillEntityAsync<TEntity>().Result;
        }

        /// <summary>
        /// Заполняет сущность данными из базы данных
        /// </summary>
        /// <typeparam name="TEntity">Сущность</typeparam>
        /// <returns>Возвращает коллекцию элементов из TEnity</returns>
        public async Task<Table<TEntity>> FillEntityAsync<TEntity>()
            where TEntity : class, new()
        {
            var attrManager = new AttributeManager<TEntity>(typeof(TEntity));
            //получаем имя сущности
            string tableName = attrManager.GetClassAttributeValue<TableAttribute, String>(a => (a as TableAttribute).Name);
            // var columns     = attrManager.GetPropertyAttributeValues<ColumnAttribute, string>(a => (a as ColumnAttribute).Name).ToList();
            //колллекция для хранения сущностей
            List<TEntity> entities = new List<TEntity>();

            //создаем подключение к базе данных
            using (_sqlConnection = new SqlConnection(_connectionString))
            {
                //открываем подключение
                await _sqlConnection.OpenAsync();


                //создаем объект для хранения запроса
                using (SqlCommand cmd = new SqlCommand())
                {
                    //занесение в свойство CommandText текст T-SQL запроса
                    cmd.CommandText = $"select * from {tableName}";

                    //занесение в свойство Connection подключение к базеданных
                    cmd.Connection = _sqlConnection;

                    //создание объекта для чтения 
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        //чтении данных из бд
                        while (await reader.ReadAsync())
                        {
                            //создание сущности
                            TEntity entity = new TEntity();

                            //заполнение свойст сущности, которые помеченны соответствующими аттрибутами
                            foreach (PropertyInfo prop in typeof(TEntity).GetProperties())
                            {
                                //получение имяни столбца
                                string columnName = (prop.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute).Name;

                                //заполнение свойства сущности
                                prop.SetValue(entity, reader[columnName]);
                            }
                            //добавление в коллекцию сущностей
                            entities.Add(entity);
                        }
                    }
                }
            }
            return new Table<TEntity>(entities);
        }


        /// <summary>
        /// Выполняте запрос Select к базе данных
        /// </summary>
        /// <param name="query">запрос</param>
        /// <typeparam name="TEntity">Сущность</typeparam>
        /// <returns>Возвращает коллекцию элементов из TEnity</returns>
        public Table<TEntity> ExecuteQuery<TEntity>(string query) where TEntity : new()
        {
            var result = ExecuteQueryAsync<TEntity>(query).Result;
            return result; 
        }


        /// <summary>
        /// Выполняте асинхронный запрос Select к базе данных
        /// </summary>
        /// <param name="query">запрос</param>
        /// <typeparam name="TEntity">Сущность</typeparam>
        /// <returns>Возвращает коллекцию элементов из TEnity</returns>
        public async Task<Table<TEntity>> ExecuteQueryAsync<TEntity>(string query) where TEntity : new()
        {
            Table<TEntity> entities = new Table<TEntity>();
            using (_sqlConnection = new SqlConnection(_connectionString))
            {
                await _sqlConnection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    cmd.Connection = _sqlConnection;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            TEntity entity = new TEntity();
                            foreach (var prop in typeof(TEntity).GetProperties())
                            {
                                string columnName = (prop.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute).Name;
                                prop.SetValue(entity, reader[columnName]);
                            }
                            entities.Add(entity);
                        }
                    }
                }
            }
            return entities;
        }
        #endregion
    }
}
