using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using AlfaBank.AFT.Core.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AlfaBank.AFT.Core.Data.DataBase.DbConnectionWrapper
{
    /// <summary>
    /// Реализация работы с MongoDB
    /// </summary>
    public class MongoDBConnectionWrapper : DbConnectionWrapper
    {
        MongoUrlBuilder connection;
        MongoClient client;
        IMongoDatabase database; 

        /// <summary>
        /// Шаг удаления записей из колекции в MongoDB
        /// TОDО написать шаг
        /// </summary>
        /// <param name="query">Записи на удаление.</param>
        /// <param name="tableName">Название коллекции.</param>
        /// <param name="params">Параметры подключения.</param>
        /// <param name="timeout">Таймаут</param>
        /// <returns></returns>
        public override (int, IEnumerable<Error>) DeleteRows(string query, string tableName = null, ICollection<DbCommandParameter.DbCommandParameter> @params = null, int? timeout = null)
        {
            throw new NotImplementedException();
        }

        /// <summary> 
        /// Метод устанавливает приватные поля, необходимые для подключения к БД 
        /// </summary> 
        /// <param name="params">Параметры подключения.</param> 
        /// <returns>Объект подключения и список ошибок.</returns> 
        public override (DbConnection, IEnumerable<Error>) GetDb(IDictionary<string, object> @params)
        {
            var errors = new List<Error>();
            string connectionString = "mongodb://" +
            @params["UserID"] + ":" +
            @params["Password"] + "@" +
            @params["DataSource"] + "/" +
            @params["InitialCatalog"];
            connection = new MongoUrlBuilder(connectionString)
            {
                ConnectTimeout = TimeSpan.FromSeconds(60),
                MaxConnectionLifeTime = TimeSpan.FromSeconds(90),
                ServerSelectionTimeout = TimeSpan.FromSeconds(90)
            };

            client = new MongoClient(connectionString);
            database = client.GetDatabase(connection.DatabaseName);

            var isConnectionValid = false;
            try
            {
                isConnectionValid = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
            }
            catch (Exception e)
            {
                errors.Add(new Error
                {
                    TargeBase = e.TargetSite,
                    Message = e.Message,
                    Type = e.GetType()
                });
            }
            if (!isConnectionValid)
            {
                errors.Add(new Error
                {
                    Message = "Некорректный источник подключения.",
                    Type = typeof(ArgumentException)
                });

            }
            return (this.DbConnection, errors);
        }

        /// <summary>
        /// Метод вставки записей в MongoDB
        /// </summary>
        /// <param name="tableName">Название коллекции.</param>
        /// <param name="records">Данные для добавление.</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="timeout">Не используется.</param>
        /// <returns>Добавленные данные, количество добавленных записей и список ошибок</returns>
        public override (object, int, IEnumerable<Error>) InsertRows(string tableName, object records, ICollection<DbCommandParameter.DbCommandParameter> parameter = null, int? timeout = null)
        {
            var errors = new List<Error>();
            try
            {

                var collection = database.GetCollection<BsonDocument>(tableName);
                var documents = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument[]>(records.ToString());
                collection.InsertMany(documents);
                return (documents, documents.ToList().Count, errors);
            }
            catch (Exception e)
            {
                errors.Add(new Error
                {
                    TargeBase = e.TargetSite,
                    Message = e.Message,
                    Type = e.GetType()
                });
                return (null, 0, errors);
            }
        }

        /// <summary>
        /// Получение IEnumerable<BsonDocument>, количества BsonDocument по запросу query из таблицы tableName
        /// </summary>
        /// <param name="query">Запрос для поиска.</param>
        /// <param name="tableName">Название коллекции.</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="timeout">Не используется.</param>
        /// <returns>Полученные данные, количество полученных записей и список ошибок</returns>
        public override (object, int, IEnumerable<Error>) SelectQuery(string query, string tableName = null, ICollection<DbCommandParameter.DbCommandParameter> parameter = null, int? timeout = null)
        {
            var errors = new List<Error>();
            try
            {
                var collection = database.GetCollection<BsonDocument>(tableName);
                var document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(query);
                var pipeline = (PipelineDefinition<BsonDocument, BsonDocument>)new BsonDocument[] { document };
                var options = new AggregateOptions()
                {
                    AllowDiskUse = false
                };

                using (var cursor = collection.Aggregate(pipeline, options))
                {
                    cursor.MoveNext();
                    var batch = cursor.Current;
                    return ((BsonDocument[])batch, batch.ToList().Count, errors);
                }
            }
            catch (Exception e)
            {
                errors.Add(new Error
                {
                    TargeBase = e.TargetSite,
                    Message = e.Message,
                    Type = e.GetType()
                });
                return (null, 0, errors);
            }
        }

        /// <summary>
        /// Шаг обновления записей в колекции в MongoDB
        /// TОDО написать шаг
        /// </summary>
        /// <param name="query">Записи на удаление.</param>
        /// <param name="tableName">Название коллекции.</param>
        /// <param name="params">Параметры подключения.</param>
        /// <param name="timeout">Таймаут</param>
        /// <returns></returns>
        public override (int, IEnumerable<Error>) UpdateRows(string query, string tableName = null, ICollection<DbCommandParameter.DbCommandParameter> @params = null, int? timeout = null)
        {
            throw new NotImplementedException();
        }
    }
}