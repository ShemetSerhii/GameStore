using GameStore.WEB.Models.DomainViewModel.Identity;
using System.Collections.Generic;

namespace GameStore.WEB.Models.DomainViewModel.EditorModels
{
    public class UserEditModel
    {
        public int Id { get; set; }
        public bool IsBanned { get; set; }
        public string BanExpires { get; set; }

        public virtual ICollection<RoleViewModel> Roles { get; set; }

        public UserEditModel()
        {
            Roles = new List<RoleViewModel>();
        }
    }
}