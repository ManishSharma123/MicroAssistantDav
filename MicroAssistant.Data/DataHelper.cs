using log4net;
using MicroAssistant.Data;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace MicroAssistant.Data
{
    public static class DataHelper
    {
        private static MicroAssistantEntities context = new MicroAssistantEntities();
        private static readonly string DELETE_FIELD_NAME = "IsDeleted";
        private static readonly string ACTIVE_FIELD_NAME = "IsActive";
        private static readonly ILog logger = LogManager.GetLogger(typeof(DataHelper));

        static DataHelper()
        {

        }

        public static object GetMaxID<T>()
           where T : class
        {
            object id = new object();
            try
            {
                var parameter = Expression.Parameter(typeof(T), "p");
                var left = Expression.PropertyOrField(parameter, "Id");
                var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(left, typeof(object)), parameter);

                id = context.Set<T>().Max(lambda.Compile());
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetMaxID > {0}", ex.StackTrace));
            }

            return id;
        }

        /// <summary>
        /// Checks the record is exist or not.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>True/False</returns>
        public static bool IsExist<T>(int Id)
            where T : class
        {
            bool exist = false;
            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("Id", Id);

                IQueryable<T> query = context.Set<T>().GetList<T>(parameters);
                exist = query.Count() > 0;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > IsExist > {0}", ex.StackTrace));
            }
            return exist;
        }

        /// <summary>
        /// Checks the record is exist or not.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>True/False</returns>
        public static bool IsExist<T>(long Id)
            where T : class
        {
            bool exist = false;
            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("Id", Id);

                IQueryable<T> query = context.Set<T>().GetList<T>(parameters);
                exist = query.Count() > 0;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > IsExist > {0}", ex.StackTrace));
            }
            return exist;
        }

        /// <summary>
        /// Checks the record is exist or not.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>True/False</returns>
        public static bool IsExist<T>(Guid Id)
            where T : class
        {
            bool exist = false;
            try
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("Id", Id);

                IQueryable<T> query = context.Set<T>().GetList<T>(parameters);
                exist = query.Count() > 0;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > IsExist > {0}", ex.StackTrace));
            }
            return exist;
        }

        /// <summary>
        /// Checks the record is exist or not.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>True/False</returns>
        public static bool IsExist<T>(Hashtable parameters)
            where T : class
        {
            IQueryable<T> query = context.Set<T>().GetList<T>(parameters);
            return query.Count() > 0;
        }

        /// <summary>
        /// Adds new record into the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object Add<T>(T entity, string Username)
        where T : class
        {
            object id = new object();
            System.Type type = typeof(T);
            try
            {
                var created_by = type.GetProperty("CreatedBy");
                if (created_by != null)
                {
                    created_by.SetValue(entity, Username, null);
                }

                var dateCreated = type.GetProperty("DateCreated");
                if (dateCreated != null)
                {
                    dateCreated.SetValue(entity, DateTime.UtcNow, null);
                }
                context.Set<T>().Add(entity);
                context.SaveChanges();

                var parameter = Expression.Parameter(typeof(T), "p");
                var left = Expression.PropertyOrField(parameter, "Id");
                var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(left, typeof(object)), parameter);
                id = context.Set<T>().Max(lambda.Compile());
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Add > {0}", ex.StackTrace));
            }
            return id;
        }

        /// <summary>
        /// Deletes the record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Delete<T>(int Id, string Username)
        where T : class
        {
            bool flag = false;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "p");
                var left = Expression.PropertyOrField(parameter, "Id");
                var right = Expression.Constant(Id, Id.GetType());
                var body = Expression.Equal(left, right);

                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

                T entity = GetSingle<T>(Id);


                System.Type type = typeof(T);
                var updated_by = type.GetProperty("UpdatedBy");
                if (updated_by != null)
                {
                    updated_by.SetValue(entity, Username, null);
                }

                var dateUpdated = type.GetProperty("DateUpdated");
                if (dateUpdated != null)
                {
                    dateUpdated.SetValue(entity, DateTime.UtcNow, null);
                }

                var deleted = type.GetProperty("IsDeleted");
                if (deleted != null)
                {
                    deleted.SetValue(entity, true, null);
                }

                context.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                flag = true;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Delete > {0}", ex.StackTrace));
            }
            return flag;
        }

        /// <summary>
        /// Deletes the record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Delete<T>(long Id, string Username)
        where T : class
        {
            bool flag = false;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "p");
                var left = Expression.PropertyOrField(parameter, "Id");
                var right = Expression.Constant(Id, Id.GetType());
                var body = Expression.Equal(left, right);

                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

                T entity = GetSingle<T>(Id);


                System.Type type = typeof(T);
                var updated_by = type.GetProperty("UpdatedBy");
                if (updated_by != null)
                {
                    updated_by.SetValue(entity, Username, null);
                }

                var dateUpdated = type.GetProperty("DateUpdated");
                if (dateUpdated != null)
                {
                    dateUpdated.SetValue(entity, DateTime.UtcNow, null);
                }

                var deleted = type.GetProperty("IsDeleted");
                if (deleted != null)
                {
                    deleted.SetValue(entity, true, null);
                }


                context.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                flag = true;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Delete > {0}", ex.StackTrace));
            }
            return flag;
        }

        /// <summary>
        /// Deletes the record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Delete<T>(Guid Id, string Username)
        where T : class
        {
            bool flag = false;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "p");
                var left = Expression.PropertyOrField(parameter, "Id");
                var right = Expression.Constant(Id, Id.GetType());
                var body = Expression.Equal(left, right);

                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

                T entity = GetSingle<T>(Id);


                System.Type type = typeof(T);
                var updated_by = type.GetProperty("UpdatedBy");
                if (updated_by != null)
                {
                    updated_by.SetValue(entity, Username, null);
                }

                var dateUpdated = type.GetProperty("DateUpdated");
                if (dateUpdated != null)
                {
                    dateUpdated.SetValue(entity, DateTime.UtcNow, null);
                }

                var deleted = type.GetProperty("IsDeleted");
                if (deleted != null)
                {
                    deleted.SetValue(entity, true, null);
                }

                context.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                flag = true;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Delete > {0}", ex.StackTrace));
            }
            return flag;
        }

        /// <summary>
        /// Deletes the record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Delete<T>(T entity, string Username)
        where T : class
        {
            bool flag = false;

            try
            {
                System.Type type = typeof(T);
                var updated_by = type.GetProperty("UpdatedBy");
                if (updated_by != null)
                {
                    updated_by.SetValue(entity, Username, null);
                }

                var dateUpdated = type.GetProperty("DateUpdated");
                if (dateUpdated != null)
                {
                    dateUpdated.SetValue(entity, DateTime.UtcNow, null);
                }

                var deleted = type.GetProperty("IsDeleted");
                if (deleted != null)
                {
                    deleted.SetValue(entity, true, null);
                }

                context.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                flag = true;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Delete > {0}", ex.StackTrace));
            }
            return flag;
        }
        public static bool Remove<T>(T entity)
        where T : class
        {
            bool flag = false;

            try
            {
                context.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();

                flag = true;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Delete > {0}", ex.StackTrace));
            }
            return flag;
        }
        /// <summary>
        /// Updates the existing record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public static bool Update<T>(T entity, string Username)
            where T : class
        {
            bool flag = false;
            try
            {
                System.Type type = typeof(T);
                var updated_by = type.GetProperty("UpdatedBy");
                if (updated_by != null)
                {
                    updated_by.SetValue(entity, Username, null);
                }

                var dateUpdated = type.GetProperty("DateUpdated");
                if (dateUpdated != null)
                {
                    dateUpdated.SetValue(entity, DateTime.UtcNow, null);
                }

                context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                flag = true;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Delete > {0}", ex.StackTrace));
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// Gets the count of records by excluding deleted records. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int GetExclusiveCounts<T>()
          where T : class
        {
            return GetExclusiveAll<T>().Count();
        }

        public static int GetActiveCounts<T>(string Key, int Value)
         where T : class
        {
            return context.Set<T>().GetActiveCounts<T>(Key, Value);
        }

        /// <summary>
        /// Gets the count of records by excluding deleted records. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static int GetActiveCounts<T>(this IQueryable<T> source, string field, int Id)
          where T : class
        {
            System.Type type = typeof(T);
            int count = 0;
            try
            {
                var property = type.GetProperty(DELETE_FIELD_NAME);
                var parameter = Expression.Parameter(type, "p");
                var left = Expression.PropertyOrField(parameter, property.Name);
                var right = Expression.Constant(false, property.PropertyType);
                var body = Expression.Equal(left, right);

                var property1 = type.GetProperty(ACTIVE_FIELD_NAME);
                var parameter1 = Expression.Parameter(type, "p");
                var left1 = Expression.PropertyOrField(parameter, property1.Name);
                var right1 = Expression.Constant(true, property1.PropertyType);
                var body1 = Expression.Equal(left1, right1);

                var ee = Expression.And(body, body1);

                var property2 = type.GetProperty(field);
                var left2 = Expression.PropertyOrField(parameter, property2.Name);
                var right2 = Expression.Constant(Id, property2.PropertyType);
                var body2 = Expression.Equal(left2, right2);

                var e1 = Expression.And(ee, body2);

                var whereClause = Expression.Lambda<Func<T, bool>>(ee, parameter);
                count = context.Set<T>().Where(whereClause).Count();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetActiveCounts > {0}", ex.StackTrace));
            }
            return count;
        }

        /// <summary>
        /// Gets a single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetSingle<T>(object Id)
            where T : class
        {
            T entity = null;
            Hashtable parameters = new Hashtable();
            try
            {
                parameters.Add("Id", Id);
                entity = context.Set<T>().GetSingle<T>(parameters);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetSingle > {0}", ex.StackTrace));
            }
            return entity;
        }

        /// <summary>
        /// Gets a single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetSingle<T>(int Id)
            where T : class
        {
            T entity = null;
            Hashtable parameters = new Hashtable();
            try
            {
                parameters.Add("Id", Id);
                entity = context.Set<T>().GetSingle<T>(parameters);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetSingle > {0}", ex.StackTrace));
            }
            return entity;
        }
        /// <summary>

        /// Gets a single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetSingle<T>(long Id)
            where T : class
        {
            T entity = null;
            Hashtable parameters = new Hashtable();
            try
            {
                parameters.Add("Id", Id);
                entity = context.Set<T>().GetSingle<T>(parameters);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetSingle > {0}", ex.StackTrace));
            }
            return entity;
        }

        /// <summary>
        /// Gets a single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetSingle<T>(Guid Id)
            where T : class
        {
            T entity = null;
            Hashtable parameters = new Hashtable();
            try
            {
                parameters.Add("Id", Id);
                entity = context.Set<T>().GetSingle<T>(parameters);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetSingle > {0}", ex.StackTrace));
            }
            return entity;
        }

        /// <summary>
        /// Gets a single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>;
        /// <returns></returns>
        public static T GetSingle<T>(Hashtable parameters)
            where T : class
        {
            T entity = null;
            try
            {
                entity = context.Set<T>().GetSingle<T>(parameters);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetSingle > {0}", ex.StackTrace));
            }
            return entity;
        }


        /// <summary>
        /// Gets the list of type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List of Type Parameter passed</returns>
        public static IQueryable<T> GetList<T>()
      where T : class
        {
            return context.Set<T>().AsQueryable<T>();
        }

        /// <summary>
        /// Gets the list of type based object (any data type) as Primary Key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static IQueryable<T> GetList<T>(object Id)
        where T : class
        {
            IQueryable<T> list = null;
            Hashtable parameters = new Hashtable();
            try
            {
                parameters.Add("Id", Id);
                list = context.Set<T>().GetList<T>(parameters);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetList > {0}", ex.StackTrace));
            }
            return list;
        }

        /// <summary>
        /// Gets the list of type based integer as Primary Key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static IQueryable<T> GetList<T>(int Id)
        where T : class
        {
            IQueryable<T> list = null;
            Hashtable parameters = new Hashtable();
            try
            {
                parameters.Add("Id", Id);
                list = context.Set<T>().GetList<T>(parameters);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetList > {0}", ex.StackTrace));
            }
            return list;
        }

        /// <summary>
        /// Gets the list of type based long as Primary Key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static IQueryable<T> GetList<T>(long Id)
        where T : class
        {
            IQueryable<T> list = null;
            Hashtable parameters = new Hashtable();
            try
            {
                parameters.Add("Id", Id);
                list = context.Set<T>().GetList<T>(parameters);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetList > {0}", ex.StackTrace));
            }
            return list;
        }

        /// <summary>
        /// Gets the list of type based GUID as Primary Key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static IQueryable<T> GetList<T>(Guid Id)
        where T : class
        {
            IQueryable<T> list = null;
            Hashtable parameters = new Hashtable();
            try
            {
                parameters.Add("Id", Id);
                list = context.Set<T>().GetList<T>(parameters);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetList > {0}", ex.StackTrace));
            }
            return list;
        }

        /// <summary>
        /// Gets the list of type based on the parameters passed as criteria.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns>List Type Parameter passed.</returns>
        public static IQueryable<T> GetList<T>(Hashtable parameters)
         where T : class
        {
            IQueryable<T> list = null;
            try
            {
                list = context.Set<T>().GetList<T>(parameters);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetList > {0}", ex.StackTrace));
            }
            return list;
        }

        public static int GetCounts<T>(Hashtable parameters)
         where T : class
        {
            return context.Set<T>().GetCounts<T>(parameters);
        }

        /// <summary>
        /// Builds the filter criteria for query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static BinaryExpression Build<T>(Hashtable parameters, out ParameterExpression parameter)
            where T : class
        {
            System.Type type = typeof(T);
            BinaryExpression criteria = null;
            parameter = Expression.Parameter(type, "p");

            try
            {
                IEnumerator keys = parameters.Keys.GetEnumerator();
                while (keys.MoveNext())
                {
                    string key = keys.Current.ToString();
                    var property = type.GetProperty(key);
                    var left = Expression.PropertyOrField(parameter, property.Name);
                    var right = Expression.Constant(parameters[key], property.PropertyType);

                    if (criteria == null)
                    {
                        criteria = Expression.Equal(left, right);
                    }
                    else
                    {
                        var body = Expression.Equal(left, right);
                        criteria = Expression.And(criteria, body);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Private > Build > {0}", ex.StackTrace));
            }

            return criteria;
        }

        /// <summary>
        /// Gets the number of record counts.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static int GetCounts<T>(this IQueryable<T> source, Hashtable parameters)
         where T : class
        {
            BinaryExpression criteria = null;
            int count = 0;
            try
            {
                ParameterExpression parameter = null;
                criteria = Build<T>(parameters, out parameter);

                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);
                count = context.Set<T>().Count(whereClause);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Private > GetCounts > {0}", ex.StackTrace));
            }
            return count;
        }

        /// <summary>
        /// Gets the single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static T GetSingle<T>(this IQueryable<T> source, Hashtable parameters)
          where T : class
        {
            BinaryExpression criteria = null;
            T entity = null;
            try
            {
                ParameterExpression parameter = null;
                criteria = Build<T>(parameters, out parameter);
                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);

                entity = context.Set<T>().Where(whereClause).Single<T>();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Private > GetSingle > {0}", ex.StackTrace));
            }
            return entity;
        }

        /// <summary>
        /// Gets the list of records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static IQueryable<T> GetList<T>(this IQueryable<T> source, Hashtable parameters)
          where T : class
        {
            BinaryExpression criteria = null;
            IQueryable<T> list = null;
            try
            {
                ParameterExpression parameter = null;
                criteria = Build<T>(parameters, out parameter);
                if (criteria != null)
                {
                    var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);
                    list = context.Set<T>().Where(whereClause).AsQueryable<T>();
                }
                else
                {
                    list = context.Set<T>().GetAll<T>();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Private > GetList > {0}", ex.StackTrace));
            }
            return list;
        }

        /// <summary>
        /// Gets the count of all records by including deleted one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int GetCounts<T>()
           where T : class
        {
            int counts = 0;
            try
            {
                counts = context.Set<T>().Count();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetInclusiveAll > {0}", ex.StackTrace));
            }
            return counts;
        }

        /// <summary>
        /// Gets all the records including deleted one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> GetInclusiveAll<T>()
            where T : class
        {
            IQueryable<T> list = null;
            try
            {
                list = context.Set<T>().AsQueryable<T>();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetInclusiveAll > {0}", ex.StackTrace));
            }
            return list;
        }

        /// <summary>
        /// Gets all the records excluding deleted one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> GetExclusiveAll<T>()
            where T : class
        {
            IQueryable<T> records = null;
            System.Type type = typeof(T);
            try
            {
                var property = type.GetProperty(DELETE_FIELD_NAME);
                var parameter = Expression.Parameter(type, property.Name);
                var left = Expression.PropertyOrField(parameter, property.Name);
                var right = Expression.Constant(false, property.PropertyType);
                var body = Expression.Equal(left, right);
                var whereClause = Expression.Lambda<Func<T, bool>>(body, parameter);

                records = context.Set<T>().Where(whereClause).AsQueryable<T>();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetExclusiveAll > {0}", ex.StackTrace));
            }
            return records;
        }

        /// <summary>
        /// Gets the records by building lamada expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="field"></param>
        /// <param name="IsDeleted"></param>
        /// <returns></returns>
        private static IQueryable<T> GetAll<T>(this IQueryable<T> source)
            where T : class
        {
            IQueryable<T> records = null;
            System.Type type = typeof(T);
            try
            {
                var property = type.GetProperty(DELETE_FIELD_NAME);
                var parameter = Expression.Parameter(type, property.Name);
                var left = Expression.PropertyOrField(parameter, property.Name);
                var right = Expression.Constant(false, property.PropertyType);
                var body = Expression.Equal(left, right);
                var whereClause = Expression.Lambda<Func<T, bool>>(body, parameter);

                records = source.Where(whereClause).AsQueryable<T>();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Private > GetAll > {0}", ex.StackTrace));
            }
            return records;
        }

        public static IQueryable<T> GetPageWise<T>(Hashtable parameters, string order_field, int index, int size, out int total)
            where T : class
        {
            IQueryable<T> list = null;
            total = 0;
            try
            {
                list = context.Set<T>().GetPageWise<T>(parameters, order_field, index, size, out total);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > GetListPageWise > {0}", ex.StackTrace));
            }
            return list;
        }

        /// <summary>
        /// Gets the list of records page wise.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="parameters"></param>
        /// <param name="order_field"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        private static IQueryable<T> GetPageWise<T>(this IQueryable<T> source, Hashtable parameters, string order_field, int index, int size, out int total)
          where T : class
        {
            System.Type type = typeof(T);
            BinaryExpression criteria = null;
            IQueryable<T> list = null;
            ParameterExpression parameter = null;
            total = 0;
            try
            {
                criteria = Build<T>(parameters, out parameter);
                var whereClause = Expression.Lambda<Func<T, bool>>(criteria, parameter);
                var query = context.Set<T>().Where(whereClause);

                var orderByProperty = type.GetProperty(order_field);
                var orderByExpression = Expression.PropertyOrField(parameter, orderByProperty.Name);
                var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(orderByExpression, typeof(object)), parameter);
                list = query.OrderByDescending(lambda.Compile()).Skip((index - 1) * size).Take(size).AsQueryable<T>();

                if (list != null)
                {
                    total = list.Count();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DataHelper > Private > GetListPageWise > {0}", ex.StackTrace));
            }

            return list;
        }

        public static IQueryable<T> In<T>(this IQueryable<T> source,
                            IQueryable<T> checkAgainst)
        {
            IQueryable<T> list = null;
            try
            {
                list = from s in source
                       where checkAgainst.Contains(s)
                       select s;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return list;
        }

        public static IQueryable<T> NotIn<T>(this IQueryable<T> source,
                                          IQueryable<T> checkAgainst)
        {
            IQueryable<T> list = null;
            try
            {
                list = from s in source
                       where !checkAgainst.Contains(s)
                       select s;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return list;
        }
    }
}