using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LoliSQLLib.DataAttributes
{
    /// <summary>
    /// Класс для менеджмента аттрибутов и их значений
    /// </summary>
    public class AttributeManager<TEntity> where TEntity : class
    {
        #region Private Members

        /// <summary>
        /// Тип сущности
        /// </summary>
        Type _typeOfEntity;

        /// <summary>
        /// Сущность
        /// </summary>
        TEntity obj;

        #endregion

        #region Constructors
        public AttributeManager(Type typeOfEntity)
        {
            this._typeOfEntity = typeOfEntity;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Метод для получения значений аттрибутов в сущности
        /// </summary>
        /// <typeparam name="TAttribute"> тип аттрибута </typeparam>
        /// <typeparam name="TOut"> тип свойства аттрибута </typeparam>
        /// <param name="ValueOfProperty"> лямбда, которая вернет значние свойства </param>
        /// <returns>Коллеский значний аттрибута</returns>
        public IEnumerable<TOut> GetPropertyAttributeValues<TAttribute, TOut>(Func<TAttribute, TOut> ValueOfProperty)
            where TAttribute : System.Attribute
        {
            List<TOut> outs = new List<TOut>();
            //Получение свойств объекта
            foreach (PropertyInfo prop in _typeOfEntity.GetProperties())
            {
                //получая аттрибут свойства
                TAttribute attribute = prop.GetCustomAttribute(typeof(TAttribute)) as TAttribute;
                //получаем значение свойствааттрибута и добовляет его в коллекцию значений аттрибутов
                outs.Add(ValueOfProperty(attribute));
            }
            //Возврат значения из свойства
            foreach (TOut @out in outs)
                yield return @out;
        }

        /// <summary>
        /// Получения значение из свойства аттрибута сущности
        /// </summary>
        /// <typeparam name="TAttribute">тип аттрибута</typeparam>
        /// <typeparam name="TOut">тип свойства аттрибута</typeparam>
        /// <param name="ValueOfClassAttribute">лямбда, которая вернет значние свойства</param>
        /// <returns>Значние свойства</returns>
        public TOut GetClassAttributeValue<TAttribute, TOut>(Func<TAttribute, TOut> ValueOfClassAttribute)
           where TAttribute : System.Attribute
        {
            //получаем тип свойства
            Type typeOfAttribute = typeof(TAttribute);
            //почучаем сам аттрибут
            TAttribute attribute = _typeOfEntity.GetCustomAttribute(typeOfAttribute) as TAttribute;
            //возвращаем значение свойства
            return ValueOfClassAttribute(attribute);
        }
        #endregion
    }
}
