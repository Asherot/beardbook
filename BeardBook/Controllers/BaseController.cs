using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;

namespace BeardBook.Controllers
{
    public class BaseController : Controller
    {
        protected internal ActionResult RedirectToLocal(string returnUrl) =>
            Url.IsLocalUrl(returnUrl) 
            ? (ActionResult)Redirect(returnUrl)
            : RedirectToAction("Index", "Home");

        protected internal RedirectToRouteResult RedirectToHome() => 
            RedirectToAction("Index", "Home");

        protected internal void AddErrorsTo(ModelStateDictionary modelState, IEnumerable<string> errors) => 
            errors.ForEach(e => modelState.AddModelError("", e));
    }
}