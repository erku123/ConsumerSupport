using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerSupport.Controllers;
using ConsumerSupport.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Xunit;
namespace tests
{
    public class RequestsControllerTests
    {
        private RequestsControllerTests _controller;

        public RequestsControllerTests()
        {
                _controller = new RequestsControllerTests();
        }

        [Fact]
        public void True_Equals_True()
        {
            Assert.Equal(true, true);
        }

        [Fact]
        public void Add_Returns_Error_When_Model_State_Is_Invalid()
        {
            _controller.ModelState.AddModelError("", "error");

            var addRequestViewModel = new AddRequestViewModel();

            // Act 
            var result = _controller.Add(addRequestViewModel) as ViewResult;

            // Assert
            Assert.Same(result.Model, addRequestViewModel);
        }
    }
}
