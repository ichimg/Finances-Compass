using System.ComponentModel.DataAnnotations.Schema;

namespace DebtsCompass.Domain.Entities.Models
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal EurExchangeRate { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal UsdExchangeRate { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
