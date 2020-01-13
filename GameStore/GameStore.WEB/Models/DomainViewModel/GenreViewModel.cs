using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models
{
    public class GenreViewModel
    {
        public int Id { get; set; }

        public string CrossProperty { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Genre.GenreResource), ErrorMessageResourceName = "NameRequiredError")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resources.Genre.GenreResource), ErrorMessageResourceName = "NameMinError")]
        [Display(ResourceType = typeof(Resources.Genre.GenreResource), Name = "Name")]
        public string Name { get; set; }

        public string NameRu { get; set; }

        public string NameTranslate { get; set; }

        public int? ParentId { get; set; }

        public GenreViewModel Parent { get; set; }

        public ICollection<GameViewModel> Games { get; set; }

        public ICollection<GenreViewModel> ChildrenGenres { get; set; }

        public GenreViewModel()
        {
            Games = new List<GameViewModel>();
            ChildrenGenres = new List<GenreViewModel>();
        }
    }
}