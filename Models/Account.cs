using System;

namespace reto2Propietaria.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int AccountNumber { get; set; }
        public DateTime OpenDate { get; set; }
        public int State { get; set; }
    }
}