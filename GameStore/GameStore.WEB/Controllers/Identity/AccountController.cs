using AutoMapper;
using GameStore.BLL.Interfaces.Identity;
using GameStore.Domain.Entities.Identity;
using GameStore.WEB.Auth.Interfaces;
using GameStore.WEB.Controllers.Tools;
using GameStore.WEB.Models.IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WEB.Controllers.Identity
{
    public class AccountController : Controller
    {
        private const string BasketSession = "basket";

        private readonly IAuthentication _auth;
        private readonly IIdentityService _identityService;

        private readonly Dictionary<BanOptions, DateTime> _banOptions;

        public AccountController(IAuthentication auth, IIdentityService identityService)
        {
            _auth = auth;
            _identityService = identityService;

            _banOptions = new Dictionary<BanOptions, DateTime>
            {
                {BanOptions.OneHour, DateTime.UtcNow.AddHours(1)},
                {BanOptions.OneDay, DateTime.UtcNow.AddDays(1)},
                {BanOptions.OneWeek, DateTime.UtcNow.AddDays(7)},
                {BanOptions.OneMonth, DateTime.UtcNow.AddMonths(1)},
                {BanOptions.Permanent, DateTime.MaxValue}
            };
        }

        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = _auth.Login(loginModel.Login, loginModel.Password, loginModel.IsPersistent);

                if (user != null)
                {
                    return RedirectToAction("Index", "Game");
                }

                ModelState.AddModelError("Password", Resources.Account.AccountResource.AuthError);
            }

            return View(loginModel);
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerModel)
        {
            AccountValidation(registerModel);

            if (ModelState.IsValid)
            {
                var user = Mapper.Map<RegisterViewModel, User>(registerModel);

                if (registerModel.IsPublisher)
                {
                    _identityService.RegisterPublisher(user, registerModel.CompanyName);
                }

                _identityService.Register(user);

                _auth.Login(user.Login);

                return RedirectToAction("Index", "Game");
            }

            return View(registerModel);
        }

        public ActionResult Logout()
        {
            _auth.LogOut();

            Session[BasketSession] = null;

            return RedirectToAction("Index", "Game");
        }

        public ActionResult BanComments(string login, BanOptions options)
        {
            var user = _identityService.GetUser(login);

            if (user != null)
            {
                user.IsBanned = true;
                user.BanExpires = _banOptions[options];

                _identityService.Update(user);
            }

            return RedirectToAction("Index", "Game");
        }

        public ActionResult ChangeCulture(string lang)
        {
            var returnUrl = Request.UrlReferrer.AbsolutePath;

            var cultures = new List<string>() {"ru", "en",};

            if (!cultures.Contains(lang))
            {
                lang = "en";
            }

            var cookie = Request.Cookies["lang"];

            if (cookie != null)
            {
                cookie.Value = lang;
            }
            else
            {
                cookie = new HttpCookie("lang")
                {
                    HttpOnly = false,
                    Value = lang,
                    Expires = DateTime.Now.AddYears(1)
                };
            }

            Response.Cookies.Add(cookie);

            return Redirect(returnUrl);
        }

        private void AccountValidation(RegisterViewModel registerModel)
        {
            if (_identityService.GetUser(registerModel.Login) != null)
            {
                ModelState.AddModelError("Login", Resources.Account.AccountResource.LoginExistsError);
            }

            if (registerModel.IsPublisher)
            {
                if (registerModel.CompanyName == string.Empty)
                {
                    ModelState.AddModelError("CompanyName", Resources.Account.AccountResource.CompanyNameRequired);
                }

                if (registerModel.CompanyName.Count() < 3)
                {
                    ModelState.AddModelError("CompanyName", Resources.Account.AccountResource.CompanyNameMinError);
                }

                if (_identityService.CompanyNameValidation(registerModel.CompanyName))
                {
                    ModelState.AddModelError("CompanyName", Resources.Account.AccountResource.CompanyNameExistsError);
                }
            }
        }
    }
}