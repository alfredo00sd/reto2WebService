
using System;

namespace reto2Propietaria.Models
{
    public class AcountingSeat
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Account { get; set; }
        //Tipo de movimiento debito, credito
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public int State { get; set; }
        
    }
}