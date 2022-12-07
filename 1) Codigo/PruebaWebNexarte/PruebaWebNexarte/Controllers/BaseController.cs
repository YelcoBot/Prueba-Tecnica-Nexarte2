using PruebaWebNexarte.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace PruebaWebNexarte.Controllers
{
    public class BaseController : Controller
    {
        private Double TimeCache = Convert.ToDouble(ConfigurationManager.AppSettings["TiempoCache"]);

        protected PruebaNexarteEntities db = new PruebaNexarteEntities();
        public List<NOM_CONCEPTOS_NOMINA> ConceptosNomina
        {
            get
            {
                List<NOM_CONCEPTOS_NOMINA> Conceptos = (List<NOM_CONCEPTOS_NOMINA>)HttpRuntime.Cache.Get("ConceptosNomina");
                if ((Conceptos?.Count ?? 0) < 1)
                {
                    HttpRuntime.Cache.Remove("ConceptosNomina");
                    HttpRuntime.Cache.Insert("ConceptosNomina", db.NOM_CONCEPTOS_NOMINA.ToList(), null, DateTime.Now.AddMinutes(TimeCache), Cache.NoSlidingExpiration);
                    Conceptos = (List<NOM_CONCEPTOS_NOMINA>)HttpRuntime.Cache.Get("ConceptosNomina");
                }
                return Conceptos;
            }
        }
        public List<REG_SOLICITUDES_INGRESOS> SolicitudesIngresos
        {
            get
            {
                List<REG_SOLICITUDES_INGRESOS> Solicitudes = (List<REG_SOLICITUDES_INGRESOS>)HttpRuntime.Cache.Get("SolicitudesIngresos");
                if ((Solicitudes?.Count ?? 0) < 1)
                {
                    HttpRuntime.Cache.Remove("SolicitudesIngresos");
                    HttpRuntime.Cache.Insert("SolicitudesIngresos", db.REG_SOLICITUDES_INGRESOS.ToList(), null, DateTime.Now.AddMinutes(TimeCache), Cache.NoSlidingExpiration);
                    Solicitudes = (List<REG_SOLICITUDES_INGRESOS>)HttpRuntime.Cache.Get("SolicitudesIngresos");
                }
                return Solicitudes;
            }
        }
    }
}