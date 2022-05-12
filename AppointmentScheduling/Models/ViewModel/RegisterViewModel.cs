using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduling.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please Enter your Name")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please Enter Email Address")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage ="Please Enter Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage ="The {0} Must be at least {2} Characters Long.",MinimumLength=6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="The password and Confrim Password do not Match.")]
        public string Confrimpassword { get; set; }

        [Required]
        [Display (Name ="Role Name")]
        public string RoleName { get; set; }
    }
}
