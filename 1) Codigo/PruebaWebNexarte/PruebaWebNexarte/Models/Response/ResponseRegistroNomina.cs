using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaWebNexarte.Models.Response
{
    public class ResponseRegistroNomina
    {
        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public string DescripcionError { get; set; }
        public string StackTrace { get; set; }
        public NominaDefinitiva Data { get; set; }
    }

}