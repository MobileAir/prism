using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prism.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }

        public EditUserViewModel(ApplicationUser user)
        {
            this.Id = user.Id;
            this.Username = user.UserName;
            this.FirstName = user.Firstname;
            this.LastName = user.Lastname;
        }

        public EditUserViewModel()
        {
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}