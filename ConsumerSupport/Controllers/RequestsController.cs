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
        private readonly IRequestsFinder _requestFinder;
        private readonly IRequestChanger _requestChanger;

        public RequestsController(IRequestCreator requestCreator, IRequestsFinder requestsFinder, IRequestChanger requestChanger)
        {
            _requestCreator = requestCreator;
            _requestFinder = requestsFinder;
            _requestChanger = requestChanger;
        }

        public IActionResult Add()
        {
            return View("Add", new AddRequestViewModel());
        }

        [HttpPost]
        public IActionResult Add(AddRequestViewModel model)
        {

            if (!ModelState.IsValid)
                return View("Add", model);

            _requestCreator.Create(model, User);
            return RedirectToAction("Display", "Requests");
        }

        public IActionResult Display()
        {

            var requests = _requestFinder.FindByUser(User);

            return View("Display", requests);
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            _requestChanger.DeleteRequest(Id);

            return Display();
        }

        public IActionResult Edit(int Id)
        {

            var changeRequest = _requestChanger.GetChangeRequest(Id);

            return View("Edit", changeRequest);
        }

        [HttpPost]
        public IActionResult Edit(ChangeRequestViewModel changeRequest)
        {
            _requestChanger.EditRequest(changeRequest);

            return Display();
        }


    }
}
