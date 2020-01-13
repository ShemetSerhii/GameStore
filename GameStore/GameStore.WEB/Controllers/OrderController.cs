using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.Domain.Entities;
using GameStore.WEB.Filters;
using GameStore.WEB.Models;
using GameStore.WEB.Services.Payment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace GameStore.WEB.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private const string SessionName = "basket";

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Auth]
        public ViewResult GetBasket()
        {
            var basket = Session[SessionName] as Order;

            if (basket == null || basket.OrderDetails.Count == 0)
            {
                var emptyBasket = new List<OrderDetailViewModel>();
                return View(emptyBasket);
            }
            var basketView = Mapper.Map<Order, OrderViewModel>(basket);

            return View(basketView.OrderDetails);
        }

        [HttpGet]
        [Auth]
        public ActionResult AddIntoBasket(string key)
        {
            var game = _orderService.GetGame(key);
            var order = Session[SessionName] as Order;
            var orderDetail = new OrderDetail();

            if (order == null)
            {
                order = new Order
                {
                    CustomerId = ControllerContext.HttpContext.User.ToString(),
                    OrderDate = DateTime.Now

                };
                Session[SessionName] = order;
            }

            if (order.OrderDetails.Select(o => o.Game.Key).Contains(key))
            {
                orderDetail = order.OrderDetails.SingleOrDefault(x => x.Game.Key == key);
                orderDetail.Quantity += 1;
            }
            else
            {
                orderDetail = new OrderDetail
                {
                    CrossKey = game.Key,
                    Game = game,
                    GameId = null,
                    Price = game.Price,
                    Quantity = 1,
                    OrderId = order.Id,
                    Order = order
                };

                if (game.Id != 0)
                {
                    orderDetail.GameId = game.Id;
                }

                order.OrderDetails.Add(orderDetail);
            }

            Session[SessionName] = order;
            return RedirectToAction("GetBasket");
        }

        [Auth]
        public ActionResult Delete(int gameId)
        {
            var order = Session["basket"] as Order;
            var orderDetail = order.OrderDetails.SingleOrDefault(x => x.GameId == gameId);

            order.OrderDetails.Remove(orderDetail);
            Session[SessionName] = order;

            return RedirectToAction("GetBasket");
        }

        public ActionResult OrderReduction(int gameId)
        {
            var order = Session["basket"] as Order;
            var orderDetail = order.OrderDetails.SingleOrDefault(x => x.GameId == gameId);

            orderDetail.Quantity -= 1;

            return RedirectToAction("GetBasket");
        }

        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public PartialViewResult GetTotalNumbers()
        {
            var order = Session[SessionName] as Order;
            var count = 0;

            if (order != null)
            {
                count = order.OrderDetails.Select(ord => (int)ord.Quantity).ToList().Sum();
                return PartialView("GetTotalNumbers", count.ToString());
            }

            return PartialView("GetTotalNumbers", count.ToString());
        }

        [HttpGet]
        [Auth]
        public ActionResult MakeOrder()
        {
            var order = Session[SessionName] as Order;

            if (order == null)
            {
                return RedirectToAction("GetBasket");
            }

            var checkOrder = _orderService.GetLastOrder(order.CustomerId);

            if (checkOrder == null || !checkOrder.OrderDate.ToString().Equals(order.OrderDate.ToString()))
            {
                CreateOrder(order);
            }

            SetupGames(order);

            var orderView = Mapper.Map<Order, OrderViewModel>(order);

            return View(orderView.OrderDetails);
        }

        [Auth]
        public ActionResult ChoosePayment(PaymentEnum paymentEnum)
        {
            var order = Session[SessionName] as Order;

            if (order == null)
            {
                return RedirectToAction("GetBasket");
            }

            _orderService.ChangeOrderStatus(order);

            var payment = new Payment(order);

            object result = payment.MakePayment(paymentEnum);

            if (result is string viewName)
            {
                var orderView = Mapper.Map<Order, OrderViewModel>(order);               

                return View(viewName, orderView);
            }

            if (result is MemoryStream stream)
            {
                var strPdfFileName = string.Format("Invoice-" + order.Id + "-" + order.OrderDate.ToShortDateString() + ".pdf");

                Session[SessionName] = null;

                return File(stream, "application/pdf", strPdfFileName);
            }

            return new EmptyResult();
        }

        public ActionResult CompleteOrder()
        {
            Session[SessionName] = null;

            return RedirectToAction("GetBasket");
        }

        private void CreateOrder(Order order)
        {
            foreach (var orderDetail in order.OrderDetails)
            {
                orderDetail.Game = null;
            }

            _orderService.CreateOrder(order);
        }

        private void SetupGames(Order order)
        {
            foreach (var detail in order.OrderDetails)
            {
                detail.Game = _orderService.GetGame(detail.CrossKey);
            }
        }
    }
}