using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using ConsumerSupport.Data;
using ConsumerSupport.Entities.Requests;
using ConsumerSupport.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ConsumerSupport.Tests.Models.Requests
{

    public class TestableRequestFinder : RequestsFinder
    {
        public TestableRequestFinder(ApplicationDbContext context) : base(context)
        {
        }

        protected override List<Request> GetAll()
        {
            var result = new List<Request>();

            result.Add(new Request("testTitle1", "testDescription1", new DateTime(), "1"));
            result.Add(new Request("testTitle2", "testDescription2", new DateTime(), "2"));
            result.Add(new Request("testTitle3", "testDescription3", new DateTime(), "3"));

            return result;
        }
    }

    public class RequestFinderTests
    {

        private TestableRequestFinder _requestsFinder;
        private Mock<ApplicationDbContext> _contextMock;

        public RequestFinderTests()
        {
            _contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _requestsFinder = new TestableRequestFinder(_contextMock.Object);
        }

        [Fact]
        public void Find_By_User_Returns_Correct_Requests()
        {

            // act
            var result = _requestsFinder.FindByUserId("1");

            // Assert
            Assert.Equal(1, result.Count);
            Assert.Equal("testTitle1", result.First().Title);



        }
        


    }
}
