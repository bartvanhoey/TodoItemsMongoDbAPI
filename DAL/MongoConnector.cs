using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using TodoItemsMongoDbAPI.Models;

namespace TodoItemsMongoDbAPI.DAL
{
    public class MongoConnector : IMongoConnector
    {
        public string HostName { get; }
        public string Collection { get; }
        public string Database { get; }
        private MongoClientSettings MongoClientSettings { get; set; }
        private MongoClient Client { get; set; }

        public MongoConnector(string hostName, string collection, string database)
        {
            HostName = hostName;
            Collection = collection;
            Database = database;
            SetMongoClientSettings(HostName);
        }

        public MongoConnector(IConfigurationBuilder configurationBuilder)
        {
            var config = configurationBuilder.Build();
            HostName = config.GetSection("TodoItemsDbDatabaseSettings:HostName").Value;
            Collection = config.GetSection("TodoItemsDbDatabaseSettings:TodoItemsCollection").Value;
            Database = config.GetSection("TodoItemsDbDatabaseSettings:Database").Value;
            SetMongoClientSettings(HostName);
        }

        public MongoConnector(ITodoItemsDbDatabaseSettings settings)
        {
            HostName = settings.HostName;
            Collection = settings.TodoItemsCollection;
            Database = settings.Database;
            SetMongoClientSettings(HostName);
        }

        public void Init()
        {
            Client = new MongoClient(MongoClientSettings);
            var database = Client.GetDatabase(Database);
            var items = database.GetCollection<TodoItem>(Collection);
            database.DropCollection(Collection);
            database.CreateCollection(Collection);

            using var session = Client.StartSession();
            session.StartTransaction();
            try
            {
                var todoItems = new List<TodoItem>
                {
                    new TodoItem {Name = "Clean Windows", IsComplete = false},
                    new TodoItem {Name = "Brush Teeth", IsComplete = true},
                    new TodoItem {Name = "Go to Work", IsComplete = false},
                    new TodoItem {Name = "Drink Coffee", IsComplete = true}
                };
                items.InsertMany(todoItems);
                session.CommitTransaction();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing to MongoDB: " + e.Message);
                session.AbortTransaction();
            }
        }

        public IMongoCollection<TodoItem> GetTodoItemsCollection()
        {
            Client = new MongoClient(MongoClientSettings);
            return Client.GetDatabase(Database).GetCollection<TodoItem>(Collection);
        }

        private void SetMongoClientSettings(string hostName)
        {
            MongoClientSettings = new MongoClientSettings
            {
                WaitQueueSize = int.MaxValue,
                MinConnectionPoolSize = 1,
                MaxConnectionPoolSize = 25,
                RetryWrites = false,
                Server = new MongoServerAddress(hostName, 27017)
            };
        }
    }
}