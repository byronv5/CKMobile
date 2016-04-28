using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB;
using MongoDB.Configuration;

namespace CK.Dal
{
    /// <summary>
    /// Mongo for linq
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongodbHelper<T> where T : class
    {
        readonly string _connectionString = string.Empty;

        readonly string _databaseName = string.Empty;

        readonly string _collectionName = string.Empty;

        #region 构造函数
        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="connectionStr">数据库连接字符串</param>
        /// <param name="collectionName"></param>
        public MongodbHelper(string connectionStr, string collectionName)
        {
            string[] str = connectionStr.Split(';');
            _connectionString = str[0];//服务器
            _databaseName = str[1];//数据库名
            _collectionName = collectionName;//collection名称
        }
        #endregion

        #region 实现linq查询的映射配置
        /// <summary>
        /// 实现linq查询的映射配置
        /// </summary>
        private MongoConfiguration Configuration
        {
            get
            {
                var config = new MongoConfigurationBuilder();

                config.Mapping(mapping =>
                {
                    mapping.DefaultProfile(profile => profile.SubClassesAre(t => t.IsSubclassOf(typeof(T))));
                    mapping.Map<T>();
                });

                config.ConnectionString(_connectionString);

                return config.BuildConfiguration();
            }
        }
        #endregion

        #region 插入操作
        /// <summary>
        /// 插入操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public void Insert(T t)
        {
            using (Mongo mongo = new Mongo(Configuration))
            {
                try
                {
                    mongo.Connect();

                    var db = mongo.GetDatabase(_databaseName);

                    var collection = db.GetCollection<T>(_collectionName);

                    collection.Insert(t, true);

                    mongo.Disconnect();

                }
                catch (Exception)
                {
                    mongo.Disconnect();
                    throw;
                }
            }
        }
        #endregion

        #region 更新操作
        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public void Update(T t, Expression<Func<T, bool>> func)
        {
            using (Mongo mongo = new Mongo(Configuration))
            {
                try
                {
                    mongo.Connect();

                    var db = mongo.GetDatabase(_databaseName);

                    var collection = db.GetCollection<T>(_collectionName);

                    collection.Update<T>(t, func, true);

                    mongo.Disconnect();

                }
                catch (Exception)
                {
                    mongo.Disconnect();
                    throw;
                }
            }
        }
        #endregion

        #region 获取集合
        /// <summary>
        /// 获取集合（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="func"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public List<T> List(int pageIndex, int pageSize, Expression<Func<T, bool>> func, out int pageCount)
        {
            pageCount = 0;

            using (Mongo mongo = new Mongo(Configuration))
            {
                try
                {
                    mongo.Connect();

                    var db = mongo.GetDatabase(_databaseName);

                    var collection = db.GetCollection<T>(_collectionName);

                    pageCount = Convert.ToInt32(collection.FindAll().Documents.Count()) % pageSize > 0
                        ? Convert.ToInt32(collection.FindAll().Documents.Count()) / pageSize + 1
                        : Convert.ToInt32(collection.FindAll().Documents.Count()) / pageSize;//页数

                    var resultList = collection.Linq().Where(func).Skip(pageSize * (pageIndex - 1))
                                                   .Take(pageSize).Select(i => i).ToList();

                    mongo.Disconnect();

                    return resultList;

                }
                catch (Exception)
                {
                    mongo.Disconnect();
                    throw;
                }
            }
        }

        /// <summary>
        /// 获取集合（无分页）
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public List<T> List(Expression<Func<T, bool>> func)
        {
            using (Mongo mongo = new Mongo(Configuration))
            {
                try
                {
                    mongo.Connect();

                    var db = mongo.GetDatabase(_databaseName);

                    var collection = db.GetCollection<T>(_collectionName);

                    var resultList = collection.Linq().Where(func).Select(i => i).ToList();

                    mongo.Disconnect();

                    return resultList;

                }
                catch (Exception)
                {
                    mongo.Disconnect();
                    throw;
                }
            }
        }
        #endregion

        #region 读取单条记录
        /// <summary>
        /// 读取单条记录
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public T Single(Expression<Func<T, bool>> func)
        {
            using (Mongo mongo = new Mongo(Configuration))
            {
                try
                {
                    mongo.Connect();

                    var db = mongo.GetDatabase(_databaseName);

                    var collection = db.GetCollection<T>(_collectionName);

                    var single = collection.Linq().FirstOrDefault(func);

                    mongo.Disconnect();

                    return single;

                }
                catch (Exception)
                {
                    mongo.Disconnect();
                    throw;
                }
            }
        }
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="func"></param>
        public void Delete(Expression<Func<T, bool>> func)
        {
            using (Mongo mongo = new Mongo(Configuration))
            {
                try
                {
                    mongo.Connect();

                    var db = mongo.GetDatabase(_databaseName);

                    var collection = db.GetCollection<T>(_collectionName);

                    //这个地方要注意，一定要加上T参数，否则会当作object类型处理
                    //导致删除失败
                    collection.Remove<T>(func);

                    mongo.Disconnect();

                }
                catch (Exception)
                {
                    mongo.Disconnect();
                    throw;
                }
            }
        }
        #endregion
    }
}
