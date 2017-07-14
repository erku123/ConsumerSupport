using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using ConsumerSupport.Data;
using ConsumerSupport.Entities.Requests;
using ConsumerSupport.Models.Requests;
using ConsumerSupport.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ConsumerSupport.Tests.Models.Requests
{
    public class RequestFinderTests
    {

        private RequestsFinder _requestsFinder;
        private Mock<ApplicationDbContext> _contextMock;

        public RequestFinderTests()
        {
            _contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _requestsFinder = new RequestsFinder(_contextMock.Object);
        }

        [Fact]
        public void Find_By_User_Returns_Correct_Requests()
        {
            var userId = UniqueString.Next;
            var expectedRequest1 = new Request(UniqueString.Next, UniqueString.Next, DateTime.Now, userId);
            var expectedRequest2 = new Request(UniqueString.Next, UniqueString.Next, DateTime.Now, userId);
            var notExpectedRequest = new Request(UniqueString.Next, UniqueString.Next, DateTime.Now, UniqueString.Next);

            _contextMock.Setup(c => c.GetRequests())
                .Returns(new[] {expectedRequest1, notExpectedRequest, expectedRequest2}.AsQueryable());

            // act
            var result = _requestsFinder.FindByUserId(userId);

            // Assert
            Assert.Equal(result.Length, 2);
            Assert.Contains(expectedRequest1, result);
            Assert.Contains(expectedRequest2, result);
        }       
    }
}
