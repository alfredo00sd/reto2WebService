
namespace reto2Propietaria.Models
{
    public class TransaccionLog
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public byte TranscType { get; set; }
        public string Date { get; set; }
        public string Amount { get; set; }
        public byte State { get; set; }
    }
}