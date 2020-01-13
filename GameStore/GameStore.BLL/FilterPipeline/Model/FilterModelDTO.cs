using GameStore.BLL.FilterPipeline.Concrete;

namespace GameStore.BLL.FilterPipeline.Model
{
    public class FilterModelDTO
    {
        public string[] GenresName { get; set; }
        public string[] PlatformTypesName { get; set; }
        public string[] PublishersName { get; set; }
        public SortFilter SortingType { get; set; }
        public decimal PriceRangeFrom { get; set; }
        public decimal PriceRangeTo { get; set; }
        public FiltersEnum PublicationTime { get; set; }
        public string PartOfName { get; set; }
    }
}
