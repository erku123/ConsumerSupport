using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerSupport.Models.Requests;
using Microsoft.AspNetCore.Authorization;

namespace ConsumerSupport.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {

        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Add(AddRequestViewModel model)
        {


            return RedirectToAction("Index", "Home");
        }


    }
}
