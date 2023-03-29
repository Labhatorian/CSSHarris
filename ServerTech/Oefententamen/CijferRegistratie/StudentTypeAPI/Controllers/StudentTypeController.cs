using Microsoft.AspNetCore.Mvc;

namespace StudentTypeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentTypeController : ControllerBase
    {
        private static readonly string[] Types = new[]
        {
        "Chipmunk", "Snatcher", "Master"
    };

        private readonly ILogger<StudentTypeController> _logger;

        public StudentTypeController(ILogger<StudentTypeController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetStudentType")]
        public string Get()
        {
            return Types[Random.Shared.Next(Types.Length)];
        }
    }
}