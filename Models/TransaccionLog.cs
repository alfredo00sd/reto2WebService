
namespace reto2Propietaria.Models
{
    public class TransaccionLog
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string TranscType { get; set; }
        public string EntryDetails { get; set; }
        public string DeductionDetails { get; set; }
        public string Concept { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public bool State { get; set; }
    }
}