
namespace reto2Propietaria.Models
{
    public class TransaccionLog
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string TranscType { get; set; }
        public string EntryDetails { get; set; }
        public string DeductionDetails { get; set; }
        public string Date { get; set; }
        public string Amount { get; set; }
        public byte State { get; set; }
    }
}