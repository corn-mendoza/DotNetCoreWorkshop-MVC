using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DotNetCoreWorkshop_MVC.Models;

namespace DotNetCoreWorkshop_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var _index = Environment.GetEnvironmentVariable("INSTANCE_INDEX");
            if (_index == null)
            {
                _index = "Running Local";
            }

            var _prodmode = Environment.GetEnvironmentVariable("PROD_MODE");
            if (_prodmode == null)
            {
                _prodmode = "Development";
            }

            var _port = Environment.GetEnvironmentVariable("PORT");
            if (_port == null)
            {
                _port = "localhost";
            }

            ViewBag.Index = $"Application Index: {_index}";
            ViewBag.ProdMode = $"Production Mode: {_prodmode}";
            ViewBag.Port = $"Port: {_port}";
            ViewBag.Uptime = $"Uptime: {DateTime.Now.Subtract(TimeSpan.FromMilliseconds(Environment.TickCount)).ToString() }";
            ViewBag.WorkshopUrl = Environment.GetEnvironmentVariable("WORKSHOP_URL");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public ActionResult KillMe()
        {
            Environment.Exit(-1);

            return View();
        }

        [Authorize]

        public IActionResult Secured()
        {
            ViewData["Message"] = "This is a secured page";

            return View();
        }


        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
