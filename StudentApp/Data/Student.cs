using StudentApp.Models.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Data
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Student name is required!")]
        [StringLength(30)]
        public string StudentName { get; set; }
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }
        [Range(16, 35)]
        public int Age { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [DateCheck]
        public DateTime DOB { get; set; }
    }
}
