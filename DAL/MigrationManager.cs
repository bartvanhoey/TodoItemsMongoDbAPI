using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TodoItemsMongoDbAPI.Models;

namespace TodoItemsMongoDbAPI.DAL
{
    public static class MigrationManager
    {
//        public static IWebHostBuilder MigrateDatabase(this IWebHostBuilder webHostBuilder,  IConfiguration configuration)
//        {
//                var collectionName = configuration["TodoItemsDbDatabaseSettings:TodoItemsCollectionName"];
////           
//            return webHostBuilder;
//        }

        public static IWebHostBuilder MigrateDatabase(this IWebHostBuilder webhostbuilder)
        {
//            var config = configuration.Method;

            webhostbuilder.Configure(async app =>
            {
                var config = app.ApplicationServices.GetRequiredService<IConfiguration>();
                var connectionString = config["TodoItemsDbDatabaseSettings:ConnectionString"];
                var todoItemsCollectionName = config["TodoItemsDbDatabaseSettings:TodoItemsCollectionName"];
                var databaseName = config["TodoItemsDbDatabaseSettings:DatabaseName"];

                var settings = new MongoClientSettings
                {
                    WaitQueueSize = int.MaxValue,
                    MinConnectionPoolSize = 1,
                    MaxConnectionPoolSize = 25,
                    RetryWrites = false,
                    Server = new MongoServerAddress("localhost",   27017)
                };
                //                settings.WaitQueueuTimeout = new TimeSpan(0,2,0);

                var client = new MongoClient(settings);
                
                // Create the collection object that represents the "products" collection
                var database = client.GetDatabase(databaseName);
                var todoItems = database.GetCollection<TodoItem>(todoItemsCollectionName);

                // Clean up the collection if there is data in there
                database.DropCollection(todoItemsCollectionName);

                // collections can't be created inside a transaction so create it first
                database.CreateCollection(todoItemsCollectionName);

                using (var session =  client.StartSession())
                {
                    // Begin transaction
                    session.StartTransaction();

                    try
                    {
                        // Create some sample data
                        var items = new List<TodoItem>
                        {
                            new TodoItem() {Name = "Clean Windows", IsComplete = false},
                            new TodoItem() {Name = "Brush Teeth", IsComplete = true},
                            new TodoItem() {Name = "Go to Work", IsComplete = false},
                            new TodoItem() {Name = "Drink Coffe", IsComplete = true}
                        };

                            todoItems.InsertMany(items);
                        



                        session.CommitTransaction();
                        var filter = new FilterDefinitionBuilder<TodoItem>().Empty;
                        var results =todoItems.Find<TodoItem>(filter).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error writing to MongoDB: " + e.Message);
                        session.AbortTransaction();
                        return;
                    }

                    // Let's print the new results to the console
//                    Console.WriteLine("\n\nNew Prices (10% increase):\n");
//                    var resultsAfterCommit = await todoItems
//                        .Find<TodoItem>(session, Builders<TodoItem>.Filter.Empty)
//                        .ToListAsync();
//                    foreach (TodoItem d in resultsAfterCommit)
//                    {
//                        Console.WriteLine(
//                            String.Format("TodoItem Name: {0}\tPrice: {1:0.00}",
//                                d.Description, d.Price)
//                        );
//                    }
                }
            });

            return webhostbuilder;
//            configuration?.Invoke());
//            throw new NotImplementedException();
        }
    }
}