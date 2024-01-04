
using Microsoft.EntityFrameworkCore;

namespace StudentApp.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CollegeDBContext _dbContext;
        public StudentRepository(CollegeDBContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<int> CreateAsync(Student student)
        {
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var studentToDelete = await _dbContext.Students.Where(student => student.Id == id).FirstOrDefaultAsync();
            if (studentToDelete == null)
                throw new ArgumentNullException($"No student found with id: {id}");

            _dbContext.Students.Remove(studentToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _dbContext.Students.Where(student => student.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Student> GetByNameAsync(string name)
        {
            return await _dbContext.Students.Where(student => student.StudentName.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync(Student student)
        {
            var studentToUpdate = await _dbContext.Students.Where(student => student.Id == student.Id).FirstOrDefaultAsync();
            if (student == null)
                throw new ArgumentNullException($"No student found with id: {student.Id}");

            studentToUpdate.StudentName = student.StudentName;
            studentToUpdate.Email = student.Email;
            studentToUpdate.Address = student.Address;
            studentToUpdate.DOB = student.DOB;

            await _dbContext.SaveChangesAsync();
            return student.Id;
        }
    }
}
