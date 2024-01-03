using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentApp.Data;
using StudentApp.Models;
using System.Numerics;

namespace StudentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly CollegeDBContext _dbContext;
        public StudentController(ILogger<StudentController> logger, CollegeDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            _logger.LogInformation("GetStudents method started");
            return Ok(_dbContext.Students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetAllStudentById")]
        public ActionResult<Student> GetStudentById(int id)
        {
            return Ok(_dbContext.Students.Where(n => n.Id == id).FirstOrDefault());
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        public ActionResult<Student> GetStudentByName(string name)
        {
            return Ok(_dbContext.Students.Where(n => n.Name == name).FirstOrDefault());
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Student> CreateStudent([FromBody] Student student)
        {
            return Ok("Created...");
        }

        [HttpPut]
        [Route("Update")]
        public ActionResult<Student> UpdateStudent([FromBody] Student student)
        {
            return Ok();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        public ActionResult<Student> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<Student> patchDocument)
        {
            return Ok();
        }

        [HttpDelete("{id}", Name = "DeleteStudentById")]
        public bool DeleteStudent(int id)
        {
            var student = _dbContext.Students.Where(n => n.Id == id).FirstOrDefault();
            _dbContext.Students.Remove(student);
            return true;
        }
    }
}
