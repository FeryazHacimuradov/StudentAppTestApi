namespace StudentApp.Models
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>(){ new Student
            {
                Id = 1,
                Name = "Student1",
                Email = "student1@gmail.com",
                Address = "student1adress"
            },
            new Student
            {
                Id = 2,
                Name = "Student2",
                Email = "student2@gmail.com",
                Address = "student2adress"
            },
        };
    }
}
