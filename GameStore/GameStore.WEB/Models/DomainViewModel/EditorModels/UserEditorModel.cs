using System.Collections.Generic;
using System.Web.Mvc;

namespace GameStore.WEB.Models.DomainViewModel.EditorModels
{
    public class UserEditorModel
    {
        public string Login { get; set; }
        public UserEditModel UserModel { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}