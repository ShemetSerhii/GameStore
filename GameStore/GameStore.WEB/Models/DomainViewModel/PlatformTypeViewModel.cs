using System.Collections.Generic;

namespace GameStore.WEB.Models
{
    public class PlatformTypeViewModel
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public ICollection<GameViewModel> Games { get; set; }

        public PlatformTypeViewModel()
        {
            Games = new List<GameViewModel>();
        }
    }
}