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
    public class TestableRequestChanger : RequestChanger
    {
        public TestableRequestChanger(ApplicationDbContext context) : base(context)
        {
        }

        protected override Request Find(int Id)
        {
            return new Request("", "", new DateTime(), "");
        }

    }

    public class RequestChangerTests
    {

        private TestableRequestChanger _requestChanger;
        private Mock<ApplicationDbContext> _contextMock;

        public RequestChangerTests()
        {
            _contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _requestChanger = new TestableRequestChanger(_contextMock.Object);
        }

        [Fact]
        public void Updates_Model_And_Saves_To_Database()
        {

            var dummyTitle = "testTitle";
            var dummyDescription = "testDescription";
            var dummyId = 1;

            var changedRequest = new ChangeRequestViewModel()
            {
                Title = dummyTitle,
                Description = dummyDescription,
                Id = dummyId
            };

            // Act
            var savedRequest = _requestChanger.EditRequest(changedRequest);

            //Assert
            Assert.Equal(dummyTitle, savedRequest.Title);
            Assert.Equal(dummyDescription, savedRequest.Description);

            _contextMock.Verify(c => c.SaveChanges(), Times.Once);


        }

        [Fact]
        public void Deletes_Model_And_Saves_To_Database()
        {
            
        }

    }
}
