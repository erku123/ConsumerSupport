using System;
using System.Collections.Generic;
using System.Text;
using ConsumerSupport.Entities.Requests;
using ConsumerSupport.Tests.Infrastructure;

namespace ConsumerSupport.Tests.Entities
{
    public class TestRequest
    {
        public static Request Create(
            int id = 855774,
            string title = "undefined", 
            string description = "undefinedDescription", 
            DateTime? datetime = null,
            string userId = "undefinedUserId")
        {
            return new Request(title, description, datetime ?? DateTime.Now, userId)
                .SetPropertyValue(nameof(Request.Id), id);
        }

    }
}
