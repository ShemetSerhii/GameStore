using GameStore.Domain.Entities.Interfaces;

namespace GameStore.Domain.Entities
{
    public class Shipper : Entity
    {
        public string CompanyName { get; set; }

        public string Phone { get; set; }
    }
}
