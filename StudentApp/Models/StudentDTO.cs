using StudentApp.Models.Validators;
using System.ComponentModel.DataAnnotations;

namespace StudentApp.Models
{
    public class StudentDTO
    {
        public int Id { get; set; }

        [Required]
        public string StudentName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        //[DateCheck]
        //public DateTime AdmissionDate { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
