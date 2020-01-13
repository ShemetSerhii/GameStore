using GameStore.BLL.FilterPipeline.Concrete;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GameStore.WEB.Models.FilterModel
{
    public class FilterViewModel
    {
        public string[] GenresName { get; set; }
        public string[] PlatformTypesName { get; set; }
        public string[] PublishersName { get; set; }
        public List<SelectListItem> Genres { get; set; }
        public List<SelectListItem> PlatformTypes { get; set; }
        public List<SelectListItem> Publishers { get; set; }
        public List<SelectListItem> PublicationTimes { get; set; }
        public SortFilter SortingType { get; set; }
        public List<SelectListItem> SortingTypesList { get; set; }
        public decimal PriceRangeFrom { get; set; }
        public decimal PriceRangeTo { get; set; }
        public FiltersEnum PublicationTime { get; set; }
        public string PartOfName { get; set; }
    }
}