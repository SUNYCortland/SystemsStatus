using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemsStatus.Web.Controllers
{
    public class ErrorsController : Controller
    {
        //
        // GET: /Errors/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Errors/NotFound

        public ActionResult NotFound()
        {
            return View();
        }

        //
        // GET: /Errors/NotAuthorized

        public ActionResult NotAuthorized()
        {
            return View();
        }

        //
        // GET: /Errors/InternalServerError

        public ActionResult InternalServerError()
        {
            return View();
        }
    }
}
