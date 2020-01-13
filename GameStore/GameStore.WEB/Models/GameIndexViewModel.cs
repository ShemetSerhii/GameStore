using GameStore.WEB.Models.FilterModel;
using GameStore.WEB.Models.PaginationModel;
using System.Collections.Generic;

namespace GameStore.WEB.Models
{
    public class GameIndexViewModel
    {
        public IEnumerable<GameViewModel> GameViewModels { get; set; }
        public PageInfo PageInfo { get; set; }
        public FilterViewModel FilterModel { get; set; }
    }
}