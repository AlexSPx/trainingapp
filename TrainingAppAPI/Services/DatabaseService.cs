using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TrainingAppAPI.Models;

namespace TrainingAppAPI.Services;


public class DatabaseService {
    private readonly IMongoDatabase _database;

    public DatabaseService(IOptions<DatabaseSettings> dbSettings){
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        _database = client.GetDatabase(dbSettings.Value.DatabaseName);

    }

    public bool IsDatabaseConnected() {
        try
        {
            // Check the connection by executing a simple database command
            var pingCommand = new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "ping", 1 } });
            var pingResult = _database.RunCommand(pingCommand);
            
            // If the ping was successful, the database is connected
            return pingResult != null;
        }
        catch
        {
            // An exception occurred, indicating a connection issue
            return false;
        }
    }


    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<Session> Sessions => _database.GetCollection<Session>("Sessions");
}