using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using ConsumerSupport.Infrastructure.PrincipalExtensions;
using Xunit;

namespace ConsumerSupport.Tests.Infrastructure
{
    public class PrincipalExtensionsTests
    {

        [Fact]
        public void Get_User_Id_Returns_User_Id_From_Claims()
        {

            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString());
            var identity = new ClaimsIdentity(new []{userIdClaim});
            var principal = new ClaimsPrincipal(identity);

            // Act
            var result = principal.GetUserId();

            // Assert
            Assert.Equal(userIdClaim.Value, result);

        }

    }
}
