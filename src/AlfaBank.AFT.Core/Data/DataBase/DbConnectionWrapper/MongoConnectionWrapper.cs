using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
        //IMongoCollection<BsonDocument> collection;

        public override (int, IEnumerable<Error>) DeleteRows(string query, ICollection<DbCommandParameter.DbCommandParameter> @params = null, int? timeout = null)
        {
            throw new NotImplementedException();
        }

        public override DbConnection GetDb(IDictionary<string, object> @params)
        {
            string connectionString = "mongodb://" +
            @params["UserID"] + ":" +
            @params["Password"] + "@" +
            @params["DataSource"] + "/" +
            @params["InitialCatalog"];
            connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            database = client.GetDatabase(connection.DatabaseName);
            return this.DbConnection;
        }

        public void InsertRows(string tableName, string query)
        {
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(tableName);
            BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(query);
            collection.InsertOne(document);
            var number = document.ElementCount;
            collection.Find<BsonDocument>(document);
            //проверить соответствие
        }

        public override (DataTable, int, IEnumerable<Error>) InsertRows(string tableName, DataTable records, ICollection<DbCommandParameter.DbCommandParameter> parameter = null, int? timeout = null)
        {
            throw new NotImplementedException();
        }

        public string SelectQuery(string tableName, string query)
        {
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(tableName);
            BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(query);
            PipelineDefinition<BsonDocument, BsonDocument> pipeline = new BsonDocument[] { document };
            Console.WriteLine(document);

            string json = "";

            var options = new AggregateOptions()
            {
                AllowDiskUse = false
            };
            using (var cursor = collection.Aggregate(pipeline, options))
            {
                while (cursor.MoveNext())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    Console.WriteLine(batch.ToJson());
                    foreach (BsonDocument doc in batch)
                    {
                        json += doc.ToJson() + Environment.NewLine;
                    }
                }
            }
            return json;
        }

        public override (DataTable, int, IEnumerable<Error>) SelectQuery(string query, ICollection<DbCommandParameter.DbCommandParameter> parameter = null, int? timeout = null)
        {
            throw new NotImplementedException();
        }

        public override (int, IEnumerable<Error>) UpdateRows(string query, ICollection<DbCommandParameter.DbCommandParameter> @params = null, int? timeout = null)
        {
            throw new NotImplementedException();
        }
    }
}
