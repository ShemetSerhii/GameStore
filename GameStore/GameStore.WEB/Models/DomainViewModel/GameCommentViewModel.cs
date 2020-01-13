
namespace GameStore.WEB.Models.DomainViewModel
{
    public class GameCommentViewModel
    {
        public CommentViewModel CommentModel { get; set; }
        public GameViewModel GameViewModel { get; set; }
        public string Id { get; set; }
        public string Quoted { get; set; }
    }
}