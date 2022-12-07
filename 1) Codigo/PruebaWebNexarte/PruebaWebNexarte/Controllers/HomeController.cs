using PruebaWebNexarte.Models;
using PruebaWebNexarte.Models.Response;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace PruebaWebNexarte.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            List<SelectListItem> OptionsConceptos = ConceptosNomina?.Select(con => new SelectListItem()
            {
                Text = $@"{con?.COD_CONCEPTO} - {con?.DESC_CONCEPTO}",
                Value = con?.ID_CONCEPTO.ToString()
            })?.ToList();

            List<SelectListItem> OptionsSolicitudes = SolicitudesIngresos?.SelectMany(soli => soli.NOM_EMPLEADOS, (soli, empl) => new SelectListItem()
            {
                Text = $@"{soli?.DOCUMENTO} - {soli?.NOMBRES} {soli?.APELLIDOS}",
                Value = empl?.ID_EMPLEADO.ToString()
            })?.ToList();

            ViewBag.OptionsConceptos = OptionsConceptos;
            ViewBag.OptionsSolicitudes = OptionsSolicitudes;

            return View();
        }

        public ActionResult Search(string documento, string usuario)
        {
            List<List<string>> Data = new List<List<string>>();

            try
            {
                object[] Parametros = new object[] {
                    new SqlParameter ("@Doc",documento),
                    new SqlParameter("@Event",1),
                    new SqlParameter("@User",usuario),
                };

                var Nominas = db.Database.SqlQuery<USP_CONSULTA_NOMINA_POR_DOCUMENTO_Result>("EXEC [dbo].[USP_CONSULTA_NOMINA_POR_DOCUMENTO] @Doc,@Event,@User", Parametros).ToList();

                if ((Nominas?.Count ?? 0) > 0)
                {

                    foreach (var Nomina in Nominas)
                    {
                        string Buttons = "<button type=\"button\" id=\"btn-edit\" documento=\"" + Nomina.DOCUMENTO + "\" registro=\"" + Nomina.REGISTRO + "\" class=\"btn btn-primary\">Editar</button>";
                        Buttons += "<button type=\"button\" id=\"btn-remove\" documento=\"" + Nomina.DOCUMENTO + "\" registro=\"" + Nomina.REGISTRO + "\" class=\"btn btn-danger\">Eliminar</button>";

                        List<string> Row = new List<string>();

                        Row.Add(Nomina?.REGISTRO.ToString());
                        Row.Add(Nomina?.COD_CONCEPTO);
                        Row.Add(Nomina?.DESC_CONCEPTO);
                        Row.Add(Nomina?.ID_EMPLEADO.ToString());
                        Row.Add(Nomina?.DOCUMENTO);
                        Row.Add(Nomina?.APELLIDOS);
                        Row.Add(Nomina?.NOMBRES);
                        Row.Add(Nomina?.VAL_NOMINA.ToString("C", CultureInfo.CreateSpecificCulture("es-CO")));
                        Row.Add(Nomina?.CANTIDAD.ToString());
                        Row.Add(Buttons);

                        Data.Add(Row);
                    }
                }
            }
            catch { }

            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get(int Registro)
        {
            ResponseRegistroNomina Response = new ResponseRegistroNomina();

            try
            {
                NominaDefinitiva RegistroNomina = db.NOM_NOMINA_DEFINITIVA?.Where(nomi => nomi.REGISTRO.Equals(Registro))?.Select(nomi => new NominaDefinitiva()
                {
                    Registro = nomi.REGISTRO,
                    IdConcepto = nomi.ID_CONCEPTO,
                    IdEmpleado = nomi.ID_EMPLEADO,
                    ValorNomina = nomi.VAL_NOMINA,
                    Cantidad = nomi.CANTIDAD
                })?.FirstOrDefault();

                Response.Tipo = "OK";
                Response.Mensaje = "Registro consultado correctamente!";
                Response.Data = RegistroNomina;
            }
            catch (Exception ex)
            {
                Response.Tipo = "Error";
                Response.Mensaje = "Error al consultar el registro!";
                Response.DescripcionError = ex?.Message;
                Response.StackTrace = ex?.StackTrace;
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(NominaDefinitiva Request)
        {
            ResponseRegistroNomina Response = new ResponseRegistroNomina();

            try
            {
                object[] Parametros = new object[] {
                    new SqlParameter ("@Doc",Request.Documento),
                    new SqlParameter("@Event",2),
                    new SqlParameter("@User",Request.Usuario),
                    new SqlParameter("@Regt",Request.Registro),
                    new SqlParameter("@IdConcep",Request.IdConcepto),
                    new SqlParameter("@ValorNomin",Request.ValorNomina),
                    new SqlParameter("@Cant",Request.Cantidad),
                };

                db.Database.ExecuteSqlCommand("EXEC [dbo].[USP_CONSULTA_NOMINA_POR_DOCUMENTO] @Doc,@Event,@User,@Regt,@IdConcep,@ValorNomin,@Cant", Parametros);

                Response.Tipo = "OK";
                Response.Mensaje = "Registro actualizado correctamente!";
            }
            catch (Exception ex)
            {
                Response.Tipo = "Error";
                Response.Mensaje = "Error al actualizar el registro!";
                Response.DescripcionError = ex?.Message;
                Response.StackTrace = ex?.StackTrace;
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(NominaDefinitiva Request)
        {
            ResponseRegistroNomina Response = new ResponseRegistroNomina();

            try
            {
                object[] Parametros = new object[] {
                    new SqlParameter ("@Doc",Request.Documento),
                    new SqlParameter("@Event",3),
                    new SqlParameter("@User",Request.Usuario),
                    new SqlParameter("@Regt",Request.Registro),
                };

                db.Database.ExecuteSqlCommand("EXEC [dbo].[USP_CONSULTA_NOMINA_POR_DOCUMENTO] @Doc,@Event,@User,@Regt", Parametros);

                Response.Tipo = "OK";
                Response.Mensaje = "Registro eliminado correctamente!";
            }
            catch (Exception ex)
            {
                Response.Tipo = "Error";
                Response.Mensaje = "Error al eliminar el registro!";
                Response.DescripcionError = ex?.Message;
                Response.StackTrace = ex?.StackTrace;
            }

            return Json(Response, JsonRequestBehavior.AllowGet);
        }

    }
}