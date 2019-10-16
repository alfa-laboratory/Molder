using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using AlfaBank.AFT.Core.Data.DataBase.DbCommandParameter;
using AlfaBank.AFT.Core.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AlfaBank.AFT.Core.Data.DataBase.DbConnectionWrapper
{
    public class MongoConnectionWrapper : DbConnectionWrapper
    {
        MongoUrlBuilder connection;
        MongoClient client;
        IMongoDatabase database;

        public override (int, IEnumerable<Error>) DeleteRows(string query, string tableName = null, ICollection<DbCommandParameter.DbCommandParameter> @params = null, int? timeout = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// метод устанавливает приватные поля, необходимые для подключения к БД
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public override DbConnection GetDb(IDictionary<string, object> @params)
        {
            string connectionString = "mongodb://" +
            @params["UserID"] + ":" +
            @params["Password"] + "@" +
            @params["DataSource"] + "/" +
            @params["InitialCatalog"];
            connection = new MongoUrlBuilder(connectionString);
            client = new MongoClient(connectionString);
            database = client.GetDatabase(connection.DatabaseName);
            return this.DbConnection;
        }

        public void InsertRows(string tableName, string query)
        {
            var collection = database.GetCollection<BsonDocument>(tableName);
            var document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(query);
            collection.InsertOne(document);
            var number = document.ElementCount;
            //проверить количество на Монго
            var res = collection.Find(document);
            //проверить соответствие на Монго
        }

        public override (object, int, IEnumerable<Error>) InsertRows(string tableName, DataTable records, ICollection<DbCommandParameter.DbCommandParameter> parameter = null, int? timeout = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// метод получает имя таблицы и Aggregate-запрос на получение данных в виде строки,
        /// выдаёт строку, содержащую найденные записи
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public (object, int, IEnumerable<Error>) SelectQuery(string tableName, string query)
        {
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(tableName);
            BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(query);
            PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[] { document };
            Console.WriteLine(document);
            List<Error> errors = new List<Error>();
            string json = "";

            var options = new AggregateOptions()
            {
                AllowDiskUse = false
            };
            using (var cursor = collection.Aggregate(pipeline, options))
            {
                var count = cursor.Current.ToList().Count;
                return (cursor.Current, count, errors);
            }
        }

        public override (object, int, IEnumerable<Error>) SelectQuery(string query, string tableName = null, ICollection<DbCommandParameter.DbCommandParameter> parameter = null, int? timeout = null)
        {
            throw new NotImplementedException();
        }

        public override (int, IEnumerable<Error>) UpdateRows(string query, string tableName = null, ICollection<DbCommandParameter.DbCommandParameter> @params = null, int? timeout = null)
        {
            throw new NotImplementedException();
        }
    }
}
