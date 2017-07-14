using System;
using System.Collections.Generic;
using System.Text;
using ConsumerSupport.Data;
using ConsumerSupport.Entities.Requests;
using ConsumerSupport.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using Xunit;

namespace ConsumerSupport.Tests.Models.Requests
{

    public class RequestChangerTests
    {

        private RequestChanger _requestChanger;
        private Mock<ApplicationDbContext> _contextMock;

        public RequestChangerTests()
        {
            _contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _requestChanger = new RequestChanger(_contextMock.Object);
        }

        [Fact]
        public void EditRequest_Updates_Model_And_Saves_To_Database()
        {

            var requestToBeChanged = new Request("a", "a", new DateTime(), "");

            var changedRequest = new ChangeRequestViewModel()
            {
                Title = "testTitle",
                Description = "testDescription",
                DeadlineTime = DateTime.Now,
                DeadlineDate = DateTime.Today,
                Id = 1
            };

            _contextMock.Setup(c => c.Requests.Find(changedRequest.Id)).Returns(requestToBeChanged);

            // Act
            _requestChanger.EditRequest(changedRequest);

            //Assert
            Assert.Equal(changedRequest.Title, requestToBeChanged.Title);
            Assert.Equal(changedRequest.Description, requestToBeChanged.Description);

            var expectedDeadline = changedRequest.DeadlineDate.Add(changedRequest.DeadlineTime.TimeOfDay);
            Assert.Equal(expectedDeadline, requestToBeChanged.Deadline);

            _contextMock.Verify(c => c.SaveChanges(), Times.Once);


        }

        [Fact]
        public void Deletes_Model_And_Saves_To_Database()
        {

            const int dummyId = 1;
            var dummyRequest = new Request("", "", new DateTime(),"");

            _contextMock.Setup(c => c.Requests.Find(dummyId)).Returns(dummyRequest);

            // Act
            _requestChanger.DeleteRequest(1);

            // Assert
            _contextMock.Verify(c => c.Requests.Remove(It.Is<Request>(r => r == dummyRequest)), Times.Once);
            _contextMock.Verify(c => c.SaveChanges(), Times.Once);

        }

    }
}
