using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore.WEB.Models
{
    public class GameViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Game.GameResource), ErrorMessageResourceName = "KeyRequiredError")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Resources.Game.GameResource), ErrorMessageResourceName = "KeyMaxError")]
        [Display(ResourceType = typeof(Resources.Game.GameResource), Name = "Key")]
        public string Key { get; set; }

        public string CrossProperty { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Game.GameResource), ErrorMessageResourceName = "NameRequiredError")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resources.Game.GameResource), ErrorMessageResourceName = "NameMinError")]
        [Display(ResourceType = typeof(Resources.Order.OrderResource), Name = "GameName")]
        public string Name { get; set; }

        public string NameRU { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Game.GameResource), ErrorMessageResourceName = "DescriptionRequiredError")]
        public string Description { get; set; }

        public string DescriptionRU { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Game.GameResource), ErrorMessageResourceName = "PriceRequiredError")]
        [Range(0.01, int.MaxValue, ErrorMessageResourceType = typeof(Resources.Game.GameResource), ErrorMessageResourceName = "PriceMinError")]
        [Display(ResourceType = typeof(Resources.Order.OrderResource), Name = "Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Game.GameResource), ErrorMessageResourceName = "UnitsInStockRequiredError")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Resources.Game.GameResource), ErrorMessageResourceName = "UnitsInStockMinError")]
        [Display(ResourceType = typeof(Resources.Game.GameResource), Name = "UnitsInStock")]
        public short UnitsInStock { get; set; }

        [Display(ResourceType = typeof(Resources.Game.GameResource), Name = "Discontinued")]
        public bool Discontinued { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Game.GameResource), ErrorMessageResourceName = "DatePublicationRequiredError")]
        [Display(ResourceType = typeof(Resources.Game.GameResource), Name = "DatePublication")]
        public string DatePublication { get; set; }

        public int? PublisherId { get; set; }

        [Display(ResourceType = typeof(Resources.Game.GameResource), Name = "Publisher")]
        public PublisherViewModel Publisher { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }

        public ICollection<GenreViewModel> Genres { get; set; }

        public ICollection<PlatformTypeViewModel> PlatformTypes { get; set; }

        public GameViewModel()
        {
            Comments = new List<CommentViewModel>();
            Genres = new List<GenreViewModel>();
            PlatformTypes = new List<PlatformTypeViewModel>();
        }
    }
}