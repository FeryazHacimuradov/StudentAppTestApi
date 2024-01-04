using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public StudentController(ILogger<StudentController> logger, CollegeDBContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsAsync()
        {
            var students = await _dbContext.Students.ToListAsync();
            var studentDTOData = _mapper.Map<List<StudentDTO>>(students);

            return Ok(studentDTOData);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetAllStudentById")]
        public ActionResult<Student> GetStudentById(int id)
        {
            return Ok(_dbContext.Students.Where(n => n.Id == id).FirstOrDefault());
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        public async Task<ActionResult<Student>> GetStudentByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var student = await _dbContext.Students.Where(n => n.StudentName == name).FirstOrDefaultAsync();

            if (student == null)
                return NotFound($"The student with name {name} not found.");

            var studentDTO = _mapper.Map<StudentDTO>(student);

            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> CreateStudentAsync([FromBody] StudentDTO dto)
        {
            if(dto == null)
                return BadRequest();

            Student student = _mapper.Map<Student>(dto);
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();

            dto.Id = student.Id;


            return CreatedAtRoute("GetStudentById", new { id = dto.Id}, dto);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> UpdateStudentAsync([FromBody] StudentDTO dto)
        {
            if (dto == null || dto.Id <=0)
                return BadRequest();

            var existingStudent = await _dbContext.Students.Where(s => s.Id == dto.Id).FirstOrDefaultAsync();

            if(existingStudent == null) 
                return NotFound();

            var newRecord = _mapper.Map<Student>(dto);

            _dbContext.Students.Update(newRecord);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <=0 )
                return BadRequest();

            var existingStudent = await _dbContext.Students.Where(s => s.Id == id).FirstOrDefaultAsync();

            if(existingStudent == null)
                return NotFound();

            var studentDTO = _mapper.Map<StudentDTO>(existingStudent);

            patchDocument.ApplyTo(studentDTO, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent = _mapper.Map<Student>(studentDTO);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteStudentAsync(int id)
        {
            if(id <= 0)
                return BadRequest();

            var student = await _dbContext.Students.Where(n => n.Id == id).FirstOrDefaultAsync();

            if (student == null)
                return NotFound($"The student with id {id} not found.");

            _dbContext.Students.Remove(student);

            await _dbContext.SaveChangesAsync();

            return Ok(true);
        }
    }
}
