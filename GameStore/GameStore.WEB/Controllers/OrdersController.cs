using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.Domain.Entities;
using GameStore.WEB.Models;
using GameStore.WEB.Models.DomainViewModel.EditorModels;
using GameStore.WEB.Models.OrderHistoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.WEB.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IShipperService _shipperService;

        public OrdersController(IOrderService orderService, IShipperService shipperService)
        {
            _orderService = orderService;
            _shipperService = shipperService;
        }

        public ViewResult Index()
        {
            var defaultTimeFrom = DateTime.UtcNow.AddDays(-30);
            var defaultTimeTo = DateTime.UtcNow;

            var orders = _orderService.GetOrdersLog(defaultTimeFrom, defaultTimeTo);

            var orderView = Mapper.Map<IEnumerable<Order>, List<OrderViewModel>>(orders);

            var orderHistoryModel = new OrderHistoryModel
            {
                OrderModels = orderView,
                TimeFrom = defaultTimeFrom.ToLocalTime().ToShortDateString(),
                TimeTo = defaultTimeTo.ToLocalTime().ToShortDateString()
            };

            return View(orderHistoryModel);
        }

        public ViewResult Orders(string TimeFrom, string TimeTo)
        {
            if (TimeFrom == null || TimeTo == null)
            {
                ModelState.AddModelError("TimeFrom", Resources.Orders.OrdersResource.DateRequiredError);
            }

            if (ModelState.IsValid)
            {
                var orders = _orderService.GetOrdersLog(DateTime.Parse(TimeFrom), DateTime.Parse(TimeTo));

                var orderView = Mapper.Map<IEnumerable<Order>, List<OrderViewModel>>(orders);

                var orderHistoryModel = new OrderHistoryModel
                {
                    OrderModels = orderView,
                    TimeFrom = TimeFrom,
                    TimeTo = TimeTo
                };

                return View("Index", orderHistoryModel);
            }

            var orderModel = new OrderHistoryModel
            {
                TimeFrom = TimeFrom,
                TimeTo = TimeTo
            };

            return View("Index", orderModel);
        }

        [HttpGet]
        public ViewResult History()
        {
            var defaultTimeFrom = DateTime.UtcNow.AddDays(-60);
            var defaultTimeTo = DateTime.UtcNow.AddDays(-30);

            var orders = _orderService.GetOrdersLog(defaultTimeFrom, defaultTimeTo);

            var orderView = Mapper.Map<IEnumerable<Order>, List<OrderViewModel>>(orders);

            var orderHistoryModel = new OrderHistoryModel
            {
                OrderModels = orderView,
                TimeFrom = defaultTimeFrom.ToLocalTime().ToShortDateString(),
                TimeTo = defaultTimeTo.ToLocalTime().ToShortDateString()
            };

            return View(orderHistoryModel);
        }

        [HttpGet]
        public ViewResult GetHistoryLog(string TimeFrom, string TimeTo)
        {
            if (TimeFrom == null || TimeTo == null)
            {
                ModelState.AddModelError("TimeFrom", Resources.Orders.OrdersResource.DateRequiredError);
            }

            if (ModelState.IsValid)
            {
                var orders = _orderService.GetOrdersLog(DateTime.Parse(TimeFrom), DateTime.Parse(TimeTo));

                var orderView = Mapper.Map<IEnumerable<Order>, List<OrderViewModel>>(orders);

                var orderHistoryModel = new OrderHistoryModel
                {
                    OrderModels = orderView,
                    TimeFrom = TimeFrom,
                    TimeTo = TimeTo
                };

                return View("History", orderHistoryModel);
            }

            var orderModel = new OrderHistoryModel();

            orderModel.TimeFrom = TimeFrom;
            orderModel.TimeTo = TimeTo;

            return View("History", orderModel);
        }

        [HttpGet]
        public ViewResult GetOrderDetails(string crossId)
        {
            var order = _orderService.GetOrderByInterimProperty(crossId);

            var orderView = Mapper.Map<Order, OrderViewModel>(order);

            return View(orderView);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            var order = _orderService.Get(id);

            var orderView = Mapper.Map<Order, OrderEditModel>(order);

            var orderStatus = new string[] { };

            if (order.OrderStatus != null)
            {
                 orderStatus = new[] { order.OrderStatus };
            }

            var shipper = new[] {orderView.Shipper};
         
            var editorModel = new OrderEditorModel
            {
                OrderModel = orderView,
                Shippers = CreateSelectList(shipper),
                OrderStatuses = CreateListStatuses(orderStatus)
            };

            return View(editorModel);
        }

        [HttpPost]
        public ActionResult Edit(OrderEditorModel editorModel, string[] selectShipper, string[] selectStatus)
        {
            if (ModelState.IsValid)
            {
                var order = _orderService.Get(editorModel.OrderModel.Id);

                Mapper.Map(editorModel.OrderModel, order);

                order.Shipper = selectShipper[0];
                order.OrderStatus = selectStatus[0];

                _orderService.Update(order);

                return RedirectToAction("Index");
            }

            editorModel.Shippers = CreateSelectList(selectShipper);

            return View(editorModel);
        }

        public ActionResult Cancel(int id)
        {
            _orderService.Delete(id);

            return RedirectToAction("Index");
        }

        private List<SelectListItem> CreateSelectList(string[] selectedShipper = null)
        {
            var shippers = _shipperService.GetAll();

            var list = new SelectList(shippers, "CompanyName", "CompanyName").ToList();

            if (selectedShipper != null)
            {
                var select = list.Where(rol => selectedShipper.Contains(rol.Value));

                foreach (var item in select)
                {
                    item.Selected = true;
                }
            }

            return list;
        }

        private List<SelectListItem> CreateListStatuses(string[] selectedStatutes = null)
        {
            var statuses = _orderService.Statuses.Values.ToList();

            var list = new SelectList(statuses).ToList();
            list[0].Disabled = true;

            if (selectedStatutes.Any())
            {
                var select = list.SingleOrDefault(status => selectedStatutes.Contains(status.Text));

                select.Selected = true;
            }

            return list;
        }
    }
}