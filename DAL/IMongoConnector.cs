using MongoDB.Driver;
using TodoItemsMongoDbAPI.Models;

namespace TodoItemsMongoDbAPI.DAL
{
    public interface IMongoConnector
    {
        string HostName { get; }
        string Collection { get; }
        string Database { get; }
        IMongoCollection<TodoItem> GetTodoItemsCollection();
    }
}