using AutoMapper;
using GameStore.BLL.FilterPipeline.Abstract;
using GameStore.BLL.FilterPipeline.Concrete;
using GameStore.BLL.FilterPipeline.Model;
using GameStore.BLL.Interfaces;
using GameStore.Domain.Entities;
using GameStore.WEB.Models;
using GameStore.WEB.Models.FilterModel;
using GameStore.WEB.Models.PaginationModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GameStore.WEB.Models.DomainViewModel;
using GameStore.WEB.Resources.Game;

namespace GameStore.WEB.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IPublisherService _publisherService;
        private readonly Pipeline<IEnumerable<Game>> _pipeline;

        public GameController(IGameService gameService, IPublisherService publisherService, GameSelectionPipeline pipeline)
        {
            _gameService = gameService;
            _publisherService = publisherService;
            _pipeline = pipeline;
        }

        public async Task<ViewResult> Index(int page = 1, string pageSize = "10")
        {
            var games = await _gameService.GetAll();

            var indexView = CreateGameIndexViewModel(games, page, pageSize);

            return View(indexView);
        }

        public ActionResult Filter(FilterViewModel filterModel, int page = 1, string pageSize = "10")
        {
            var filterModelDto = Mapper.Map<FilterViewModel, FilterModelDTO>(filterModel);
            _pipeline.FilterRegister(filterModelDto);

            var games = _pipeline.Process();

            var indexView = CreateGameIndexViewModel(games, page, pageSize);

            return View("Index", indexView);
        }
  
        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public ViewResult New()
        {
            CreateSelectLists();

            return View("Create", new GameViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult New(GameViewModel gameView, string[] publisherSelect, string[] genresSelect, string[] platformTypesSelect)
        {
            if (_gameService.GetByKey(gameView.Key) != null)
            {
                ModelState.AddModelError("Key", GameResource.KeyExistsError);
            }

            GeneralValidation(gameView, publisherSelect, genresSelect, platformTypesSelect);

            if (ModelState.IsValid)
            {
                var game = Mapper.Map<GameViewModel, Game>(gameView);

                var publishers = _gameService.GetPublishers().Where(pub => publisherSelect.Contains(pub.CompanyName));
                var genres = _gameService.GetGenres().Where(gen => genresSelect.Contains(gen.Name));
                var platformTypes = _gameService.GetPlatformTypes().Where(plt => platformTypesSelect.Contains(plt.Type));

                foreach (var publisher in publishers)
                {
                    game.Publisher = publisher;
                    game.PublisherId = publisher.Id;
                }

                foreach (var genre in genres)
                {
                    game.Genres.Add(genre);
                }

                foreach (var platformType in platformTypes)
                {
                    game.PlatformTypes.Add(platformType);
                }

                game.DateAdded = DateTime.UtcNow;

                _gameService.Create(game);

                return Redirect("/games");
            }

            CreateSelectLists(genresSelect, platformTypesSelect, publisherSelect);

            return View("Create", gameView);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager, Publisher")]
        public ActionResult Update(string key)
        {
            var game = _gameService.GetByKey(key);

            if (GameEditAccess(game).Equals(new EmptyResult()))
            {
                return GameEditAccess(game);
            }

            var gameView = Mapper.Map<Game, GameViewModel>(game);

            var genresSelect = _gameService.GetGenres().Select(g => g.Name)
                .Where(x => game.Genres.Select(y => y.Name).Contains(x)).ToArray();

            var platformTypesSelect = _gameService.GetPlatformTypes().Select(plt => plt.Type)
                .Where(x => game.PlatformTypes.Select(y => y.Type).Contains(x)).ToArray();

            var publishers = new string[1];

            if (game.Publisher != null)
            {
                 publishers = _gameService.GetPublishers().Select(pub => pub.CompanyName)
                    .Where(x => game.Publisher.CompanyName == x).ToArray();
            }
               
            CreateSelectLists(genresSelect, platformTypesSelect, publishers);

            return View("Edit", gameView);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager, Publisher")]
        public ActionResult Update(GameViewModel gameView, string[] publisherSelect, string[] genresSelect, string[] platformTypesSelect)
        {
            if (_gameService.GetGame(gameView.Key) != null && _gameService.GetGame(gameView.Key).Id != gameView.Id)
            {
                ModelState.AddModelError("Key", Resources.Game.GameResource.KeyExistsError);
            }

            GeneralValidation(gameView, publisherSelect, genresSelect, platformTypesSelect);

            if (ModelState.IsValid)
            {
                var game = _gameService.GetGameByInterimProperty(gameView.Id, gameView.CrossProperty);

                Mapper.Map(gameView, game);

                game.Genres.Clear();
                game.PlatformTypes.Clear();

                foreach (var publisher in _gameService.GetPublishers().Where(pub => publisherSelect.Contains(pub.CompanyName)))
                {
                    game.Publisher = publisher;
                    game.PublisherId = publisher.Id;
                }

                foreach (var genre in _gameService.GetGenres().Where(gen => genresSelect.Contains(gen.Name)))
                {
                    game.Genres.Add(genre);
                }

                foreach (var platformType in _gameService.GetPlatformTypes().Where(plt => platformTypesSelect.Contains(plt.Type)))
                {
                    game.PlatformTypes.Add(platformType);
                }

                _gameService.UpdateGame(game);

                return RedirectToAction("Details", new { key = game.Key });
            }

            CreateSelectLists(genresSelect, platformTypesSelect, publisherSelect);

            return View("Edit", gameView);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(string key)
        {
            _gameService.DeleteGame(key);

            return Redirect("/games");
        }

        [HttpGet]
        public FileResult Download(string key)
        {
            string path = Server.MapPath("~/Files/Some Game.txt");

            FileStream fs = new FileStream(path, FileMode.Open);

            string fileType = "application/txt";
            string fileName = "Some Game.txt";

            return File(fs, fileType, fileName);
        }

        public ActionResult Details(string key, GameCommentViewModel commentView = null)
        {
            var game = _gameService.GetGame(key);

            if (game.IsDeleted && !(ControllerContext.HttpContext.User.IsInRole("Administrator") || ControllerContext.HttpContext.User.IsInRole("Moderator")))
            {
                return HttpNotFound();
            }

            var gameView = Mapper.Map<Game, GameViewModel>(game);

            var viewModel = new GameCommentViewModel
            {
                GameViewModel = gameView,
                CommentModel = commentView?.CommentModel,
                Id = commentView?.Id,
                Quoted = commentView?.Quoted
            };

            return View(viewModel);
        }

        private ActionResult GameEditAccess(Game game)
        {
            if (game.Publisher != null)
            {
                var accessResult = _publisherService.PublisherAccess(game.Publisher.CompanyName, ControllerContext.HttpContext.User.ToString());

                if (!accessResult)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }
            else
            {
                if (ControllerContext.HttpContext.User.IsInRole("Publisher"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            return new EmptyResult();
        }

        private void GeneralValidation(GameViewModel gameView, string[] publisherSelect, string[] genresSelect, string[] platformTypesSelect)
        {
            if (publisherSelect == null)
            {
                ModelState.AddModelError("publishers", Resources.Game.GameResource.PublisherRequiredError);
            }

            if (genresSelect == null)
            {
                ModelState.AddModelError("genres", Resources.Game.GameResource.GenresRequiredError);
            }

            if (platformTypesSelect == null)
            {
                ModelState.AddModelError("platformTypes", Resources.Game.GameResource.PlatformTypesRequiredError);
            }

            if (DateTime.Parse(gameView.DatePublication) > DateTime.UtcNow)
            {
                ModelState.AddModelError("DatePublication", Resources.Game.GameResource.PublicationDateExceedsError);
            }
        }

        private void CreateSelectLists(string[] genreId = null, string[] platformId = null, string[] publisherId = null)
        {
            var genres = new SelectList(_gameService.GetGenres(), "Name", "Name").ToList();
            var platformTypes = new SelectList(_gameService.GetPlatformTypes(), "Type", "Type").ToList();
            var publishers = new SelectList(_gameService.GetPublishers(), "CompanyName", "CompanyName").ToList();

            if (genreId != null && platformId != null && publisherId != null)
            {
                var select = genres.Where(x => genreId.Contains(x.Value)).ToList();
                SelectItem(select);
            }

            if (platformId != null)
            {
                var select = platformTypes.Where(x => platformId.Contains(x.Value)).ToList();
                SelectItem(select);
            }

            if (publisherId != null)
            {
                var select = publishers.Where(x => publisherId.Contains(x.Value)).ToList();
                SelectItem(select);
            }

            ViewBag.Genres = genres;
            ViewBag.PlatformTypes = platformTypes;
            ViewBag.Publishers = publishers;
        }

        private void SelectItem(List<SelectListItem> list)
        {
            foreach (var item in list)
            {
                item.Selected = true;
            }
        }

        private int PageSize(string pageSizeClient)
        {
            var games = _gameService.GetAllGames();

            var pageSize = games.Count();

            if (int.TryParse(pageSizeClient, out var result))
            {
                pageSize = result;
            }

            return pageSize;
        }

        private GameIndexViewModel CreateGameIndexViewModel(IEnumerable<Game> games, int page, string pageSizeClient)
        {
            var pageSize = PageSize(pageSizeClient);

            var gamesPerPages = _gameService.Pagination(games, page, pageSize);

            var gamesPerPagesView = Mapper.Map<IEnumerable<Game>, List<GameViewModel>>(gamesPerPages);
            var sortingDictionary = new Dictionary<string, SortFilter>
            {
                {"Most popular", SortFilter.MostPopular },
                {"Most commented", SortFilter.MostCommented },
                {"Price ASC", SortFilter.PriceASC },
                {"Price DESC", SortFilter.PriceDESC },
                {"Sort by date added", SortFilter.SortByDateAdded }
            };

            var indexView = new GameIndexViewModel
            {
                PageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = games.Count() },
                GameViewModels = gamesPerPagesView,

                FilterModel = new FilterViewModel
                {
                    Genres = new SelectList(_gameService.GetGenres(), "Name", "Name").ToList(),
                    PlatformTypes = new SelectList(_gameService.GetPlatformTypes(), "Type", "Type").ToList(),
                    Publishers = new SelectList(_gameService.GetPublishers(), "CompanyName", "CompanyName").ToList(),
                    PublicationTimes = CreateReleaseDateList(),
                    SortingTypesList = new SelectList(sortingDictionary, "value", "key").ToList(),
                }
            };

            return indexView;
        }

        private List<SelectListItem> CreateReleaseDateList()
        {
            var dictionary = new Dictionary<FiltersEnum, string>
            {
                {FiltersEnum.LastWeek, GameResource.LastWeek},
                {FiltersEnum.LastMonth, GameResource.LastMonth},
                {FiltersEnum.LastYear, GameResource.LastYear},
                {FiltersEnum.LastTwoYear, GameResource.LastTwoYear},
                {FiltersEnum.LastThreeYears, GameResource.LastThreeYears}
            };

            var list = new SelectList(dictionary, "Key", "Value").ToList();

            return list;
        }
    }
}