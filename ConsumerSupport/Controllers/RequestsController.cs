using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConsumerSupport.Models.Requests;
using Microsoft.AspNetCore.Authorization;

namespace ConsumerSupport.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {

        private readonly IRequestCreator _requestCreator;

        public RequestsController(IRequestCreator requestCreator)
        {
            _requestCreator = requestCreator;
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public IActionResult Add(AddRequestViewModel model)
        {

            if (!ModelState.IsValid)
                return View("Add", model);

            _requestCreator.Create(model, User);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Display()
        {
            return NoContent();
        }


    }
}
