using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCoreMVCCRUD.Models
{
    public class EmployeeViewModel
    {
        public int EmpId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string Password { get; set; }
      
        public string ConfirmPassword { get; set; }
        [Required]
        public string Gender { get; set; }

        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }
    
        public string SecurityQuestion { get; set; }
      
        public string Answer { get; set; }
    }
}
