namespace TodoItemsMongoDbAPI.Models
{
    public interface ITodoItemsDbDatabaseSettings
    {
        string TodoItemsCollection { get; set; }
        string HostName { get; set; }
        string Database { get; set; }
    }

    public class TodoItemsDbDatabaseSettings : ITodoItemsDbDatabaseSettings
    {
        public string TodoItemsCollection { get; set; }
        public string HostName { get; set; }
        public string Database { get; set; }
    }
}