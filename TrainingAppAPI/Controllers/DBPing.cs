using Microsoft.AspNetCore.Mvc;
using TrainingAppAPI.Services;

[ApiController]
[Route("/api/[controller]")]
public class DBPing : ControllerBase
{
    private readonly DatabaseService _dbService;
    
    public DBPing(DatabaseService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpGet]
    public IActionResult CheckDatabaseConnection()
    {
        bool isConnected = _dbService.IsDatabaseConnected();
        
        if (isConnected)
        {
            // Database is connected
            return Ok("Database is connected");
        }
        else
        {
            // Database connection issue
            return StatusCode(500, "Database connection error");
        }
    }
}
