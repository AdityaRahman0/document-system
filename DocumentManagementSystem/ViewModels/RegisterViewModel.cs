using Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DocumentManagementSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        public List<SelectListItem> Roles { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public int SelectedDepartments { get; set; }
    }
}