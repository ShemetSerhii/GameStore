using System.Collections.Generic;

namespace GameStore.WEB.Models.DomainViewModel.Identity
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserViewModel> Users { get; set; }

        public RoleViewModel()
        {
            Users = new List<UserViewModel>();
        }
    }
}