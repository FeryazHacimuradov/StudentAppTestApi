using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentApp.Data;
using StudentApp.Data.Repository;
using StudentApp.Models;

namespace StudentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Student> _studentRepository;

        public StudentController(ILogger<StudentController> logger, IMapper mapper, ICollegeRepository<Student> studentRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            var studentDTOData = _mapper.Map<List<StudentDTO>>(students);

            return Ok(studentDTOData);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetAllStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            if (id <= 0)
                return BadRequest();

            var student = await _studentRepository.GetByIdAsync(student => student.Id == id);

            if(student == null)
                return NotFound($"The student with id: {id} not found.");

            var studentDTO = _mapper.Map<StudentDTO>(student);

            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Student>> GetStudentByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var student = await _studentRepository.GetByNameAsync(student => student.StudentName.ToLower().Contains(name.ToLower()));

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

            var studentAfterCreation = await _studentRepository.CreateAsync(student);

            dto.Id = studentAfterCreation.Id;


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

            var existingStudent = await _studentRepository.GetByIdAsync(student => student.Id == dto.Id, true);

            if(existingStudent == null) 
                return NotFound();

            var newRecord = _mapper.Map<Student>(dto);

            await _studentRepository.UpdateAsync(newRecord);

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

            var existingStudent = await _studentRepository.GetByIdAsync(student => student.Id == id, true);

            if (existingStudent == null)
                return NotFound();

            var studentDTO = _mapper.Map<StudentDTO>(existingStudent);

            patchDocument.ApplyTo(studentDTO, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent = _mapper.Map<Student>(studentDTO);

            await _studentRepository.UpdateAsync(existingStudent);

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

            var student = await _studentRepository.GetByIdAsync(student => student.Id == id);

            if (student == null)
                return NotFound($"The student with id {id} not found.");

            await _studentRepository.DeleteAsync(student);

            return Ok(true);
        }
    }
}
