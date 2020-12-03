using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reto2Propietaria.API
{
    public class AsientoFromAPI
    {
        private int noAsiento { get; set; }
        private string descripcion { get; set; }
        private string periodoInicio { get; set; }
        private string periodoFin { get; set; }
        private int auxiliar { get; set; }
        private string fecha { get; set; }
        private string estado { get; set; }
        private int cuenta { get; set; }
        private string moneda { get; set; }
        private string tasaCambio { get; set; }
        private string descripcionCuenta { get; set; }
        private string tipoMovimiento { get; set; }
    }
}