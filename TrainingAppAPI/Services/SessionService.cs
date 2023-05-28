using MongoDB.Driver;
using TrainingAppAPI.Models;

namespace TrainingAppAPI.Services;

public interface ISessionService {
    Task<Session> CreateSession(string userid);
    Task<Session?> GetSession(string id);
}

public class SessionService : ISessionService {
    private readonly IMongoCollection<Session> _sessionsCollection;

    public SessionService(DatabaseService DBService) {
        _sessionsCollection = DBService.Sessions;
    }

    public async Task<Session> CreateSession(string userid){
        var session = new Session(){
            UserId = userid,
            ExpireAt = DateTime.Now.AddHours(1)
        };

        await _sessionsCollection.InsertOneAsync(session);
        return session;
    }

    public async Task<Session?> GetSession(string id){
        return await _sessionsCollection.Find(sessions => sessions.Id == id).FirstOrDefaultAsync();
    }
}