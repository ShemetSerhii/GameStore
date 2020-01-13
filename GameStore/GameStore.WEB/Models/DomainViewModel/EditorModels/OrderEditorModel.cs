using System.Collections.Generic;
using System.Web.Mvc;

namespace GameStore.WEB.Models.DomainViewModel.EditorModels
{
    public class OrderEditorModel
    {
        public OrderEditModel OrderModel { get; set; }
        public List<SelectListItem> Shippers { get; set; }
        public List<SelectListItem> OrderStatuses { get; set; }
    }
}