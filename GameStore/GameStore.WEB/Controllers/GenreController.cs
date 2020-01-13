using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.Domain.Entities;
using GameStore.WEB.Models;
using GameStore.WEB.Models.DomainViewModel.EditorModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace GameStore.WEB.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public ViewResult Index()
        {
            var genres = _genreService.GetAll();

            var genresView = Mapper.Map<IEnumerable<Genre>, List<GenreViewModel>>(genres);

            GenreLanguageSelection(genres, genresView);

            return View(genresView);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public ViewResult Create()
        {
            var genreModel = new GenreEditorModel
            {
                ParentGenres = CreateSelectList(),
            };

            return View(genreModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(GenreEditorModel genreEditorModel, string[] parentGenre)
        {
            if (_genreService.Get(genreEditorModel.Genre.Name) != null)
            {
                ModelState.AddModelError("Name", Resources.Genre.GenreResource.NameExistsError);
            }

            if (ModelState.IsValid)
            {
                var genre = Mapper.Map<GenreViewModel, Genre>(genreEditorModel.Genre);

                var parent = _genreService.Get(parentGenre[0]);

                if (parent != null)
                {
                    genre.ParentId = parent.Id;
                }

                SetTranslate(genre.GenreTranslates, genreEditorModel.Genre);

                _genreService.Create(genre);

                return RedirectToAction("Index");
            }

            var genreModel = new GenreEditorModel
            {
                ParentGenres = CreateSelectList(),
                Genre = genreEditorModel.Genre,
            };

            return View(genreModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Manager")]
        public ViewResult Edit(string name)
        {
            var genre = _genreService.Get(name);

            var genreView = Mapper.Map<Genre, GenreViewModel>(genre);

            string[] parentGenre = null;

            if (genreView.Parent != null)
            {
                parentGenre = new[] {genreView.Parent.Name};
            }

            var genreModel = new GenreEditorModel
            {
                ParentGenres = CreateSelectList(parentGenre),
                Genre = genreView,
            };

            return View(genreModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(GenreEditorModel genreEditorModel, string[] parentGenre)
        {
            if (_genreService.Get(genreEditorModel.Genre.Name) != null &&
                _genreService.Get(genreEditorModel.Genre.Name).Id != genreEditorModel.Genre.Id)
            {
                ModelState.AddModelError("Name", Resources.Genre.GenreResource.NameExistsError);
            }

            if (ModelState.IsValid)
            {
                var genreView = genreEditorModel.Genre;
                var genre = _genreService.GetGenreByByInterimProperty(genreView.Id, genreView.CrossProperty);

                Mapper.Map(genreView, genre);

                var parent = _genreService.Get(parentGenre[0]);

                if (parent != null)
                {
                    genre.ParentId = parent.Id;
                    genre.Parent = parent;
                }

                SetTranslate(genre.GenreTranslates, genreEditorModel.Genre);

                _genreService.Update(genre);

                return RedirectToAction("Index");
            }

            var genreModel = new GenreEditorModel
            {
                ParentGenres = CreateSelectList(parentGenre),
                Genre = genreEditorModel.Genre,
            };

            return View(genreModel);
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(string name)
        {
            _genreService.Delete(name);

            return RedirectToAction("Index");
        }

        private List<SelectListItem> CreateSelectList(string[] parentGenre = null)
        {
            var genres = _genreService.GetAll().ToList();
            genres.Insert(0, new Genre());

            var selectList = new SelectList(genres, "Name", "Name").ToList();

            if (parentGenre == null)
            {
                selectList[0].Selected = true;
            }
            else
            {
                selectList.SingleOrDefault(x => parentGenre.Contains(x.Text)).Selected = true;
            }

            return selectList;
        }

        private void SetTranslate(ICollection<GenreTranslate> genreTranslates, GenreViewModel genreView)
        {
            if (!genreTranslates.Any())
            {
                genreTranslates.Add(new GenreTranslate());
            }

            foreach (var translate in genreTranslates)
            {
                translate.Name = genreView.NameRu;
                translate.Language = "ru-RU";
            }
        }

        private void GenreLanguageSelection(IEnumerable<Genre> genres, IEnumerable<GenreViewModel> genresView)
        {
            var cultureName = Thread.CurrentThread.CurrentCulture.Name;

            foreach (var genre in genres)
            {
                var genreTranslates = genre.GenreTranslates.SingleOrDefault(x => x.GenreId == genre.Id && x.Language == cultureName);

                if (genreTranslates != null && genreTranslates.Name != null)
                {
                    genresView.SingleOrDefault(x => x.Id == genreTranslates.GenreId).NameTranslate =
                        genreTranslates.Name;
                }
            }
        }
    }
}