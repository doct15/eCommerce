using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCommerce.Model;
using eCommerce.UI.Admin.Controllers.Custom;

namespace eCommerce.UI.Admin.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {

            return View();
        }

    }
}
