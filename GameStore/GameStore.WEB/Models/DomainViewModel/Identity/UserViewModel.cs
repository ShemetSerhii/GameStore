using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models.DomainViewModel.Identity
{
    public class UserViewModel
    {
        [Display(Name = "Id", ResourceType = typeof(Resources.Admin.AdminResource))]
        public int Id { get; set; }

        [Display(Name = "IsBanned", ResourceType = typeof(Resources.Admin.AdminResource))]
        public bool IsBanned { get; set; }

        [Display(Name = "BanExpires", ResourceType = typeof(Resources.Admin.AdminResource))]
        public DateTime BanExpires { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Admin.AdminResource))]
        public string Name { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resources.Admin.AdminResource))]
        public string Address { get; set; }

        [Display(Name = "Login", ResourceType = typeof(Resources.Admin.AdminResource))]
        public string Login { get; set; }

        [Display(Name = "Roles", ResourceType = typeof(Resources.Admin.AdminResource))]
        public virtual ICollection<RoleViewModel> Roles { get; set; }

        public UserViewModel()
        {
            Roles = new List<RoleViewModel>();
        }
    }
}