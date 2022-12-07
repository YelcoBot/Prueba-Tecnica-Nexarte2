using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaWebNexarte.Models
{
    public class NominaDefinitiva
    {
        public int Registro { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public int IdConcepto { get; set; }
        public int IdEmpleado { get; set; }
        public decimal ValorNomina { get; set; }
        public int Cantidad { get; set; }
        public string Documento { get; set; }
        public string Usuario { get; set; }

    }
}