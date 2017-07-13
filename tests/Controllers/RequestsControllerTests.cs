using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerSupport.Controllers;
using ConsumerSupport.Entities.Requests;
using ConsumerSupport.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace tests
{
    public class RequestsControllerTests
    {
        private readonly RequestsController _controller;
        private readonly Mock<IRequestCreator> _requestCreatorMock;
        private readonly Mock<IRequestsFinder> _requestFinderMock;
        private readonly Mock<IRequestChanger> _requestChangerMock;

        public RequestsControllerTests()
        {
            _requestCreatorMock = new Mock<IRequestCreator>();
            _requestFinderMock = new Mock<IRequestsFinder>();
            _requestChangerMock = new Mock<IRequestChanger>();
            _controller = new RequestsController(_requestCreatorMock.Object, _requestFinderMock.Object, _requestChangerMock.Object);
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
            Assert.Equal("Display", result.ActionName);
        }

        [Fact]
        public void Display_Finds_Requests_And_Returns_View()
        {

            // Act
            var result = (ViewResult) _controller.Display();

            // Assert
            _requestFinderMock.Verify(f => f.FindByUser(_controller.User), Times.Once);

            Assert.Equal("Display", result.ViewName);

        }

        [Fact]
        public void Display_Returns_Correct_Model_To_View()
        {

            var dummyList = new List<Request>()
            {
                new Request("", "", new DateTime(), "")
            };

            _requestFinderMock.Setup(f => f.FindByUser(_controller.User)).Returns(dummyList);


            var result = (ViewResult) _controller.Display();

            var resultList = ((List<Request>) result.Model);

            Assert.Equal(dummyList.First(), resultList.First());

        }

        [Fact]
        public void Edit_Returns_Currect_View_And_Model()
        {

            var dummyId = 1;
            var dummyRequest = new ChangeRequestViewModel()
            {
                Id = dummyId
            };

            
            _requestChangerMock.Setup(c => c.GetChangeRequest(dummyId)).Returns(dummyRequest);

            var result = (ViewResult) _controller.Edit(dummyId);

            Assert.Equal(result.ViewName, "Edit");
            Assert.Equal(dummyId, ((ChangeRequestViewModel) result.Model).Id);

            _requestChangerMock.Verify(c => c.GetChangeRequest(dummyId), Times.Once);

        }

    }
}
