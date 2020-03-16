using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UnauthorizedCustom()
        {
            return RedirectToAction("Index", "Admin");
        }

        public IActionResult BadRequestCustom()
        {
            return View();
        }
    }
}