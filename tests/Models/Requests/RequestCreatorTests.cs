using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using ConsumerSupport.Data;
using ConsumerSupport.Entities.Requests;
using ConsumerSupport.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ConsumerSupport.Tests.Models.Requests
{
    public class RequestCreatorTests
    {
        private RequestCreator _requestCreator;
        private Mock<ApplicationDbContext> _contextMock;

        public RequestCreatorTests()
        {
                
                _contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
                _requestCreator = new RequestCreator(_contextMock.Object);
        }

        [Fact]
        public void Creates_Request_From_Model_And_Saves_To_Database()
        {
            var deadlineDate = DateTime.Today;
            var deadlineTime = new DateTime().AddSeconds(12345);
            var request = new AddRequestViewModel
            {
                Title = "TestTitle",
                Description = "TestDescription",
                DeadlineDate = deadlineDate,
                DeadlineTime = deadlineTime
            };

            Request createdRequest = null;
            _contextMock.Setup(c => c.Requests.Add(It.IsAny<Request>()))
                .Callback<Request>(r => createdRequest = r);

            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString());
            var identity = new ClaimsIdentity(new[] { userIdClaim });
            var principal = new ClaimsPrincipal(identity);

            // Act
            _requestCreator.Create(request, principal);

            // Assert
            var expectedDeadline = deadlineDate.Add(deadlineTime.TimeOfDay);

            Assert.Equal(request.Title, createdRequest.Title);
            Assert.Equal(request.Description, createdRequest.Description);
            Assert.Equal(expectedDeadline, createdRequest.Deadline);
            Assert.Equal(userIdClaim.Value, createdRequest.UserId);

            _contextMock.Verify(c => c.SaveChanges(), Times.Once);
        }

    }
}
