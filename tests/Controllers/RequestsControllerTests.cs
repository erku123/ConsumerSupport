using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerSupport.Controllers;
using ConsumerSupport.Entities.Requests;
using ConsumerSupport.Infrastructure.PrincipalExtensions;
using ConsumerSupport.Models.Requests;
using ConsumerSupport.Tests.Controllers;
using ConsumerSupport.Tests.Entities;
using ConsumerSupport.Tests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace tests
{
    public class RequestsControllerTests : ControllerTestBase
    {
        private readonly RequestsController _controller;
        private readonly Mock<IRequestCreator> _requestCreatorMock;
        private readonly Mock<IRequestsFinder> _requestFinderMock;
        private readonly Mock<IRequestChanger> _requestChangerMock;
        private readonly Mock<IRequestAuthorizer> _requestAuthorizerMock;
        private string _userId = UniqueString.Next;

        public RequestsControllerTests()
        {
            _requestCreatorMock = new Mock<IRequestCreator>();
            _requestFinderMock = new Mock<IRequestsFinder>();
            _requestChangerMock = new Mock<IRequestChanger>();
            _requestAuthorizerMock = new Mock<IRequestAuthorizer>();
            
            _controller = new RequestsController(_requestCreatorMock.Object, _requestFinderMock.Object, _requestChangerMock.Object, _requestAuthorizerMock.Object)
            {
                ControllerContext = GetControllerContextMock(_userId).Object
            };
        }

        [Fact]
        public void Add_Returns_View_With_Correct_View_Name()
        {
            var result = _controller.Add() as ViewResult;

            Assert.Equal("Add", result.ViewName);
        }

        [Fact]
        public void Add_Returns_To_Add_When_Model_State_Is_Invalid()
        {
            _controller.ModelState.AddModelError("", "error");

            var addRequestViewModel = new AddRequestViewModel();

            // Act 
            var result = _controller.Add(addRequestViewModel) as ViewResult;

            // Assert
            Assert.Equal("Add", result.ViewName);
            Assert.Same(addRequestViewModel, result.Model);
        }

        [Fact]
        public void Add_Creates_New_Request_And_Redirects_To_Home()
        {
            var addRequestViewModel = new AddRequestViewModel();

             // Act
            var result = (RedirectToActionResult) _controller.Add(addRequestViewModel);

            // Assert
            _requestCreatorMock.Verify(c => c.Create(addRequestViewModel, _controller.User), Times.Once);

            Assert.Equal("Requests", result.ControllerName);
            Assert.Equal("List", result.ActionName);
        }

        [Fact]
        public void List_Finds_Requests_And_Returns_View()
        {

            // Act
            var result = (ViewResult) _controller.List();

            // Assert
            _requestFinderMock.Verify(f => f.FindByUserId(_userId), Times.Once);

            Assert.Equal("List", result.ViewName);

        }

        [Fact]
        public void List_Returns_Correct_Model_To_View()
        {

            var dummyList = new []
            {
                new Request("", "", new DateTime(), "")
            };

            _requestFinderMock.Setup(f => f.FindByUserId(_userId)).Returns(dummyList);


            var result = (ViewResult) _controller.List();

            var resultList = ((Request[]) result.Model);

            Assert.Equal(dummyList.First(), resultList.First());
            Assert.Equal("List", result.ViewName);

        }

        [Fact]
        public void EditPost_Returns_Currect_View_And_Model()
        {

            var dummyId = 1;
            var dummyRequest = new ChangeRequestViewModel()
            {
                Id = dummyId
                
            };

            _requestAuthorizerMock.Setup(c => c.CanAccess(dummyId, _userId)).Returns(true);
            _requestChangerMock.Setup(c => c.GetChangeRequest(dummyId)).Returns(dummyRequest);

            var result = (ViewResult) _controller.Edit(dummyId);

            Assert.Equal(result.ViewName, "Edit");
            Assert.Equal(dummyId, ((ChangeRequestViewModel) result.Model).Id);

            _requestChangerMock.Verify(c => c.GetChangeRequest(dummyId), Times.Once);

        }

        [Fact]
        public void Edit_Returns_403Forbidden_When_Unauthorized()
        {

            var requestId = 21421;

            _requestAuthorizerMock.Setup(c => c.CanAccess(requestId, _userId)).Returns(false);

            // Act
            var result = (StatusCodeResult) _controller.Edit(requestId);

            // Assert
            Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);

        }

        [Fact]
        public void EditPost_Returns_403Forbidden_When_Unauthorized()
        {

            var request = new ChangeRequestViewModel()
            {
                Id = 123124
            };

            _requestAuthorizerMock.Setup(c => c.CanAccess(request.Id, _userId)).Returns(false);

            // Act
            var result = (StatusCodeResult)_controller.Edit(request);

            // Assert
            Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);

        }

        [Fact]
        public void Delete_Returns_403Forbidden_When_Unauthorized()
        {

            var requestId = 21421;

            _requestAuthorizerMock.Setup(c => c.CanAccess(requestId, _userId)).Returns(false);

            // Act
            var result = (StatusCodeResult)_controller.Delete(requestId);

            // Assert
            Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);

        }
    }
}
