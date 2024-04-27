using Taka.App.Deliverer.Domain.Enums;
using Taka.Common.Middlewares.Enums;

namespace Taka.App.Deliverer.Domain.Entities
{
    public class PersonDelivery
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateOnly BirthDate { get; set; }

        public string CNHNumber { get; set; }
        public CnhValidType CNHType { get; set; }        
        public string CNHImageUrl { get; set; }
        
        private DateTime _birthDate;
    }
}
