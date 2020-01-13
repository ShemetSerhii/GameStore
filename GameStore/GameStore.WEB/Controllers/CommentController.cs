using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Identity;
using GameStore.Domain.Entities;
using GameStore.WEB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GameStore.WEB.Models.DomainViewModel;

namespace GameStore.WEB.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IGameService _gameService;
        private readonly IIdentityService _identityService;

        public CommentController(ICommentService commentService, IGameService gameService, IIdentityService identityService)
        {
            _commentService = commentService;
            _gameService = gameService;
            _identityService = identityService;
        }

        [HttpGet]
        public ViewResult GetComments(string key, CommentViewModel formModel = null, string id = null, string quoted = null)
        {
            var comments = _commentService.GetAllCommentsByGame(key);

            var commentsView = Mapper.Map<IEnumerable<Comment>, List<CommentViewModel>>(comments.OrderBy(x => x.ParentId));

            var game = _gameService.GetGame(key);

            if (game != null)
            {
                ViewBag.GameName = game.Name;
            }

            if (quoted != null)
            {
                ViewBag.IsQuoted = quoted;
            }

            ViewBag.FormModel = formModel;
            ViewBag.Id = id;

            return View("GetAllComments", commentsView);
        }

        public PartialViewResult NewComment()
        {
            return PartialView();
        }

        public ActionResult AddComment(string key, int id, CommentViewModel commentViewModel,  string quoted = null)
        {
            if (_identityService.IsBanned(ControllerContext.HttpContext.User.ToString()))
            {
                ModelState.AddModelError("BanMessage", Resources.Comment.CommentResource.BanMessage);
            }

            if (ModelState.IsValid)
            {
                var comment = Mapper.Map<CommentViewModel, Comment>(commentViewModel);

                comment.Name = ControllerContext.HttpContext.User.ToString();

                SetupParentIdAndOrder(key, comment, id.ToString());

                if (quoted != string.Empty)
                {
                    comment.IsQuoted = true;
                }

                _commentService.AddComment(comment);

                return RedirectToAction("Details", "Game", new { key = key });
            }
           
            var viewModel = new GameCommentViewModel
                    {CommentModel = commentViewModel, Id = id.ToString(), Quoted = quoted};

            return null;
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public PartialViewResult DeleteModalWindow()
        {
            return PartialView("Delete");
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Delete(string commentId, string gameId, string crossProperty)
        {
            var game = _gameService.GetGameByInterimProperty(int.Parse(gameId), crossProperty);
            var gameKey = game.Key;

            if (int.TryParse(commentId, out int id))
            {
                _commentService.Delete(id);

                return RedirectToAction("GetComments", new { key = gameKey });
            }

            return RedirectToAction("GetComments", new { key = gameKey });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public ViewResult Ban(string user)
        {
            return View("Ban", null, user);
        }

        private void SetupParentIdAndOrder(string key, Comment comment, string id)
        {
            const string emptyId = "0";
            int count;

            var game = _gameService.GetGame(key);

            if (id == emptyId)
            {
                comment.ParentId = null;
                count = _commentService
                    .GetAllCommentsByGame(key).Count(x => x.ParentId == null);
            }
            else
            {
                comment.ParentComment = _commentService.GetComment(int.Parse(id));
                comment.ParentId = int.Parse(id);
                count = _commentService
                    .GetAllCommentsByGame(key).Count(x => x.ParentId == int.Parse(id));
            }

            if (game.CrossProperty != null)
            {
                comment.CrossProperty = game.CrossProperty;
                comment.GameId = null;
            }
            else
            {
                comment.GameId = game.Id;
            }
         
            comment.Order = count + 1;
        }
    }
}