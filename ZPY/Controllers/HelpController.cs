using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZPY.Controllers
{
    public class HelpController : BaseController
    {
        //
        // GET: /Help/

        public ActionResult Helps()
        {
            return View();
        }

    }
}
