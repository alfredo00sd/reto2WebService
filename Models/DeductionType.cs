using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reto2Propietaria.Models
{
    public class DeductionType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte Salarydependent { get; set; }
        public byte State { get; set; }

    }
}