using Microsoft.AspNetCore.Mvc;
using StudentApp.Models;
using System.Numerics;

namespace StudentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return new List<Student>(){ new Student
            {
                Id = 1,
                StudentName = "Student1",
                Email = "student1@gmail.com",
                Address = "student1adress"
            },
            new Student
            {
                Id = 2,
                StudentName = "Student2",
                Email = "student2@gmail.com",
                Address = "student2adress"
            },
            };
        }
    }
}
