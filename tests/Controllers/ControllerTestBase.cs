using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ConsumerSupport.Tests.Controllers
{
    public class ControllerTestBase
    {
        public Mock<ControllerContext> GetControllerContextMock(string userId)
        {
            var idClaim = new Claim(ClaimTypes.NameIdentifier, userId);
            var claimsIdentity = new ClaimsIdentity(new []{idClaim});
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(claimsPrincipal);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Object.HttpContext = httpContextMock.Object;
            return controllerContextMock;
        }
    }
}
