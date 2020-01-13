using AutoMapper;
using GameStore.BLL.Interfaces.Identity;
using GameStore.Domain.Entities.Identity;
using GameStore.WEB.Models.DomainViewModel.EditorModels;
using GameStore.WEB.Models.DomainViewModel.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.WEB.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IRoleService _roleService;

        public AdminController(IIdentityService identityService, IRoleService roleService)
        {
            _identityService = identityService;
            _roleService = roleService;
        }

        public ActionResult Index()
        {
            var users = _identityService.GetAll();

            var usersView = Mapper.Map<IEnumerable<User>, List<UserViewModel>>(users);

            return View(usersView);
        }

        [HttpGet]
        public ViewResult Edit(string login)
        {
            var user = _identityService.GetUser(login);

            var userView = Mapper.Map<User, UserEditModel>(user);

            var userRoles = user.Roles.Select(rol => rol.Name).ToArray();

            var editorModel = new UserEditorModel
            {
                Login = user.Login,
                UserModel = userView,
                Roles = CreateSelectList(userRoles)
            };

            return View(editorModel);
        }

        [HttpPost]
        public ActionResult Edit(UserEditorModel editorModel, string[] rolesSelected)
        {
            if (ModelState.IsValid)
            {
                var user = _identityService.GetUser(editorModel.Login);

                Mapper.Map(editorModel.UserModel, user);

                user.Roles.Clear();

                foreach (var role in _roleService.GetAll().Where(rol => rolesSelected.Contains(rol.Name)))
                {
                    user.Roles.Add(role);
                }

                _identityService.Update(user);

                return RedirectToAction("Index");
            }

            return View(editorModel);
        }

        public ViewResult UserDetails(string login)
        {
            var user = _identityService.GetUser(login);

            var userView = Mapper.Map<User, UserViewModel>(user);

            return View(userView);
        }

        private List<SelectListItem> CreateSelectList(string[] userRoles = null)
        {
            var roles = _roleService.GetAll();

            var list = new SelectList(roles, "Name", "Name").ToList();

            if (userRoles != null)
            {
                var select = list.Where(rol => userRoles.Contains(rol.Value));

                foreach (var item in select)
                {
                    item.Selected = true;
                }
            }

            return list;
        }
    }
}