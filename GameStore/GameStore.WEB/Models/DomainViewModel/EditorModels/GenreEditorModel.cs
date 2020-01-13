using System.Collections.Generic;
using System.Web.Mvc;

namespace GameStore.WEB.Models.DomainViewModel.EditorModels
{
    public class GenreEditorModel
    {
        public GenreViewModel Genre { get; set; }
        public List<SelectListItem> ParentGenres { get; set; }
    }
}