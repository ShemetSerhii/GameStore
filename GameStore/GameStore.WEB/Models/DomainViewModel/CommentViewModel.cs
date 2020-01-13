using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore.WEB.Models
{
    public class CommentViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsQuoted { get; set; }

        public string CrossProperty { get; set; }

        [Display(ResourceType = typeof(Resources.Comment.CommentResource), Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Comment.CommentResource), ErrorMessageResourceName = "BodyRequiredError")]
        [Display(ResourceType = typeof(Resources.Comment.CommentResource), Name = "Body")]
        public string Body { get; set; }

        public int Order { get; set; }

        public int? ParentId { get; set; }

        public CommentViewModel ParentComment { get; set; }

        public ICollection<CommentViewModel> ChildrenComments { get; set; }

        public int GameId { get; set; }

        public GameViewModel Game { get; set; }

        public CommentViewModel()
        {
            ChildrenComments = new List<CommentViewModel>();
        }
    }
}