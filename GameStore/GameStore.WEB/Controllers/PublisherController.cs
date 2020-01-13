using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.Domain.Entities;
using GameStore.WEB.Models;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace GameStore.WEB.Controllers
{
    public class PublisherController : Controller
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        public ViewResult Index()
        {
            var publishers = _publisherService.GetAll();
            var publisherViews = Mapper.Map<IEnumerable<Publisher>, List<PublisherViewModel>>(publishers);

            return View(publisherViews);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public ViewResult Create()
        {
            return View(new PublisherViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(PublisherViewModel publisherView)
        {
            if (_publisherService.GetByName(publisherView.CompanyName) != null)
            {
                ModelState.AddModelError("CompanyName", Resources.Publisher.PublisherResource.CompanyNameExistsError);
            }

            if (ModelState.IsValid)
            {
                var publisher = Mapper.Map<PublisherViewModel, Publisher>(publisherView);

                _publisherService.Create(publisher);

                return RedirectToAction("Index");
            }
            else
            {
                return View(publisherView);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager, Publisher")]
        public ActionResult Edit(string companyName)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Manager"))
            {
                var publisher = _publisherService.GetByName(companyName);
                var publisherView = Mapper.Map<Publisher, PublisherViewModel>(publisher);

                return View(publisherView);
            }

            if (PublisherAccess(companyName))
            {
                var publisher = _publisherService.GetByName(companyName);
                var publisherView = Mapper.Map<Publisher, PublisherViewModel>(publisher);

                return View(publisherView);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager, Publisher")]
        public ActionResult Edit(PublisherViewModel publisherView)
        {
            if (_publisherService.GetByName(publisherView.CompanyName) != null && _publisherService.GetByName(publisherView.CompanyName).Id != publisherView.Id)
            {
                ModelState.AddModelError("CompanyName", Resources.Publisher.PublisherResource.CompanyNameExistsError);
            }

            if (ModelState.IsValid)
            {
                var publisher = _publisherService.GetByInterimProperty(publisherView.Id, publisherView.CrossProperty);

                Mapper.Map(publisherView, publisher);

                _publisherService.Update(publisher);

                return RedirectToAction("Index");
            }

            return View(publisherView);
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(string companyName)
        {
            _publisherService.Delete(companyName);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Details(string companyName)
        {
            companyName = Decoder(companyName);

            var publisher = _publisherService.GetByName(companyName);

            var publisherView = Mapper.Map<Publisher, PublisherViewModel>(publisher);

            return View(publisherView);
        }

        private bool PublisherAccess(string companyName)
        {
            return _publisherService.PublisherAccess(companyName, ControllerContext.HttpContext.User.ToString());
        }

        private string Decoder(string companyName)
        {
            string result = companyName;
            result = result.Replace("$$point$$", ".");
            result = result.Replace("$$and$$", "&");

            return result;
        }
    }
}