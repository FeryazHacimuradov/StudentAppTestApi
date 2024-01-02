using StudentApp.Models.Validators;
using System.ComponentModel.DataAnnotations;

namespace StudentApp.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Student name is required!")]
        [StringLength(30)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }
        [Range(16, 35)]
        public int Age { get; set; }
        [Required]
        public string Address { get; set; }
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [DateCheck]
        public DateTime AdmissionDate { get; set; }


    }
}
