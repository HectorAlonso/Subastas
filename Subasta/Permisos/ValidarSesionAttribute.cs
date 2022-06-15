using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Subasta.Permisos
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //si en la session usuario del contexto actual es nulo
            if(HttpContext.Current.Session["Usuario"] == null)
            {
                //redireccionamos al login
                filterContext.Result = new RedirectResult("~/User/Login");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}