namespace StudentApp.Models
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>(){ new Student
            {
                Id = 1,
                Name = "Fedya",
                Email = "fedya1@gmail.com",
                Address = "student1adress"
            },
            new Student
            {
                Id = 2,
                Name = "Toti",
                Email = "toti2@gmail.com",
                Address = "student2adress"
            },
        };
    }
}
