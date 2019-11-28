namespace TodoItemsMongoDbAPI.Models
{
    public interface ITodoItemsDbDatabaseSettings
    {
        string TodoItemsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class TodoItemsDbDatabaseSettings : ITodoItemsDbDatabaseSettings
    {
        public string TodoItemsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}