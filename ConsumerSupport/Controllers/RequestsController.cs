using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConsumerSupport.Infrastructure.PrincipalExtensions;
using ConsumerSupport.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ConsumerSupport.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {

        private readonly IRequestCreator _requestCreator;
        private readonly IRequestsFinder _requestFinder;
        private readonly IRequestChanger _requestChanger;
        private readonly IRequestAuthorizer _requestAuthorizer;

        public RequestsController(IRequestCreator requestCreator, IRequestsFinder requestsFinder, IRequestChanger requestChanger, IRequestAuthorizer requestAuthorizer)
        {
            _requestCreator = requestCreator;
            _requestFinder = requestsFinder;
            _requestChanger = requestChanger;
            _requestAuthorizer = requestAuthorizer;
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
            return RedirectToAction("List", "Requests");
        }

        public IActionResult List()
        {

            var requests = _requestFinder.FindByUserId(User.GetUserId());

            return View("List", requests);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {

            if (!_requestAuthorizer.CanAccess(id, User.GetUserId()))
                return StatusCode(StatusCodes.Status403Forbidden);

            _requestChanger.DeleteRequest(id);

            return List();
        }

        public IActionResult Edit(int id)
        {

            if (!_requestAuthorizer.CanAccess(id, User.GetUserId()))
                return StatusCode(StatusCodes.Status403Forbidden);

            var changeRequest = _requestChanger.GetChangeRequest(id);

            return View("Edit", changeRequest);
        }

        [HttpPost]
        public IActionResult Edit(ChangeRequestViewModel changeRequest)
        {

            if (!_requestAuthorizer.CanAccess(changeRequest.Id, User.GetUserId()))
                return StatusCode(StatusCodes.Status403Forbidden);

            _requestChanger.EditRequest(changeRequest);

            return List();
        }


    }
}
