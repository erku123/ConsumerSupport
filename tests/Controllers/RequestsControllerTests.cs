using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerSupport.Controllers;
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

        public RequestsControllerTests()
        {
            _requestCreatorMock = new Mock<IRequestCreator>();
            _controller = new RequestsController(_requestCreatorMock.Object);
        }

        [Fact]
        public void True_Equals_True()
        {
            Assert.Equal(true, true);
        }

        [Fact]
        public void Add_Returns_View_With_Correct_View_Name()
        {
            var result = _controller.Add() as ViewResult;

            Assert.Equal("Add", result.ViewName);
        }

        [Fact]
        public void Add_Returns_Error_When_Model_State_Is_Invalid()
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

            Assert.Equal("Home", result.ControllerName);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
