using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proc.Controllers;

namespace Proc1.Controllers
{
    public class WebSetController : BaseController
    {
        //
        // GET: /WebSet/

        public ActionResult Member()
        {
            return View();
        }
        public ActionResult Advert()
        {
            return View();
        }

    }
}
