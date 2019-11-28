using System.Collections.Generic;
using MongoDB.Driver;
using TodoItemsMongoDbAPI.Models;

namespace TodoItemsMongoDbAPI.Services
{
    public class TodoItemsService
    {
        private readonly IMongoCollection<TodoItem> _todoItems;

        public TodoItemsService(ITodoItemsDbDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _todoItems = database.GetCollection<TodoItem>(settings.TodoItemsCollectionName);
        }

        public List<TodoItem> Get() =>
            _todoItems.Find(todoItem => true).ToList();

        public TodoItem Get(string id) =>
            _todoItems.Find(todoItem => todoItem.Id == id).FirstOrDefault();

        public TodoItem Create(TodoItem todoItem)
        {
            _todoItems.InsertOne(todoItem);
            return todoItem;
        }

        public void Update(string id, TodoItem todoItemIn) =>
            _todoItems.ReplaceOne(todoItem => todoItem.Id == id, todoItemIn);

        public void Remove(TodoItem todoItemIn) =>
            _todoItems.DeleteOne(todoItem => todoItem.Id == todoItemIn.Id);

        public void Remove(string id) => 
            _todoItems.DeleteOne(todoItem => todoItem.Id == id);
    }
}