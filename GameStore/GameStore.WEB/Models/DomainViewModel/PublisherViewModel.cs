using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore.WEB.Models
{
    public class PublisherViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Publisher.PublisherResource), ErrorMessageResourceName = "CompanyNameRequiredError")]
        [Display(ResourceType = typeof(Resources.Publisher.PublisherResource), Name = "CompanyName")]
        [MaxLength(40, ErrorMessageResourceType = typeof(Resources.Publisher.PublisherResource), ErrorMessageResourceName = "CompanyNameMaxError")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resources.Publisher.PublisherResource), ErrorMessageResourceName = "CompanyNameMinError")]
        [RegularExpression("[^:]+$")]
        public string CompanyName { get; set; }

        public string CrossProperty { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Publisher.PublisherResource), ErrorMessageResourceName = "DescriptionRequiredError")]
        [Display(ResourceType = typeof(Resources.Publisher.PublisherResource), Name = "Description")]
        [MinLength(10, ErrorMessageResourceType = typeof(Resources.Publisher.PublisherResource), ErrorMessageResourceName = "DescriptionMinError")]
        [RegularExpression("[^:]+$")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Publisher.PublisherResource), ErrorMessageResourceName = "HomePageRequiredError")]
        [Display(ResourceType = typeof(Resources.Publisher.PublisherResource), Name = "HomePage")]
        [RegularExpression("[^:]+$")]
        public string HomePage { get; set; }

        [Display(ResourceType = typeof(Resources.Publisher.PublisherResource), Name = "Games")]
        public ICollection<GameViewModel> Games { get; set; }

        public PublisherViewModel()
        {
            Games = new List<GameViewModel>();
        }
    }
}